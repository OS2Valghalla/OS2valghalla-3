using Valghalla.Database.Interceptors.ChangeTracking;

namespace Valghalla.Database.Entities.Tables;

public partial class TaskTypeTemplateEntity : IChangeTrackingEntity
{
    public Guid Id { get; set; }
    public string Title { get; set; } = null!;
    public string ShortName { get; set; } = null!;
    public string Description { get; set; } = null!;
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }
    public int? Payment { get; set; }
    public bool ValidationNotRequired { get; set; }
    public bool Trusted { get; set; }
    public bool SendingReminderEnabled { get; set; }
    public DateTime CreatedAt { get; set; }
    public Guid CreatedBy { get; set; }
    public DateTime? ChangedAt { get; set; }
    public Guid? ChangedBy { get; set; }
    public virtual UserEntity CreatedByUser { get; set; } = null!;
    public virtual UserEntity? ChangedByUser { get; set; }
    public virtual ICollection<TaskAssignmentEntity> TaskAssignments { get; } = new List<TaskAssignmentEntity>();
    public virtual ICollection<FileReferenceEntity> FileReferences { get; } = new List<FileReferenceEntity>();
    public virtual ICollection<TaskTypeTemplateFileEntity> TaskTypeFileTemplates { get; } = new List<TaskTypeTemplateFileEntity>();
    public virtual ICollection<WorkLocationTaskTypeEntity> WorkLocationTaskTypes { get; } = new List<WorkLocationTaskTypeEntity>();
    public virtual ICollection<ElectionTaskTypeCommunicationTemplateEntity> ElectionTaskTypeCommunicationTemplates { get; } = new List<ElectionTaskTypeCommunicationTemplateEntity>();
    public virtual ICollection<TaskTypeEntity> TaskTypes { get; } = new List<TaskTypeEntity>();
}
