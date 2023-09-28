namespace Valghalla.Internal.Application.Modules.Administration.Election.Responses
{
    public sealed record ElectionDetailsResponse
    {
        public Guid Id { get; init; }
        public Guid ElectionTypeId { get; init; }
        public string Title { get; init; } = null!;
        public int LockPeriod { get; init; }
        public DateTime ElectionStartDate { get; init; }
        public DateTime ElectionEndDate { get; init; }
        public DateTime ElectionDate { get; init; }
        public bool Active { get; init; }
        public IEnumerable<Guid> WorkLocationIds { get; init; } = Array.Empty<Guid>();
    }
}
