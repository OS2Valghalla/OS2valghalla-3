using Valghalla.Internal.Application.Modules.Shared.Area.Queries;
using Valghalla.Internal.Application.Modules.Shared.Area.Responses;

namespace Valghalla.Internal.Application.Modules.Shared.Area.Interfaces
{
    public interface IAreaSharedQueryRepository
    {
        Task<IEnumerable<AreaSharedResponse>> GetAreasAsync(GetAreasSharedQuery query, CancellationToken cancellationToken);
    }
}
