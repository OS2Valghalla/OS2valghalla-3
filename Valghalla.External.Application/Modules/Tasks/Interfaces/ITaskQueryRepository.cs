using Valghalla.External.Application.Modules.Tasks.Commands;
using Valghalla.External.Application.Modules.Tasks.Queries;
using Valghalla.External.Application.Modules.Tasks.Responses;

namespace Valghalla.External.Application.Modules.Tasks.Interfaces
{
    public interface ITaskQueryRepository
    {
        Task<TaskOverviewFilterOptions> GetTaskOverviewFilterOptionsAsync(Guid participantId, GetTaskOverviewFilterOptionsQuery query, CancellationToken cancellationToken);
        Task<IEnumerable<TaskOverviewItem>> GetTaskOverviewAsync(Guid participantId, GetTaskOverviewQuery query, CancellationToken cancellationToken);
        Task<TaskPreviewResponse?> GetTaskPreviewAsync(GetTaskPreviewQuery query, Guid? participantId, CancellationToken cancellationToken);
        Task<TeamResponsibleTaskPreviewResponse?> GetTeamResponsibleTaskPreviewAsync(GetTeamResponsibleTaskPreviewQuery query, Guid? participantId, CancellationToken cancellationToken);
        Task<IList<TaskDetailsResponse>> GetMyTasksAsync(Guid participantId, CancellationToken cancellationToken);
        Task<TeamResponsibleTasksFiltersOptionsResponse> GetTeamResponsibleTasksFiltersOptionsAsync(Guid teamResponsibleId, CancellationToken cancellationToken);
        Task<TeamResponsibleTaskResponse> GetTeamResponsibleTasksAsync(Guid teamResponsibleId, GetTeamResponsibleTasksQuery query, CancellationToken cancellationToken);
        Task<TaskAssignmentResponse?> GetTaskAssignmentAsync(string hashValue, Guid? invitationCode, Guid participantId, CancellationToken cancellationToken);
        Task<bool> CheckIfTaskHasConflicts(Guid participantId, DateTime taskDate, TimeSpan startTime, TimeSpan endTime, Guid? invitationCode, CancellationToken cancellationToken);
    }
}
