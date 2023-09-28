using Valghalla.Database.Interceptors.ChangeTracking;

namespace Valghalla.Database.Entities.Tables;

public partial class RejectedTaskAssignmentEntity : IChangeTrackingEntity, IElectionConnectedEntity
{
    public Guid Id { get; set; }
    public Guid ElectionId { get; set; }
    public Guid WorkLocationId { get; set; }
    public Guid TaskTypeId { get; set; }
    public Guid TeamId { get; set; }
    public Guid ParticipantId { get; set; }
    public DateTime TaskDate { get; set; }
    public DateTime CreatedAt { get; set; }
    public Guid CreatedBy { get; set; }
    public DateTime? ChangedAt { get; set; }
    public Guid? ChangedBy { get; set; }
    public virtual ParticipantEntity Participant { get; set; } = null!;
    public virtual WorkLocationEntity WorkLocation { get; set; } = null!;
    public virtual TaskTypeEntity TaskType { get; set; } = null!;
    public virtual TeamEntity Team { get; set; } = null!;
    public virtual ElectionEntity Election { get; set; } = null!;
    public virtual UserEntity CreatedByUser { get; set; } = null!;
    public virtual UserEntity? ChangedByUser { get; set; }
}
