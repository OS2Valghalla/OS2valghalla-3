namespace Valghalla.External.Application.Modules.Tasks.Request
{
    public sealed record RemoveTeamMemberRequest
    {
        public Guid TeamId { get; set; }
        public Guid ParticipantId { get; set; }
    }
}
