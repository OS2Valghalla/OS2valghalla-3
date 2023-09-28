namespace Valghalla.Worker.Infrastructure.Modules.Election.Responses
{
    public sealed record ElectionResponse
    {
        public Guid Id { get; init; }
        public string Title { get; init; } = null!;
        public bool Active { get; init; }
        public DateTime ElectionEndDate { get; init; }
    }
}
