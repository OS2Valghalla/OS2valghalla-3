using ITfoxtec.Identity.Saml2;
using ITfoxtec.Identity.Saml2.MvcCore;
using ITfoxtec.Identity.Saml2.Schemas;
using ITfoxtec.Identity.Saml2.Schemas.Metadata;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Diagnostics;
using System.Security.Authentication;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using System.Xml.Serialization;
using Valghalla.Application.Authentication;
using Valghalla.Application.Cache;
using Valghalla.Application.Configuration;
using Valghalla.Application.Saml;
using Valghalla.Application.Tenant;
using Valghalla.Integration.Auth;

namespace Valghalla.Integration.Saml
{
    internal class Saml2AuthService : ISaml2AuthService
    {
        private const string RELAY_STATE_REDIRECT = "redirect";
        private const string RELAY_STATE_PROFILE_DELETED = "profiledeleted";
        private const string COOKIE_STATE = "valghalla.signedin";

        private readonly IOptions<GlobalAuthConfiguration> authConfigOptions;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IWebHostEnvironment environment;
        private readonly ITenantMemoryCache tenantMemoryCache;
        private readonly ITenantContextProvider tenantContextProvider;
        private readonly ISaml2AuthContextProvider saml2AuthContextProvider;
        private readonly InternalAuthConfiguration configuration;
        private readonly ILogger<Saml2AuthService> logger;

        private HttpContext HttpContext
        {
            get
            {
                return httpContextAccessor.HttpContext!;
            }
        }

        private string RootUrl
        {
            get
            {
                return $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}";
            }
        }

        public Saml2AuthService(
            IOptions<GlobalAuthConfiguration> authConfigOptions,
            IHttpContextAccessor httpContextAccessor,
            IWebHostEnvironment environment,
            ITenantMemoryCache tenantMemoryCache,
            ITenantContextProvider tenantContextProvider,
            ISaml2AuthContextProvider saml2AuthContextProvider,
            InternalAuthConfiguration configuration,
            ILogger<Saml2AuthService> logger)
        {
            this.authConfigOptions = authConfigOptions;
            this.httpContextAccessor = httpContextAccessor;
            this.environment = environment;
            this.tenantMemoryCache = tenantMemoryCache;
            this.tenantContextProvider = tenantContextProvider;
            this.saml2AuthContextProvider = saml2AuthContextProvider;
            this.configuration = configuration;
            this.logger = logger;
        }

        public bool IsAuthenticated()
        {
            return
                HttpContext.User.Identity != null &&
                HttpContext.User.Identity.IsAuthenticated;
        }

        public void ClearClientSession()
        {
            foreach (var cookie in HttpContext.Request.Cookies)
            {
                HttpContext.Response.Cookies.Delete(cookie.Key);
            }
        }

        public void SaveClientSession()
        {
            HttpContext.Response.Cookies.Append(COOKIE_STATE, "true");
        }

        public async Task<string> GetLoginRedirectUrlAsync(CancellationToken cancellationToken)
        {
            var saml2Config = await GetSaml2ConfigurationAsync(cancellationToken);
            var binding = new Saml2RedirectBinding();

            binding.SetRelayStateQuery(new Dictionary<string, string>
            {
                { RELAY_STATE_REDIRECT, string.Empty }
            });

            binding.Bind(new Saml2AuthnRequest(saml2Config));

            return binding.RedirectLocation.OriginalString;
        }

        public async Task<string> LogoutAsync(bool profileDeleted, CancellationToken cancellationToken)
        {
            var saml2Config = await GetSaml2ConfigurationAsync(cancellationToken);
            var binding = new Saml2RedirectBinding();

            if (profileDeleted)
            {
                binding.SetRelayStateQuery(new Dictionary<string, string>
                {
                    { RELAY_STATE_PROFILE_DELETED, string.Empty }
                });
            }

            var saml2LogoutRequest = await new Saml2LogoutRequest(saml2Config, HttpContext.User).DeleteSession(HttpContext);
            binding.Bind(saml2LogoutRequest);

            return binding.RedirectLocation.OriginalString;
        }

        public async Task<string> SetupAssertionConsumerServiceAsync(Func<ClaimsPrincipal, ClaimsPrincipal> transform, bool isInternal, CancellationToken cancellationToken)
        {
            var saml2Config = await GetSaml2ConfigurationAsync(cancellationToken);
            var binding = new Saml2PostBinding();
            var saml2AuthnResponse = new Saml2AuthnResponse(saml2Config);

            binding.ReadSamlResponse(HttpContext.Request.ToGenericHttpRequest(), saml2AuthnResponse);

            if (saml2AuthnResponse.Status != Saml2StatusCodes.Success)
            {
                logger.LogError(saml2AuthnResponse.ToXml().OuterXml);
                throw new AuthenticationException($"SAML Response status: {saml2AuthnResponse.Status}");
            }

            binding.Unbind(HttpContext.Request.ToGenericHttpRequest(), saml2AuthnResponse);

            await CreateSession(saml2AuthnResponse, transform, isInternal);

            var relayStateQuery = binding.GetRelayStateQuery();

            if (relayStateQuery.ContainsKey(RELAY_STATE_REDIRECT))
            {
                var returnValue = (environment.IsDevelopment() ? tenantContextProvider.CurrentTenant.AngularDevServer : RootUrl) + "?" + RELAY_STATE_REDIRECT + "=true";
                return returnValue;
            }

            return RootUrl;
        }

        public async Task<string> SetupLogoutResponseAsync(string logoutPath, CancellationToken cancellationToken)
        {
            var saml2Config = await GetSaml2ConfigurationAsync(cancellationToken);
            var genericHttpRequest = HttpContext.Request.ToGenericHttpRequest();

            if (new Saml2PostBinding().IsResponse(genericHttpRequest) || new Saml2RedirectBinding().IsResponse(genericHttpRequest))
            {
                // Logged out response
                var binding = new Saml2RedirectBinding();
                binding.Unbind(genericHttpRequest, new Saml2LogoutResponse(saml2Config));

                var rootUrl = environment.IsDevelopment() ? tenantContextProvider.CurrentTenant.AngularDevServer! : RootUrl;
                var logoutLandingUrl = rootUrl.TrimEnd('/') + logoutPath;

                var relayStateQuery = binding.GetRelayStateQuery();

                if (relayStateQuery.ContainsKey(RELAY_STATE_PROFILE_DELETED))
                {
                    logoutLandingUrl += "?profile-deleted=true";
                }

                return await Task.FromResult(logoutLandingUrl);
            }
            else
            {
                // Single logout response
                Saml2StatusCodes status;
                var requestBinding = new Saml2RedirectBinding();
                var logoutRequest = new Saml2LogoutRequest(saml2Config, HttpContext.User);
                try
                {
                    requestBinding.Unbind(genericHttpRequest, logoutRequest);
                    status = Saml2StatusCodes.Success;
                    await logoutRequest.DeleteSession(HttpContext);
                }
                catch (Exception exc)
                {
                    // log exception
                    Debug.WriteLine("SingleLogout error: " + exc.ToString());
                    status = Saml2StatusCodes.RequestDenied;
                }

                var responsebinding = new Saml2RedirectBinding();
                responsebinding.RelayState = requestBinding.RelayState;

                var saml2LogoutResponse = new Saml2LogoutResponse(saml2Config)
                {
                    InResponseToAsString = logoutRequest.IdAsString,
                    Status = status,
                };

                responsebinding.Bind(saml2LogoutResponse);

                return responsebinding.RedirectLocation.OriginalString;
            }
        }

        private async Task CreateSession(Saml2AuthnResponse saml2AuthnResponse, Func<ClaimsPrincipal, ClaimsPrincipal> transform, bool isInternal)
        {
            if (HttpContext.Request.Cookies.Any())
            {
                await Task.CompletedTask;
                return;
            }

            await saml2AuthnResponse.CreateSession(HttpContext, claimsTransform: (claimsPrincipal) =>
            {
                logger.LogInformation("Claims");
                foreach (Claim c in claimsPrincipal.Claims)
                {
                    string tmp = c.Type + " :: " + c.Value + " :: " + c.ValueType;
                    logger.LogInformation(tmp);
                }
                logger.LogInformation("--------");

                if (isInternal)
                    CheckJobRoleDefinition(claimsPrincipal);

                CheckAssurance(claimsPrincipal);
                return transform(claimsPrincipal);
            });
        }

        private static ClaimsPrincipal CheckAssurance(ClaimsPrincipal claimsPrincipal)
        {
            var nsisLevelAccepted = claimsPrincipal.Claims.Where(c => c.Type == OioSaml3ClaimTypes.NsisLoa && (c.Value == NsisLevels.Substantial || c.Value == NsisLevels.High)).Any();
            var oldAssuranceLevelAccepted = claimsPrincipal.Claims.Where(c => c.Type == OioSaml2ClaimTypes.AssuranceLevel && Convert.ToInt32(c.Value) >= 3).Any();

            if (!nsisLevelAccepted && !oldAssuranceLevelAccepted)
            {
                throw new Exception("Assurance level not accepted.");
            }

            return claimsPrincipal;
        }

        private void CheckJobRoleDefinition(ClaimsPrincipal claimsPrincipal)
        {
            if (string.IsNullOrEmpty(configuration.JobRoleDescription))
                throw new UnauthorizedAccessException("Missing authentication configuration");

            var privilegeClaimV2 = claimsPrincipal.Claims.FirstOrDefault(x => x.Type == OioSaml2ClaimTypes.PrivilegesIntermediate && !string.IsNullOrEmpty(x.Value));
            var privilegeClaimV3 = claimsPrincipal.Claims.FirstOrDefault(x => x.Type == OioSaml3ClaimTypes.PrivilegesIntermediate && !string.IsNullOrEmpty(x.Value));
            var privilegeClaim = privilegeClaimV2 ??= privilegeClaimV3;

            if (privilegeClaim is null)
                throw new UnauthorizedAccessException("You are not authorized to the system (no job function role info supplied with claim)");

            byte[] data = Convert.FromBase64String(privilegeClaim.Value);
            string decodedString = System.Text.Encoding.UTF8.GetString(data);

            logger.LogInformation("decodedString");
            logger.LogInformation(decodedString);
            logger.LogInformation("--------");

            if (decodedString.Contains("http://digst.dk"))
            {
                var serializer = new XmlSerializer(typeof(AuthObjectsV3.PrivilegeList));
                var serializedObject = (AuthObjectsV3.PrivilegeList)serializer.Deserialize(new StringReader(decodedString));

                logger.LogInformation("Claim JobFunctionRole");
                logger.LogInformation(serializedObject.PrivilegeGroup.Privilege);
                logger.LogInformation("--------");
                logger.LogInformation("Configuration JobFunctionRole");
                logger.LogInformation(configuration.JobRoleDescription);
                logger.LogInformation("--------");

                if (configuration.JobRoleDescription != serializedObject.PrivilegeGroup.Privilege)
                    throw new UnauthorizedAccessException("You are not authorized to the system");
            }
            else
            {
                var serializer = new XmlSerializer(typeof(AuthObjects.PrivilegeList));
                var serializedObject = (AuthObjects.PrivilegeList)serializer.Deserialize(new StringReader(decodedString));

                logger.LogInformation("Claim JobFunctionRole");
                logger.LogInformation(serializedObject.PrivilegeGroup.Privilege);
                logger.LogInformation("--------");
                logger.LogInformation("Configuration JobFunctionRole");
                logger.LogInformation(configuration.JobRoleDescription);
                logger.LogInformation("--------");

                if (configuration.JobRoleDescription != serializedObject.PrivilegeGroup.Privilege)
                    throw new UnauthorizedAccessException("You are not authorized to the system");
            }
        }

        private async Task<Saml2Configuration> GetSaml2ConfigurationAsync(CancellationToken cancellationToken)
        {
            var key = nameof(Saml2Configuration);

            var result = await tenantMemoryCache.GetOrCreateAsync(key, async () =>
            {
                var authAppConfig = await saml2AuthContextProvider.GetSaml2AuthAppConfigurationAsync(cancellationToken);
                var cert = await ReadCertificateAsync(authAppConfig.SigningCertificateFile, authAppConfig.SigningCertificatePassword, cancellationToken);

                await authAppConfig.SigningCertificateFile.DisposeAsync();

                var saml2Config = new Saml2Configuration
                {
                    Issuer = authAppConfig.Issuer,
                    SignatureAlgorithm = "http://www.w3.org/2001/04/xmldsig-more#rsa-sha256",
                    CertificateValidationMode = System.ServiceModel.Security.X509CertificateValidationMode.None,
                    RevocationMode = X509RevocationMode.NoCheck,
                    SignAuthnRequest = true
                };

                saml2Config.SigningCertificate = saml2Config.DecryptionCertificate = cert;
                saml2Config.AllowedAudienceUris.Add(authAppConfig.Issuer);

                var entityDescriptor = new EntityDescriptor();

                entityDescriptor.ReadIdPSsoDescriptorFromFile(environment.MapToPhysicalFilePath(authConfigOptions.Value.IdPMetadataFile));

                if (entityDescriptor.IdPSsoDescriptor != null)
                {
                    saml2Config.AllowedIssuer = entityDescriptor.EntityId;
                    saml2Config.SingleSignOnDestination = entityDescriptor.IdPSsoDescriptor.SingleSignOnServices.First().Location;
                    saml2Config.SingleLogoutDestination = entityDescriptor.IdPSsoDescriptor.SingleLogoutServices.First().Location;
                    saml2Config.SignatureValidationCertificates.AddRange(entityDescriptor.IdPSsoDescriptor.SigningCertificates);

                    if (entityDescriptor.IdPSsoDescriptor.WantAuthnRequestsSigned.HasValue)
                    {
                        saml2Config.SignAuthnRequest = entityDescriptor.IdPSsoDescriptor.WantAuthnRequestsSigned.Value;
                    }

                    saml2Config.AuthnResponseSignType = Saml2AuthnResponseSignTypes.SignAssertionAndResponse;
                }
                else
                {
                    throw new Exception("IdPSsoDescriptor not loaded from metadata.");
                }

                return saml2Config;
            });

            if (result == null)
            {
                tenantMemoryCache.Remove(key);
                throw new Exception("Could not resolve saml auth configuration");
            }

            return result;
        }

        private static async Task<X509Certificate2> ReadCertificateAsync(Stream stream, string password, CancellationToken cancellationToken)
        {
            stream.Position = 0;

            using var bufferedStream = new BufferedStream(stream);
            using var memoryStream = new MemoryStream();
            await bufferedStream.CopyToAsync(memoryStream, cancellationToken);
            var bytes = memoryStream.ToArray();

            return new X509Certificate2(bytes, password);
        }
    }
}

