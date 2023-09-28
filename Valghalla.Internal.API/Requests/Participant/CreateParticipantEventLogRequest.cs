namespace Valghalla.Internal.API.Requests.Participant
{
    public record CreateParticipantEventLogRequest
    {
        public Guid ParticipantId { get; init; }
        public string Text { get; init; } = string.Empty;
    }
}
