using Valghalla.Database.Interceptors.ChangeTracking;

namespace Valghalla.Database.Entities.Tables
{
    public partial class ElectionCommitteeContactInformationEntity : IChangeTrackingEntity
    {
        public string PageName { get; set; } = null!;
        public string MunicipalityName { get; set; } = null!;
        public string ElectionResponsibleApartment { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string PostalCode { get; set; } = null!;
        public string City { get; set; } = null!;
        public string? TelephoneNumber { get; set; }
        public string? Email { get; set; }
        public string? DigitalPost { get; set; }
        public Guid? LogoFileReferenceId { get; set; }
        public DateTime CreatedAt { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime? ChangedAt { get; set; }
        public Guid? ChangedBy { get; set; }
        public virtual UserEntity CreatedByUser { get; set; } = null!;
        public virtual UserEntity? ChangedByUser { get; set; }
        public virtual FileReferenceEntity? LogoFileReference { get; set; }
    }
}
