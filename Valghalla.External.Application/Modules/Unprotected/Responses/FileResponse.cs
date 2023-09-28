namespace Valghalla.External.Application.Modules.Unprotected.Responses
{
    public sealed record FileResponse
    {
        public Stream Stream { get; init; }
        public string FileName { get; init; }
    }
}
