namespace Valghalla.External.Application.Modules.Team.Responses
{
    public sealed record TeamMemberRemovalResponse
    {
        public bool UserRemoved { get; init; }
        public IEnumerable<Guid> TaskIds { get; init; } = Array.Empty<Guid>();
    }
}
