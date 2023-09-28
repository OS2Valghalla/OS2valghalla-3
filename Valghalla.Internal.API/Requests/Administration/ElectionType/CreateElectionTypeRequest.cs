namespace Valghalla.Internal.API.Requests.Administration.ElectionType
{
    public sealed record CreateElectionTypeRequest
    {
        public string Title { get; init; } = string.Empty;
        public List<Guid> ValidationRuleIds { get; init; } = new List<Guid>();
    }
}
