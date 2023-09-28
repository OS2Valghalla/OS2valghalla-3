namespace Valghalla.Application.Communication
{
    public class CommunicationLogListingItem
    {
        public Guid Id { get; init; }
        public Guid ParticipantId { get; init; }
        public string ParticipantName { get; init; } = null!;
        public CommunicationLogMessageType MessageType { get; init; } = null!;
        public CommunicationLogSendType SendType { get; init; } = null!;
        public string ShortMessage { get; init; } = null!;
        public CommunicationLogStatus Status { get; init; } = null!;
        public DateTime CreatedAt { get; init; }
    }
}
