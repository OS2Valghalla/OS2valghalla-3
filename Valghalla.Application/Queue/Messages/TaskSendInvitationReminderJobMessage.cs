namespace Valghalla.Application.Queue.Messages
{
    public sealed record TaskSendInvitationReminderJobMessage
    {
        public Guid ParticipantId { get; init; }
        public Guid TaskAssignmentId { get; init; }
    }
}
