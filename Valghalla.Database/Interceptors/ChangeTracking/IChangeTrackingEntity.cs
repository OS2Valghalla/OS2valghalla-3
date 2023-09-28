using Valghalla.Database.Entities.Tables;

namespace Valghalla.Database.Interceptors.ChangeTracking
{
    internal interface IChangeTrackingEntity
    {
        DateTime CreatedAt { get; }
        Guid CreatedBy { get; }
        DateTime? ChangedAt { get; }
        Guid? ChangedBy { get; }

        UserEntity CreatedByUser { get; }
        UserEntity? ChangedByUser { get; }
    }
}
