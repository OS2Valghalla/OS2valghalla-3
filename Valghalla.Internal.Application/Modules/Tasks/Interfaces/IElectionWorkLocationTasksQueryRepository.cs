using Valghalla.Internal.Application.Modules.Tasks.Queries;
using Valghalla.Internal.Application.Modules.Tasks.Responses;

namespace Valghalla.Internal.Application.Modules.Tasks.Interfaces
{
    public interface IElectionWorkLocationTasksQueryRepository
    {
        Task<bool> CheckIfWorkLocationInElectionAsync(Guid workLocationId, Guid electionId, CancellationToken cancellationToken);
        Task<ElectionWorkLocationTasksSummaryResponse> GetElectionWorkLocationTasksSummaryAsync(GetElectionWorkLocationTasksSummaryQuery query, CancellationToken cancellationToken);
        Task<TaskAssignmentResponse> GetTaskAssignmentAsync(GetTaskAssignmentQuery query, CancellationToken cancellationToken);
        Task<IEnumerable<TaskAssignmentResponse>> GetTeamTasksAsync(GetTeamTasksQuery query, string taskDetailsPageUrl, CancellationToken cancellationToken);
        Task<IEnumerable<TaskAssignmentResponse>> GetRejectedTeamTasksAsync(GetTeamTasksQuery query, CancellationToken cancellationToken);
        Task<bool> CheckIfTaskHasConflictsAsync(Guid electionId, Guid taskAssignmentId, Guid participantId, CancellationToken cancellationToken);
    }
}
