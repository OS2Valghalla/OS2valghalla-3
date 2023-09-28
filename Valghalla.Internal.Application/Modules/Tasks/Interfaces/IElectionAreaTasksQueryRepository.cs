using Valghalla.Internal.Application.Modules.Tasks.Queries;
using Valghalla.Internal.Application.Modules.Tasks.Responses;

namespace Valghalla.Internal.Application.Modules.Tasks.Interfaces
{
    public interface IElectionAreaTasksQueryRepository
    {
        Task<ElectionAreasGeneralInfoResponse> GetElectionAreasGeneralInfoAsync(GetElectionAreasGeneralInfoQuery query, CancellationToken cancellationToken);
        Task<IList<WorkLocationTasksSummaryResponse>> GetElectionAreaTasksSummaryAsync(GetElectionAreaTasksSummaryQuery query, CancellationToken cancellationToken);        
    }
}
