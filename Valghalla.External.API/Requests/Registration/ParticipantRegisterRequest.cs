namespace Valghalla.External.API.Requests.Registration
{
    public class ParticipantRegisterRequest
    {
        public string HashValue { get; init; } = null!;
        public string? MobileNumber { get; init; }
        public string? Email { get; init; }
        public IEnumerable<Guid> SpecialDietIds { get; init; } = Enumerable.Empty<Guid>();
    }
}
