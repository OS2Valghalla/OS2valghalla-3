namespace Valghalla.External.Application.Modules.Team.Request
{
    public sealed record CreateTeamLinkRequest
    {
        public Guid TeamId { get; init; }
    }
}
