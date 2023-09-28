namespace Valghalla.Worker.Infrastructure.Modules.Participant.Responses
{
    public sealed record ParticipantResponse
    {
        public Guid Id { get; init; }
        public string Cpr { get; set; } = null!;
    }
}
