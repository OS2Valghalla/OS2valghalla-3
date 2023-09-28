namespace Valghalla.External.API.Requests.MyProfile
{
    public sealed record UpdateMyProfileRequest
    {
        public string? MobileNumber { get; init; }
        public string? Email { get; init; }
        public IEnumerable<Guid> SpecialDietIds { get; init; } = Enumerable.Empty<Guid>();
    }
}
