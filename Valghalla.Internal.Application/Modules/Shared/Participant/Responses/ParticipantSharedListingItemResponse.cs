namespace Valghalla.Internal.Application.Modules.Shared.Participant.Responses
{
    public sealed record ParticipantSharedListingItemResponse
    {
        public Guid Id { get; init; }
        public string Name { get; init; } = null!;
        public DateTime Birthdate { get; init; }
    }
}
