namespace Valghalla.Application.Authentication
{
    public sealed record GlobalAuthConfiguration
    {
        public string IdPMetadataFile { get; init; } = null!;
    }
}
