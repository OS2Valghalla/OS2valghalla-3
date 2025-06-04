using Valghalla.Internal.Application.Modules.Tasks.Queries;
using Valghalla.Internal.Application.Modules.Tasks.Responses;

namespace Valghalla.Internal.Application.Modules.Tasks.Interfaces
{
    public interface IFilteredTasksQueryRepository
    {
        Task<TasksFiltersOptionsResponse> GetTasksFiltersOptionsAsync(GetTasksFiltersOptionsQuery query, CancellationToken cancellationToken);
        Task<IList<AvailableTasksDetailsResponse>> GetAvailableTasksByFiltersAsync(GetAvailableTasksByFiltersQuery query, string taskDetailsPageUrl, CancellationToken cancellationToken);
        Task<IList<ParticipantTaskDetailsResponse>> GetParticipantTasksAsync(GetParticipantsTasksQuery query, CancellationToken cancellationToken);
        Task<TaskStatusGeneralInfoResponse> GetParticipantTasksStatusAsync(GetParticipantsTasksStatusQuery query, CancellationToken cancellationToken);
    }
}
