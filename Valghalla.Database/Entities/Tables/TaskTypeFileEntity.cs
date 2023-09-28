using Valghalla.Database.Interceptors.ChangeTracking;

namespace Valghalla.Database.Entities.Tables
{
    public partial class TaskTypeFileEntity : IChangeTrackingEntity
    {
        public Guid TaskTypeId { get; set; }
        public Guid FileReferenceId { get; set; }

        public DateTime CreatedAt { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime? ChangedAt { get; set; }
        public Guid? ChangedBy { get; set; }

        public virtual UserEntity CreatedByUser { get; set; } = null!;
        public virtual UserEntity? ChangedByUser { get; set; }
        public virtual TaskTypeEntity TaskType { get; set; } = null!;
        public virtual FileReferenceEntity FileReference { get; set; } = null!;
    }
}
