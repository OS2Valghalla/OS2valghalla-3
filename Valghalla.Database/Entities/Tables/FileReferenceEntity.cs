using Valghalla.Database.Interceptors.ChangeTracking;

namespace Valghalla.Database.Entities.Tables
{
    public partial class FileReferenceEntity : IChangeTrackingEntity
    {
        public Guid Id { get; set; }
        public Guid? FileId { get; set; }
        public string FileName { get; set; } = null!;

        public DateTime CreatedAt { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime? ChangedAt { get; set; }
        public Guid? ChangedBy { get; set; }

        public UserEntity CreatedByUser { get; set; } = null!;
        public UserEntity? ChangedByUser { get; set; }

        public virtual ICollection<TaskTypeEntity> TaskTypes { get; } = new List<TaskTypeEntity>();
        public virtual ICollection<TaskTypeFileEntity> TaskTypeFiles { get; } = new List<TaskTypeFileEntity>();
        public virtual ICollection<CommunicationTemplateEntity> CommunicationTemplates { get; } = new List<CommunicationTemplateEntity>();
        public virtual ICollection<CommunicationTemplateFileEntity> CommunicationTemplateFiles { get; } = new List<CommunicationTemplateFileEntity>();
        public virtual ICollection<ElectionCommitteeContactInformationEntity> ElectionCommitteeContactInformation { get; } = new List<ElectionCommitteeContactInformationEntity>();
    }
}
