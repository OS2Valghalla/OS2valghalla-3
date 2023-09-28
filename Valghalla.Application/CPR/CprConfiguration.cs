namespace Valghalla.Application.CPR
{
    public sealed record CprConfiguration
    {
        public string ServiceUrl { get; init; } = string.Empty;
        public string SigningCertificateFile { get; init; } = string.Empty;
        public string SigningCertificatePassword { get; init; } = string.Empty;
        public string Thumbprint { get; init; } = string.Empty;
        public string CertPath { get; init; } = string.Empty;

    }
}
