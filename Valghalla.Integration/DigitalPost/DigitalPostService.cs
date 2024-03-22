using Digst.OioIdws.OioWsTrustCore;
using Digst.OioIdws.WscCore.OioWsTrust;
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens;
using System.Text;
using System.Text.Json;
using Valghalla.Application.Cache;
using Valghalla.Application.Configuration;
using Valghalla.Application.DigitalPost;
using Valghalla.Application.Secret;

namespace Valghalla.Integration.DigitalPost
{
    internal class DigitalPostService : IDigitalPostService
    {
        private readonly AppConfiguration appConfiguration;
        private readonly SecretConfiguration secretConfiguration;
        private readonly ISecretService secretService;
        private readonly HttpClient httpClient;
        private readonly DigitalPostMessageHelper helper;
        private readonly ITenantMemoryCache tenantMemoryCache;

        public DigitalPostService(
            AppConfiguration appConfiguration,
            IOptions<SecretConfiguration> secretConfigurationOptions,
            ISecretService secretService,
            HttpClient httpClient,
            DigitalPostMessageHelper helper,
            ITenantMemoryCache tenantMemoryCache)
        {
            this.appConfiguration = appConfiguration;
            this.secretConfiguration = secretConfigurationOptions.Value;
            this.secretService = secretService;
            this.httpClient = httpClient;
            this.helper = helper;
            this.tenantMemoryCache = tenantMemoryCache;
        }

        public async Task SendAsync(Guid messageUUID, DigitalPostMessage message, CancellationToken cancellationToken)
        {
            var config = await secretService.GetDigitalPostConfigurationAsync(cancellationToken);
            var securityToken = GetSecurityToken(config);
            var accessToken = await GetAccessTokenAsync(config, securityToken, cancellationToken);

            await ExecuteSendingAsync(messageUUID, config, accessToken, message, cancellationToken);
        }

        private async Task ExecuteSendingAsync(Guid messageUUID, DigitalPostConfiguration config, DigitalPostAccessToken accessToken, DigitalPostMessage message, CancellationToken cancellationToken)
        {
            var timestamp = DateTime.Now.ToString("s") + "Z";
            var data = helper.CreateSendingMessage(message, messageUUID, timestamp);

            var httpRequest = new HttpRequestMessage(HttpMethod.Post, config.ServiceUrl)
            {
                Content = new StringContent(data, Encoding.UTF8, "application/xml")
            };

            httpRequest.Headers.Add("Authorization", "Holder-of-key " + accessToken.Value);
            httpRequest.Headers.Add("x-TransaktionsId", messageUUID.ToString());
            httpRequest.Headers.Add("x-TransaktionsTid", timestamp);

            var httpResponse = await httpClient.SendAsync(httpRequest, cancellationToken);
            httpResponse.EnsureSuccessStatusCode();

            var response = await httpResponse.Content.ReadAsStringAsync(cancellationToken: cancellationToken);
            helper.ValidateResponseMessage(response);
        }

        private GenericXmlSecurityToken GetSecurityToken(DigitalPostConfiguration config)
        {
            return tenantMemoryCache.GetOrCreate("DigitalPostCVR", cacheEntry =>
            {
                var wscConfig = new OioIdwsWcfConfigurationSection()
                {
                    WspEndpoint = config.WspEndpoint,
                    WspEndpointID = config.WspEndpointID,
                    WspSoapVersion = "1.2",
                    Cvr = appConfiguration.DigitalPostCvr,
                    TokenLifeTimeInMinutes = 5,
                    IncludeLibertyHeader = true,
                    StsEndpointAddress = config.StsEndpointAddress,
                    StsEntityIdentifier = config.StsEntityIdentifier,
                    StsCertificate = new Certificate()
                    {
                        FromFileSystem = true,
                        FilePath = GetCertFilePath(config.StsCertificateFilePath),
                        Password = config.StsCertificatePassword,
                    },
                    ServiceCertificate = new Certificate()
                    {
                        FromFileSystem = true,
                        FilePath = GetCertFilePath(config.ServiceCertificateFilePath),
                        Password = config.ServiceCertificatePassword,
                    },
                    ClientCertificate = new Certificate()
                    {
                        FromFileSystem = true,
                        FilePath = GetCertFilePath(config.ClientCertificateFilePath),
                        Password = config.ClientCertificatePassword,
                    }
                };

                var stsTokenConfig = TokenServiceConfigurationFactory.CreateConfiguration(wscConfig);
                IStsTokenService stsTokenService = new StsTokenService(stsTokenConfig);
                var token = (GenericXmlSecurityToken)stsTokenService.GetToken();
                cacheEntry.AbsoluteExpiration = token.ValidTo - stsTokenConfig.CacheClockSkew;

                return token;
            })!;
        }

        private string GetCertFilePath(string path)
        {
            return Path.Combine(AppContext.BaseDirectory, Path.GetDirectoryName(secretConfiguration.Path)!, path);
        }

        private async Task<DigitalPostAccessToken> GetAccessTokenAsync(DigitalPostConfiguration config, GenericXmlSecurityToken securityToken, CancellationToken cancellationToken)
        {
            var data = "saml-token=" + EncodeSecurityToken(securityToken);
            var httpContent = new StringContent(data, Encoding.UTF8, "application/x-www-form-urlencoded");

            var httpResponse = await httpClient.PostAsync(config.AccessTokenServiceUrl, httpContent, cancellationToken);
            httpResponse.EnsureSuccessStatusCode();

            var response = await httpResponse.Content.ReadAsStringAsync(cancellationToken: cancellationToken);
            var value = JsonSerializer.Deserialize<DigitalPostAccessTokenResult>(response)
                ?? throw new Exception("Could not deserialize access token response");

            return new DigitalPostAccessToken
            {
                Value = value.AccessToken,
                ExpiresIn = TimeSpan.FromSeconds(value.ExpiresIn),
                RetrievedAtUtc = DateTime.UtcNow,
                Type = value.TokenType
            };
        }

        private static string EncodeSecurityToken(GenericXmlSecurityToken token)
        {
            var tokenXml = token.TokenXml.OuterXml;
            var bytes = Encoding.UTF8.GetBytes(tokenXml);
            var encodedToken = Convert.ToBase64String(bytes);
            return System.Web.HttpUtility.UrlEncode(encodedToken);
        }
    }
}
