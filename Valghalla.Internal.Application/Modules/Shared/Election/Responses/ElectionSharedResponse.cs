namespace Valghalla.Internal.Application.Modules.Shared.Election.Responses
{
    public sealed record ElectionSharedResponse
    {
        public Guid Id { get; init; }
        public string Title { get; init; } = null!;
        public DateTime ElectionDate { get; init; }
        public bool Active { get; init; }
    }
}
