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

        private readonly IOptions<GlobalAuthConfiguration> authConfigOptions;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IWebHostEnvironment environment;
        private readonly ITenantMemoryCache tenantMemoryCache;
        private readonly ITenantContextProvider tenantContextProvider;
        private readonly ISaml2AuthContextProvider saml2AuthContextProvider;
        private readonly InternalAuthConfiguration configuration;
        private readonly ILogger<Saml2AuthService> logger;

        private readonly ISaml2AuthPostProcessor postProcessor;

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
            ILogger<Saml2AuthService> logger,
            ISaml2AuthPostProcessor postProcessor)
        {
            this.authConfigOptions = authConfigOptions;
            this.httpContextAccessor = httpContextAccessor;
            this.environment = environment;
            this.tenantMemoryCache = tenantMemoryCache;
            this.tenantContextProvider = tenantContextProvider;
            this.saml2AuthContextProvider = saml2AuthContextProvider;
            this.configuration = configuration;
            this.logger = logger;
            this.postProcessor = postProcessor;
        }

        public async Task<string> GetLoginRedirectUrlAsync(CancellationToken cancellationToken)
        {
            try
            {
                logger.LogDebug("[SAML2] Starting GetLoginRedirectUrlAsync");
                
                var saml2Config = await GetSaml2ConfigurationAsync(cancellationToken);
                logger.LogDebug("[SAML2] SAML2 configuration loaded. Issuer: {Issuer}", saml2Config.Issuer);
                
                var binding = new Saml2RedirectBinding();

                binding.SetRelayStateQuery(new Dictionary<string, string>
                {
                    { RELAY_STATE_REDIRECT, string.Empty }
                });

                var authnRequest = new Saml2AuthnRequest(saml2Config);
                logger.LogDebug("[SAML2] Created AuthnRequest with ID: {RequestId}", authnRequest.IdAsString);
                
                binding.Bind(authnRequest);

                var redirectUrl = binding.RedirectLocation.OriginalString;
                logger.LogDebug("[SAML2] Generated login redirect URL (length: {UrlLength})", redirectUrl.Length);
                
                return redirectUrl;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "[SAML2] Error in GetLoginRedirectUrlAsync: {ErrorMessage}", ex.Message);
                throw;
            }
        }

        public async Task<string> LogoutAsync(ClaimsPrincipal principal, bool profileDeleted, CancellationToken cancellationToken)
        {
            try
            {
                logger.LogDebug("[SAML2] Starting LogoutAsync. ProfileDeleted: {ProfileDeleted}", profileDeleted);
                
                var saml2Config = await GetSaml2ConfigurationAsync(cancellationToken);
                var binding = new Saml2RedirectBinding();

                if (profileDeleted)
                {
                    logger.LogDebug("[SAML2] Profile deletion flag set, adding relay state");
                    binding.SetRelayStateQuery(new Dictionary<string, string>
                    {
                        { RELAY_STATE_PROFILE_DELETED, string.Empty }
                    });
                }

                var saml2LogoutRequest = new Saml2LogoutRequest(saml2Config, principal);
                logger.LogDebug("[SAML2] Created LogoutRequest with ID: {RequestId}", saml2LogoutRequest.IdAsString);
                
                binding.Bind(saml2LogoutRequest);

                var logoutUrl = binding.RedirectLocation.OriginalString;
                logger.LogDebug("[SAML2] Generated logout URL (length: {UrlLength})", logoutUrl.Length);
                
                return logoutUrl;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "[SAML2] Error in LogoutAsync: {ErrorMessage}", ex.Message);
                throw;
            }
        }

        public async Task<string> SetupAssertionConsumerServiceAsync(bool isInternal, CancellationToken cancellationToken)
        {
            try
            {
                logger.LogDebug("[SAML2] Starting SetupAssertionConsumerServiceAsync. IsInternal: {IsInternal}", isInternal);
                
                var saml2Config = await GetSaml2ConfigurationAsync(cancellationToken);
                var binding = new Saml2PostBinding();
                var saml2AuthnResponse = new Saml2AuthnResponse(saml2Config);

                binding.ReadSamlResponse(HttpContext.Request.ToGenericHttpRequest(), saml2AuthnResponse);
                logger.LogDebug("[SAML2] SAML Response read from request");

                if (saml2AuthnResponse.Status != Saml2StatusCodes.Success)
                {
                    logger.LogError("[SAML2] SAML Response status is not Success: {Status}. XML: {ResponseXml}", 
                        saml2AuthnResponse.Status, saml2AuthnResponse.ToXml().OuterXml);
                    throw new AuthenticationException($"SAML Response status: {saml2AuthnResponse.Status}");
                }

                binding.Unbind(HttpContext.Request.ToGenericHttpRequest(), saml2AuthnResponse);
                logger.LogDebug("[SAML2] SAML Response unbound successfully");

                var principal = await EnsureClaimsPrincipal(saml2AuthnResponse, isInternal, cancellationToken);
                logger.LogDebug("[SAML2] Claims principal created with {ClaimCount} claims", principal.Claims.Count());

                var relayStateQuery = binding.GetRelayStateQuery();

                if (relayStateQuery.ContainsKey(RELAY_STATE_REDIRECT))
                {
                    var returnValue = (environment.IsDevelopment() ? tenantContextProvider.CurrentTenant.AngularDevServer : RootUrl) + "?" + RELAY_STATE_REDIRECT + "=true";
                    logger.LogDebug("[SAML2] Returning redirect URL with relay state");
                    return returnValue;
                }

                logger.LogDebug("[SAML2] Returning root URL");
                return RootUrl;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "[SAML2] Error in SetupAssertionConsumerServiceAsync: {ErrorMessage}", ex.Message);
                throw;
            }
        }

        public async Task<string> SetupLogoutResponseAsync(string logoutPath, CancellationToken cancellationToken)
        {
            try
            {
                logger.LogDebug("[SAML2] Starting SetupLogoutResponseAsync with logoutPath: {LogoutPath}", logoutPath);
                
                var saml2Config = await GetSaml2ConfigurationAsync(cancellationToken);
                var genericHttpRequest = HttpContext.Request.ToGenericHttpRequest();

                if (new Saml2PostBinding().IsResponse(genericHttpRequest) || new Saml2RedirectBinding().IsResponse(genericHttpRequest))
                {
                    logger.LogDebug("[SAML2] Processing logged out response");
                    
                    var binding = new Saml2RedirectBinding();
                    binding.Unbind(genericHttpRequest, new Saml2LogoutResponse(saml2Config));

                    var rootUrl = environment.IsDevelopment() ? tenantContextProvider.CurrentTenant.AngularDevServer! : RootUrl;
                    var logoutLandingUrl = rootUrl.TrimEnd('/') + logoutPath;

                    var relayStateQuery = binding.GetRelayStateQuery();

                    if (relayStateQuery.ContainsKey(RELAY_STATE_PROFILE_DELETED))
                    {
                        logger.LogDebug("[SAML2] Profile deleted flag found in relay state");
                        logoutLandingUrl += "?profile-deleted=true";
                    }

                    logger.LogDebug("[SAML2] Returning logout landing URL");
                    return await Task.FromResult(logoutLandingUrl);
                }
                else
                {
                    logger.LogDebug("[SAML2] Processing single logout response");
                    
                    Saml2StatusCodes status;
                    var requestBinding = new Saml2RedirectBinding();
                    var logoutRequest = new Saml2LogoutRequest(saml2Config, HttpContext.User);
                    
                    try
                    {
                        requestBinding.Unbind(genericHttpRequest, logoutRequest);
                        status = Saml2StatusCodes.Success;
                        logger.LogDebug("[SAML2] Logout request unbound successfully. RequestID: {RequestId}", logoutRequest.IdAsString);
                    }
                    catch (Exception exc)
                    {
                        logger.LogWarning(exc, "[SAML2] Error unbinding logout request: {ErrorMessage}", exc.Message);
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
                    logger.LogDebug("[SAML2] Logout response created with status: {Status}", status);

                    var responseUrl = responsebinding.RedirectLocation.OriginalString;
                    logger.LogDebug("[SAML2] Generated logout response URL (length: {UrlLength})", responseUrl.Length);
                    
                    return responseUrl;
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "[SAML2] Error in SetupLogoutResponseAsync: {ErrorMessage}", ex.Message);
                throw;
            }
        }

        private async Task<ClaimsPrincipal> EnsureClaimsPrincipal(Saml2AuthnResponse saml2AuthnResponse, bool isInternal, CancellationToken cancellationToken)
        {
            try
            {
                logger.LogDebug("[SAML2] Starting EnsureClaimsPrincipal. Response Status: {Status}", saml2AuthnResponse.Status);
                
                if (saml2AuthnResponse.Status != 0)
                {
                    logger.LogError("[SAML2] SAML2 Response Status is not Success: {Status}", saml2AuthnResponse.Status);
                    throw new InvalidOperationException($"The SAML2 Response Status is not Success, the Response Status is: {saml2AuthnResponse.Status}.");
                }

                ClaimsPrincipal principal = new ClaimsPrincipal(saml2AuthnResponse.ClaimsIdentity);
                if (principal.Identity == null || !principal.Identity.IsAuthenticated)
                {
                    logger.LogError("[SAML2] No authenticated claims identity created from SAML2 Response");
                    throw new InvalidOperationException("No Claims Identity created from SAML2 Response.");
                }

                logger.LogInformation("[SAML2] User authenticated with identity: {IdentityName}", principal.Identity.Name);
                logger.LogDebug("[SAML2] Claims Details:");
                foreach (Claim c in principal.Claims)
                {
                    logger.LogDebug("[SAML2] Claim Type: {ClaimType}, Value: {ClaimValue}, ValueType: {ValueType}", 
                        c.Type, c.Value, c.ValueType);
                }

                if (isInternal)
                {
                    logger.LogDebug("[SAML2] Checking job role definition for internal user");
                    CheckJobRoleDefinition(principal);
                }

                logger.LogDebug("[SAML2] Checking assurance level");
                CheckAssurance(principal);

                var processedPrincipal = await postProcessor.HandleAsync(principal, cancellationToken);
                logger.LogDebug("[SAML2] Post-processor handled claims successfully");
                
                return processedPrincipal;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "[SAML2] Error in EnsureClaimsPrincipal: {ErrorMessage}", ex.Message);
                throw;
            }
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
            {
                logger.LogError("[SAML2] Job role description not configured");
                throw new UnauthorizedAccessException("Missing authentication configuration");
            }

            var privilegeClaimV2 = claimsPrincipal.Claims.FirstOrDefault(x => x.Type == OioSaml2ClaimTypes.PrivilegesIntermediate && !string.IsNullOrEmpty(x.Value));
            var privilegeClaimV3 = claimsPrincipal.Claims.FirstOrDefault(x => x.Type == OioSaml3ClaimTypes.PrivilegesIntermediate && !string.IsNullOrEmpty(x.Value));
            var privilegeClaim = privilegeClaimV2 ??= privilegeClaimV3;

            if (privilegeClaim is null)
            {
                logger.LogError("[SAML2] No privilege claim found");
                throw new UnauthorizedAccessException("You are not authorized to the system (no job function role info supplied with claim)");
            }

            byte[] data = Convert.FromBase64String(privilegeClaim.Value);
            string decodedString = System.Text.Encoding.UTF8.GetString(data);

            logger.LogDebug("[SAML2] Decoded privilege claim");

            if (decodedString.Contains("http://digst.dk"))
            {
                logger.LogDebug("[SAML2] Processing V3 privilege list");
                var serializer = new XmlSerializer(typeof(AuthObjectsV3.PrivilegeList));
                var serializedObject = (AuthObjectsV3.PrivilegeList)serializer.Deserialize(new StringReader(decodedString));

                logger.LogDebug("[SAML2] V3 Claim JobFunctionRole: {PrivilegeGroup}", serializedObject.PrivilegeGroup.Privilege);
                logger.LogDebug("[SAML2] Configuration JobFunctionRole: {ConfiguredRole}", configuration.JobRoleDescription);

                if (configuration.JobRoleDescription != serializedObject.PrivilegeGroup.Privilege)
                {
                    logger.LogError("[SAML2] Job role mismatch. Expected: {Expected}, Got: {Got}", 
                        configuration.JobRoleDescription, serializedObject.PrivilegeGroup.Privilege);
                    throw new UnauthorizedAccessException("You are not authorized to the system");
                }
            }
            else
            {
                logger.LogDebug("[SAML2] Processing V2 privilege list");
                var serializer = new XmlSerializer(typeof(AuthObjects.PrivilegeList));
                var serializedObject = (AuthObjects.PrivilegeList)serializer.Deserialize(new StringReader(decodedString));

                logger.LogDebug("[SAML2] V2 Claim JobFunctionRole: {PrivilegeGroup}", serializedObject.PrivilegeGroup.Privilege);
                logger.LogDebug("[SAML2] Configuration JobFunctionRole: {ConfiguredRole}", configuration.JobRoleDescription);

                if (configuration.JobRoleDescription != serializedObject.PrivilegeGroup.Privilege)
                {
                    logger.LogError("[SAML2] Job role mismatch. Expected: {Expected}, Got: {Got}", 
                        configuration.JobRoleDescription, serializedObject.PrivilegeGroup.Privilege);
                    throw new UnauthorizedAccessException("You are not authorized to the system");
                }
            }

            logger.LogInformation("[SAML2] Job role validation passed");
        }

        private async Task<Saml2Configuration> GetSaml2ConfigurationAsync(CancellationToken cancellationToken)
        {
            try
            {
                logger.LogDebug("[SAML2] Getting or creating SAML2 configuration from cache");
                var key = nameof(Saml2Configuration);

                var result = await tenantMemoryCache.GetOrCreateAsync(key, async () =>
                {
                    logger.LogDebug("[SAML2] Cache miss, creating new SAML2 configuration");
                    
                    var authAppConfig = await saml2AuthContextProvider.GetSaml2AuthAppConfigurationAsync(cancellationToken);
                    logger.LogDebug("[SAML2] Auth app config retrieved. Issuer: {Issuer}", authAppConfig.Issuer);
                    
                    var cert = await ReadCertificateAsync(authAppConfig.SigningCertificateFile, authAppConfig.SigningCertificatePassword, cancellationToken);
                    logger.LogDebug("[SAML2] Certificate loaded. Subject: {Subject}, Thumbprint: {Thumbprint}", 
                        cert.Subject, cert.Thumbprint);

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
                    var metadataPath = environment.MapToPhysicalFilePath(authConfigOptions.Value.IdPMetadataFile);
                    logger.LogDebug("[SAML2] Reading IdP metadata from: {MetadataPath}", metadataPath);

                    entityDescriptor.ReadIdPSsoDescriptorFromFile(metadataPath);

                    if (entityDescriptor.IdPSsoDescriptor != null)
                    {
                        saml2Config.AllowedIssuer = entityDescriptor.EntityId;
                        saml2Config.SingleSignOnDestination = entityDescriptor.IdPSsoDescriptor.SingleSignOnServices.First().Location;
                        saml2Config.SingleLogoutDestination = entityDescriptor.IdPSsoDescriptor.SingleLogoutServices.First().Location;
                        saml2Config.SignatureValidationCertificates.AddRange(entityDescriptor.IdPSsoDescriptor.SigningCertificates);

                        logger.LogDebug("[SAML2] IdP SSO Descriptor loaded. EntityID: {EntityID}, SSO Destination: {SSODest}", 
                            entityDescriptor.EntityId, saml2Config.SingleSignOnDestination);

                        if (entityDescriptor.IdPSsoDescriptor.WantAuthnRequestsSigned.HasValue)
                        {
                            saml2Config.SignAuthnRequest = entityDescriptor.IdPSsoDescriptor.WantAuthnRequestsSigned.Value;
                            logger.LogDebug("[SAML2] SignAuthnRequest set to: {SignAuthnRequest}", saml2Config.SignAuthnRequest);
                        }

                        saml2Config.AuthnResponseSignType = Saml2AuthnResponseSignTypes.SignAssertionAndResponse;
                        logger.LogInformation("[SAML2] SAML2 configuration created successfully");
                    }
                    else
                    {
                        logger.LogError("[SAML2] IdPSsoDescriptor not loaded from metadata file");
                        throw new Exception("IdPSsoDescriptor not loaded from metadata.");
                    }

                    return saml2Config;
                });

                if (result == null)
                {
                    logger.LogWarning("[SAML2] SAML2 configuration cache returned null, removing from cache");
                    tenantMemoryCache.Remove(key);
                    throw new Exception("Could not resolve saml auth configuration");
                }

                logger.LogDebug("[SAML2] SAML2 configuration retrieved successfully");
                return result;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "[SAML2] Error in GetSaml2ConfigurationAsync: {ErrorMessage}", ex.Message);
                throw;
            }
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

