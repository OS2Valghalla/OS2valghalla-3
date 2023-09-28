namespace Valghalla.Application.Queue.Messages
{
    public sealed record TaskInvitationJobMessage
    {
        public Guid ParticipantId { get; init; }
        public Guid TaskAssignmentId { get; init; }
    }
}
