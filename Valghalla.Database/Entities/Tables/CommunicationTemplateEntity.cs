using Valghalla.Database.Interceptors.ChangeTracking;

namespace Valghalla.Database.Entities.Tables
{
    public partial class CommunicationTemplateEntity : IChangeTrackingEntity
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = null!;
        public string? Subject { get; set; }
        public string? Content { get; set; }
        public int TemplateType { get; set; }
        public bool? IsDefaultTemplate { get; set; }
        public DateTime CreatedAt { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime? ChangedAt { get; set; }
        public Guid? ChangedBy { get; set; }
        public virtual ICollection<FileReferenceEntity> CommunicationTemplateFileReferences { get; } = new List<FileReferenceEntity>();
        public virtual ICollection<CommunicationTemplateFileEntity> CommunicationTemplateFiles { get; } = new List<CommunicationTemplateFileEntity>();
        public virtual UserEntity CreatedByUser { get; set; } = null!;
        public virtual UserEntity? ChangedByUser { get; set; }
    }
}
