namespace Valghalla.Application.Saml
{
    public sealed record Saml2AuthAppConfiguration
    {
        public string Authority { get; init; } = null!;
        public string Issuer { get; init; } = null!;
        public Stream SigningCertificateFile { get; init; } = null!;
        public string SigningCertificatePassword { get; init; } = null!;
    }
}
