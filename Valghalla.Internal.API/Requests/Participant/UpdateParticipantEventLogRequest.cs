namespace Valghalla.Internal.API.Requests.Participant
{
    public record UpdateParticipantEventLogRequest
    {
        public Guid Id { get; init; }
        public string Text { get; init; } = string.Empty;
    }
}
