namespace Valghalla.Application.Queue.Messages
{
    public sealed record TaskInvitationRetractedJobMessage
    {
        public Guid ParticipantId { get; init; }
        public Guid TaskAssignmentId { get; init; }
    }
}
