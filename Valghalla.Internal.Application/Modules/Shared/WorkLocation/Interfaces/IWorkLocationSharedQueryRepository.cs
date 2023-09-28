using Valghalla.Internal.Application.Modules.Shared.WorkLocation.Queries;
using Valghalla.Internal.Application.Modules.Shared.WorkLocation.Responses;

namespace Valghalla.Internal.Application.Modules.Shared.WorkLocation.Interfaces
{
    public interface IWorkLocationSharedQueryRepository
    {
        Task<IEnumerable<WorkLocationSharedResponse>> GetWorkLocationsAsync(GetWorkLocationsSharedQuery query, CancellationToken cancellationToken);
        Task<WorkLocationSharedResponse?> GetWorkLocationAsync(GetWorkLocationSharedQuery query, CancellationToken cancellationToken);
    }
}
