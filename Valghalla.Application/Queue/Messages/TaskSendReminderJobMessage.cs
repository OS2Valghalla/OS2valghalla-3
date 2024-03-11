namespace Valghalla.Application.Queue.Messages
{
    public sealed record TaskSendReminderJobMessage
    {
        public Guid ParticipantId { get; init; }
        public Guid TaskAssignmentId { get; init; }
    }
}
