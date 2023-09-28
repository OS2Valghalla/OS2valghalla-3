using Valghalla.Database.Interceptors.ChangeTracking;

namespace Valghalla.Database.Entities.Tables
{
    public partial class ElectionWorkLocationEntity : IChangeTrackingEntity
    {
        public Guid ElectionId { get; set; }
        public Guid WorkLocationId { get; set; }

        public DateTime CreatedAt { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime? ChangedAt { get; set; }
        public Guid? ChangedBy { get; set; }

        public virtual UserEntity CreatedByUser { get; set; } = null!;
        public virtual UserEntity? ChangedByUser { get; set; }
        public virtual ElectionEntity Election { get; set; } = null!;
        public virtual WorkLocationEntity WorkLocation { get; set; } = null!;
    }
}
