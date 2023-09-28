using Valghalla.Internal.Application.Modules.Shared.ElectionType.Queries;
using Valghalla.Internal.Application.Modules.Shared.ElectionType.Responses;

namespace Valghalla.Internal.Application.Modules.Shared.ElectionType.Interfaces
{
    public interface IElectionTypeSharedQueryRepository
    {
        Task<IEnumerable<ElectionTypeSharedResponse>> GetElectionTypesAsync(GetElectionTypesSharedQuery query, CancellationToken cancellationToken);
    }
}
