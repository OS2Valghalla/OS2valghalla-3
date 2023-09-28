using Valghalla.Database.Interceptors.ChangeTracking;

namespace Valghalla.Database.Entities.Tables;

public partial class ElectionTypeEntity : IChangeTrackingEntity
{
    public Guid Id { get; set; }
    public string Title { get; set; } = null!;
    
    public DateTime CreatedAt { get; set; }
    public Guid CreatedBy { get; set; }
    public DateTime? ChangedAt { get; set; }
    public Guid? ChangedBy { get; set; }
    public virtual UserEntity CreatedByUser { get; set; } = null!;
    public virtual UserEntity? ChangedByUser { get; set; }
    public virtual ICollection<ElectionEntity> Elections { get; } = new List<ElectionEntity>();
    public virtual ICollection<ElectionTypeValidationRuleEntity> ValidationRules { get; }  = new List<ElectionTypeValidationRuleEntity>();
}
