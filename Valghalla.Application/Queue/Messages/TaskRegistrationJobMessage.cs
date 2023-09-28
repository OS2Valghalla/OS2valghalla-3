namespace Valghalla.Application.Queue.Messages
{
    public sealed record TaskRegistrationJobMessage
    {
        public Guid ParticipantId { get; init; }
        public Guid TaskAssignmentId { get; init; }
    }
}
