using Valghalla.Internal.Application.Modules.Administration.ElectionType.Queries;
using Valghalla.Internal.Application.Modules.Administration.ElectionType.Responses;

namespace Valghalla.Internal.Application.Modules.Administration.ElectionType.Interfaces
{
    public interface IElectionTypeQueryRepository
    {
        Task<ElectionTypeResponse?> GetElectionTypeAsync(GetElectionTypeQuery query, CancellationToken cancellationToken);
        Task<bool> CheckIfElectionTypeCanBeDeletedAsync(Guid electionTypeId, CancellationToken cancellationToken);
    }
}
