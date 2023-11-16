using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Valghalla.Application.Secret;

namespace Valghalla.Integration.DigitalPost
{
    internal class DigitalPostHttpClientHandler
    {
        public static HttpClientHandler Initialize(IServiceProvider serviceProvider)
        {
            var secretConfigurationOptions = serviceProvider.GetRequiredService<IOptions<SecretConfiguration>>();
            var secretService = serviceProvider.GetRequiredService<ISecretService>();

            var secretConfiguration = secretConfigurationOptions.Value;
            var config = secretService.GetDigitalPostConfigurationAsync(default).Result;
            var certPath = Path.Combine(AppContext.BaseDirectory, Path.GetDirectoryName(secretConfiguration.Path)!, config.ClientCertificateFilePath);
            var certPassword = config.ClientCertificatePassword;

            var handler = new HttpClientHandler();
            handler.ClientCertificates.Add(new System.Security.Cryptography.X509Certificates.X509Certificate2(certPath, certPassword));

            return handler;
        }
    }
}
