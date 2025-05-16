using Valghalla.Database.Interceptors.ChangeTracking;

namespace Valghalla.Database.Entities.Tables;

public partial class ParticipantEntity : IChangeTrackingEntity
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string Cpr { get; set; } = null!;
    public string? MobileNumber { get; set; }
    public string? Email { get; set; }

    // CPR provided info
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? StreetAddress { get; set; }
    public string? PostalCode { get; set; }
    public string? City { get; set; }
    public string? MunicipalityCode { get; set; }
    public string? MunicipalityName { get; set; }
    public string? CountryCode { get; set; }
    public string? CountryName { get; set; }
    public string? CoAddress { get; set; }
    public int Age { get; set; }
    public DateTime Birthdate { get; set; }
    public bool Deceased { get; set; }
    public bool Disenfranchised { get; set; }
    public bool ExemptDigitalPost { get; set; }
    public bool ProtectedAddress { get; set; }
    public DateTime? LastValidationDate { get; set; }

    public DateTime CreatedAt { get; set; }
    public Guid CreatedBy { get; set; }
    public DateTime? ChangedAt { get; set; }
    public Guid? ChangedBy { get; set; }

    public virtual ICollection<WorkLocationResponsibleEntity> WorkLocationResponsibles { get; } = new List<WorkLocationResponsibleEntity>();
    public virtual ICollection<ParticipantEventLogEntity> ParticipantEventLogs { get; } = new List<ParticipantEventLogEntity>();
    public virtual ICollection<TaskAssignmentEntity> TaskAssignments { get; } = new List<TaskAssignmentEntity>();
    public virtual ICollection<TeamEntity> TeamForMembers { get; } = new List<TeamEntity>();
    public virtual ICollection<TeamMemberEntity> TeamMembers { get; } = new List<TeamMemberEntity>();
    public virtual ICollection<TeamEntity> TeamForResponsibles { get; } = new List<TeamEntity>();
    public virtual ICollection<TeamResponsibleEntity> TeamResponsibles { get; } = new List<TeamResponsibleEntity>();
    public virtual ICollection<SpecialDietEntity> SpecialDiets { get; } = new List<SpecialDietEntity>();
    public virtual ICollection<SpecialDietParticipantEntity> SpecialDietParticipants { get; } = new List<SpecialDietParticipantEntity>();
    public virtual ICollection<CommunicationLogEntity> CommunicationLogs { get; } = new List<CommunicationLogEntity>();

    public virtual UserEntity User { get; set; } = null!;
    public virtual UserEntity CreatedByUser { get; set; } = null!;
    public virtual UserEntity? ChangedByUser { get; set; }
}
