namespace Valghalla.Application.Queue.Messages
{
    public sealed record RemovedFromTaskByValidationJobMessage
    {
        public Guid ParticipantId { get; init; }
        public Guid TaskAssignmentId { get; init; }
    }
}
