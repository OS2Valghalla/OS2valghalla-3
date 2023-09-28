using Valghalla.Database.Entities.Tables;

namespace Valghalla.Database.Interceptors.ChangeTracking
{
    internal interface IElectionConnectedEntity
    {
        Guid ElectionId { get; }

        ElectionEntity Election { get; }
    }
}
