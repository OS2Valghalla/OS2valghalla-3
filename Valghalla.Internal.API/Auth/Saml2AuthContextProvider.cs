using Valghalla.Application.Configuration;
using Valghalla.Application.Saml;
using Valghalla.Application.Storage;

namespace Valghalla.Internal.API.Auth
{
    internal class Saml2AuthContextProvider : ISaml2AuthContextProvider
    {
        private readonly InternalAuthConfiguration internalAuthConfiguration;
        private readonly InternalAuthFallbackConfiguration internalAuthFallbackConfiguration;
        private readonly IFileStorageService fileStorageService;

        public Saml2AuthContextProvider(InternalAuthConfiguration internalAuthConfiguration, IFileStorageService fileStorageService, InternalAuthFallbackConfiguration internalAuthFallbackConfiguration)
        {
            this.internalAuthConfiguration = internalAuthConfiguration;
            this.fileStorageService = fileStorageService;
            this.internalAuthFallbackConfiguration = internalAuthFallbackConfiguration;
        }

        public async Task<Saml2AuthAppConfiguration> GetSaml2AuthAppConfigurationAsync(CancellationToken cancellationToken)
        {
            var (stream, fileName) = await fileStorageService.DownloadAsync(Valghalla.Application.Constants.FileStorage.InternalAuthCertificate, cancellationToken);

            return new Saml2AuthAppConfiguration()
            {
                Authority = internalAuthConfiguration.Authority,
                Issuer = internalAuthConfiguration.Issuer,
                SigningCertificateFile = stream,
                SigningCertificatePassword = internalAuthConfiguration.SigningCertificatePassword
            };
        }
        public async Task<Saml2AuthAppConfiguration> GetSaml2AuthAppFallbackConfigurationAsync(CancellationToken cancellationToken)
        {
            var (stream, fileName) = await fileStorageService.DownloadAsync(Valghalla.Application.Constants.FileStorage.ExternalAuthCertificate, cancellationToken);

            return new Saml2AuthAppConfiguration()
            {
                Authority = internalAuthFallbackConfiguration.Authority,
                Issuer = internalAuthFallbackConfiguration.Issuer,
                SigningCertificateFile = stream,
                SigningCertificatePassword = internalAuthFallbackConfiguration.SigningCertificatePassword
            };
        }
    }
}
