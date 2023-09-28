namespace Valghalla.Internal.Application.Modules.Participant.Responses
{
    public record ParticipantListingItemResponse
    {
        public Guid Id { get; init; }
        public string Name { get; init; } = null!;
        public DateTime Birthdate { get; init; }
        public bool DigitalPost { get; init; }
        public bool HasUnansweredTask { get; init; }
        public bool AssignedTask { get; init; }
        public IEnumerable<Guid> TeamIds { get; init; } = Enumerable.Empty<Guid>();
        public IEnumerable<Guid> TaskTypeIds { get; init; } = Enumerable.Empty<Guid>();
    }
}
