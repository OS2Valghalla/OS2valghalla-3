namespace Valghalla.Application.Communication
{
    public sealed class CommunicationTaskParticipantInfo
    {
        public Guid TaskId { get; init; }
        public Guid ParticipantId { get; init; }
        public bool IsRejectedTask { get; init; }
    }
}
