namespace Valghalla.External.Application.Modules.MyProfile.Responses
{
    public sealed record MyProfileResponse
    {
        public Guid ParticipantId { get; init; }
        public string Cpr { get; init; } = null!;
        public string? FirstName { get; init; }
        public string? LastName { get; init; }
        public string? StreetAddress { get; init; }
        public string? PostalCode { get; init; }
        public string? City { get; init; }
        public string? MunicipalityName { get; init; }
        public string? MobileNumber { get; init; }
        public string? Email { get; init; }
        public IEnumerable<Guid> SpecialDietIds { get; init; } = Enumerable.Empty<Guid>();
    }
}
