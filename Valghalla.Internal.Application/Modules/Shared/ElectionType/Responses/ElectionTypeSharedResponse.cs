namespace Valghalla.Internal.Application.Modules.Shared.ElectionType.Responses
{
    public sealed record ElectionTypeSharedResponse
    {
        public Guid Id { get; init; }
        public string Title { get; init; } = null!;
    }
}
