namespace Valghalla.Internal.Application.Modules.App.Responses
{
    public sealed record AppElectionResponse
    {
        public Guid Id { get; init; }
        public string Title { get; init; } = null!;
        public DateTime ElectionDate { get; init; }
        public bool Active { get; init; }
    }
}
