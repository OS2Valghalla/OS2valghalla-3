using Valghalla.Application.Configuration;
using Valghalla.Application.Saml;
using Valghalla.Application.Storage;

namespace Valghalla.External.API.Auth
{
    internal class Saml2AuthContextProvider : ISaml2AuthContextProvider
    {
        private readonly ExternalAuthConfiguration externalAuthConfiguration;
        private readonly IFileStorageService fileStorageService;

        public Saml2AuthContextProvider(ExternalAuthConfiguration externalAuthConfiguration, IFileStorageService fileStorageService)
        {
            this.externalAuthConfiguration = externalAuthConfiguration;
            this.fileStorageService = fileStorageService;
        }

        public async Task<Saml2AuthAppConfiguration> GetSaml2AuthAppConfigurationAsync(CancellationToken cancellationToken)
        {
            var (stream, fileName) = await fileStorageService.DownloadAsync(Valghalla.Application.Constants.FileStorage.ExternalAuthCertificate, cancellationToken);

            return new Saml2AuthAppConfiguration()
            {
                Authority = externalAuthConfiguration.Authority,
                Issuer = externalAuthConfiguration.Issuer,
                SigningCertificateFile = stream,
                SigningCertificatePassword = externalAuthConfiguration.SigningCertificatePassword
            };
        }
    }
}
