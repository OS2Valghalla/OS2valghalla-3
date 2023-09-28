namespace Valghalla.Internal.Application.Modules.Shared.FileStorage.Responses
{
    public sealed record FileResponse
    {
        public Stream Stream { get; init; }
        public string FileName { get; init; }
    }
}
