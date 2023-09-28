namespace Valghalla.Application.CPR
{
    public sealed record CprPersonInfo
    {
        public string Cpr { get; init; } = null!;
        public string FirstName { get; init; } = null!;
        public string LastName { get; init; } = null!;
        public CprPersonAddressInfo Address { get; init; } = null!;
        public CprMunicipalityInfo Municipality { get; init; } = null!;
        public CprCountryInfo Country { get; init; } = null!;
        public int Age { get; init; }
        public DateTime Birthdate { get; init; }
        public string CountryCode { get; init; } = null!;
        public string CountryName { get; init; } = null!;
        public bool Deceased { get; init; }
        public bool Disenfranchised { get; init; }
        public bool ExemptDigitalPost { get; init; }

        public ParticipantPersonalRecord ToRecord()
        {
            return new()
            {
                FirstName = this.FirstName,
                LastName = this.LastName,
                StreetAddress = this.Address.Street,
                PostalCode = this.Address.PostalCode,
                City = this.Address.City,
                MunicipalityCode = this.Municipality.Code,
                MunicipalityName = this.Municipality.Name,
                CountryCode = this.Country.Code,
                CountryName = this.Country.Name,
                Age = this.Age,
                Birthdate = this.Birthdate,
                Deceased = this.Deceased,
                Disenfranchised = this.Disenfranchised,
                ExemptDigitalPost = this.ExemptDigitalPost
            };
        }
    }

    public sealed record CprPersonAddressInfo
    {
        public string Street { get; init; } = string.Empty;
        public string? City { get; init; } = string.Empty;
        public string PostalCode { get; init; } = string.Empty;
    }

    public sealed record CprMunicipalityInfo
    {
        public string? Code { get; init; }
        public string? Name { get; init; }
    }

    public sealed record CprCountryInfo
    {
        public string Code { get; init; } = null!;
        public string Name { get; init; } = null!;
    }
}
