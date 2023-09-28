namespace Valghalla.Internal.API.Requests.Administration.Web
{
    public class UpdateElectionCommitteeContactInformationRequest
    {
        public string MunicipalityName { get; set; } = null!;
        public string ElectionResponsibleApartment { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string PostalCode { get; set; } = null!;
        public string City { get; set; } = null!;
        public string? TelephoneNumber { get; set; }
        public string? Email { get; set; }
        public string? DigitalPost { get; set; }
        public Guid? LogoFileReferenceId { get; set; }
    }
}
