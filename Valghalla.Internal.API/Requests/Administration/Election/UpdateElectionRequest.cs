namespace Valghalla.Internal.API.Requests.Administration.Election
{
    public sealed record UpdateElectionRequest
    {
        public Guid Id { get; init; }
        public string Title { get; init; } = string.Empty;
        public int LockPeriod { get; init; }
    }
}
