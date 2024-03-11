namespace Valghalla.Application.Queue.Messages
{
    public sealed record RemovedFromTaskJobMessage
    {
        public Guid ParticipantId { get; init; }
        public Guid TaskAssignmentId { get; init; }
    }
}
