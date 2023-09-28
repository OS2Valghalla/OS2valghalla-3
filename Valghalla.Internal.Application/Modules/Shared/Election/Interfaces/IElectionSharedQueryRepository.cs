using Valghalla.Internal.Application.Modules.Shared.Election.Queries;
using Valghalla.Internal.Application.Modules.Shared.Election.Responses;

namespace Valghalla.Internal.Application.Modules.Shared.Election.Interfaces
{
    public interface IElectionSharedQueryRepository
    {
        Task<IEnumerable<ElectionSharedResponse>> GetElectionsAsync(GetElectionsSharedQuery query, CancellationToken cancellationToken);
    }
}
