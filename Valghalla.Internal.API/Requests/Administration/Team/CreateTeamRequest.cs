namespace Valghalla.Internal.API.Requests.Administration.Team
{
    public sealed record CreateTeamRequest
    {
        public string Name { get; init; } = string.Empty;
        public string ShortName { get; init; } = string.Empty;
        public Guid ElectionId { get; init; }
        public string? Description { get; init; }
        public List<Guid> ResponsibleIds { get; init; } = new List<Guid>();
    }
}
