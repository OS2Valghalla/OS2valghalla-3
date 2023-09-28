namespace Valghalla.Application.CPR
{
    public record ParticipantPersonalRecord
    {
        public string? FirstName { get; init; }
        public string? LastName { get; init; }
        public string? StreetAddress { get; init; }
        public string? PostalCode { get; init; }
        public string? City { get; init; }
        public string? MunicipalityCode { get; set; }
        public string? MunicipalityName { get; set; }
        public string? CountryCode { get; set; }
        public string? CountryName { get; set; }
        public string? CoAddress { get; init; }
        public int Age { get; init; }
        public DateTime Birthdate { get; init; }
        public bool Deceased { get; init; }
        public bool Disenfranchised { get; init; }
        public bool ExemptDigitalPost { get; init; }
        public DateTime? LastValidationDate { get; init; }
    }
}
