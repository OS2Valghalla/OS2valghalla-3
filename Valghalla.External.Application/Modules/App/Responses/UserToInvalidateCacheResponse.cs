namespace Valghalla.External.Application.Modules.App.Responses
{
    public sealed record UserToInvalidateCacheResponse
    {
        public Guid Id { get; init; }
        public string Cpr { get; init; } = null!;
        public string Serial { get; init; } = null!;
    }
}
