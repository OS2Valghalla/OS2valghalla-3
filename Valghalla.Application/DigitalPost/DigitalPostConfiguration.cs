namespace Valghalla.Application.DigitalPost
{
    public sealed record DigitalPostConfiguration
    {
        public string ServiceUrl { get; init; } = null!;
        public string AccessTokenServiceUrl { get; init; } = null!;

        public string WspEndpoint { get; init; } = null!;
        public string WspEndpointID { get; init; } = null!;

        public string StsEndpointAddress { get; init; } = null!;
        public string StsEntityIdentifier { get; init; } = null!;

        public string StsCertificateFilePath { get; init; } = null!;
        public string StsCertificatePassword { get; init; } = null!;
        public string ServiceCertificateFilePath { get; init; } = null!;
        public string ServiceCertificatePassword { get; init; } = null!;
        public string ClientCertificateFilePath { get; init; } = null!;
        public string ClientCertificatePassword { get; init; } = null!;
    }
}
