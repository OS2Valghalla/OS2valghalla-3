using Valghalla.Database.Interceptors.ChangeTracking;

namespace Valghalla.Database.Entities.Tables
{
    public partial class FileEntity : IChangeTrackingEntity
    {
        public Guid Id { get; set; }
        public byte[] Content { get; set; } = null!;
        public string ContentHash { get; set; } = null!;

        public DateTime CreatedAt { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime? ChangedAt { get; set; }
        public Guid? ChangedBy { get; set; }

        public UserEntity CreatedByUser { get; set; } = null!;
        public UserEntity? ChangedByUser { get; set; }

        public virtual FileReferenceEntity FileReference { get; set; } = null!;
    }
}
