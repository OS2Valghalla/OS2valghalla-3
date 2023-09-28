namespace Valghalla.Internal.API.Requests.Participant
{
    public sealed record UpdateParticipantRequest
    {
        public Guid Id { get; init; }
        public string? MobileNumber { get; init; }
        public string? Email { get; init; }
        public IEnumerable<Guid>? SpecialDietIds { get; init; } = Enumerable.Empty<Guid>();
        public IEnumerable<Guid> TeamIds { get; init; } = Enumerable.Empty<Guid>();
    }
}
