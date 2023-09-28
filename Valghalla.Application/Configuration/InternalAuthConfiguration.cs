using Valghalla.Application.Configuration.Interfaces;

namespace Valghalla.Application.Configuration
{
    public class InternalAuthConfiguration : IConfiguration
    {
        public string Authority { get; init; } = null!;
        public string Issuer { get; init; } = null!;
        public string SigningCertificatePassword { get; init; } = null!;
    }
}
