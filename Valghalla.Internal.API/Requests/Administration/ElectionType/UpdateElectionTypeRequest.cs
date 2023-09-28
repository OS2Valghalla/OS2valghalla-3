namespace Valghalla.Internal.API.Requests.Administration.ElectionType
{
    public sealed record UpdateElectionTypeRequest
    {
        public Guid Id { get; init; }
        public string Title { get; init; } = string.Empty;
        public List<Guid> ValidationRuleIds { get; init; } = new List<Guid>();
    }
}
