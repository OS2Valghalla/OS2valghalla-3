using Valghalla.Database.Interceptors.ChangeTracking;

namespace Valghalla.Database.Entities.Tables;

public partial class ParticipantEventLogEntity : IChangeTrackingEntity
{
    public Guid Id { get; set; }
    public Guid ParticipantId { get; set; }
    public string Text { get; set; } = null!;

    public DateTime CreatedAt { get; set; }
    public Guid CreatedBy { get; set; }
    public DateTime? ChangedAt { get; set; }
    public Guid? ChangedBy { get; set; }

    public virtual ParticipantEntity Participant { get; set; } = null!;
    public virtual UserEntity CreatedByUser { get; set; } = null!;
    public virtual UserEntity? ChangedByUser { get; set; }
}
