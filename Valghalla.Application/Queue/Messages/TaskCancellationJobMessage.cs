namespace Valghalla.Application.Queue.Messages
{
    public sealed record TaskCancellationJobMessage
    {
        public Guid ParticipantId { get; init; }
        public Guid TaskAssignmentId { get; init; }
    }
}
