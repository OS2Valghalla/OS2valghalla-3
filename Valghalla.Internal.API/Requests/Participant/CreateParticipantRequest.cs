namespace Valghalla.Internal.API.Requests.Participant
{
    public class CreateParticipantRequest
    {
        public string Cpr { get; init; } = null!;
        public string? MobileNumber { get; init; }
        public string? Email { get; init; }
        public IEnumerable<Guid>? SpecialDietIds { get; init; } = Enumerable.Empty<Guid>();
        public IEnumerable<Guid> TeamIds { get; init; } = Enumerable.Empty<Guid>();
        public Guid? TaskId { get; init; }
        public Guid? ElectionId { get; init; }
    }
}