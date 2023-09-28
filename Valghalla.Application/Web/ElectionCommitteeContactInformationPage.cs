using Valghalla.Application.Storage;

namespace Valghalla.Application.Web
{
    public sealed record ElectionCommitteeContactInformationPage
    {
        public string MunicipalityName { get; init; } = null!;
        public string ElectionResponsibleApartment { get; init; } = null!;
        public string Address { get; init; } = null!;
        public string PostalCode { get; init; } = null!;
        public string City { get; init; } = null!;
        public string? TelephoneNumber { get; init; }
        public string? Email { get; init; }
        public string? DigitalPost { get; init; }
        public Guid? LogoFileReferenceId { get; init; }
        public FileReferenceInfo? LogoFileReference { get; init; }
    }
}
