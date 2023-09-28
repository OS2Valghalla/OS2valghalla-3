namespace Valghalla.Internal.Application.Modules.Participant.Responses
{
    public record ParticipantEventLogListingItemResponse
    {
        public Guid Id { get; init; }
        public Guid ParticipantId { get; init; }
        public string Text { get; init; } = string.Empty;
        public DateTime CreatedAt { get; init; }
    }
}
