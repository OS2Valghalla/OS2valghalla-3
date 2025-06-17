using Valghalla.Internal.Application.Modules.Administration.TaskTypeTemplate.Commands;
using Valghalla.Internal.Application.Modules.Administration.TaskTypeTemplate.Responses;

namespace Valghalla.Internal.Application.Modules.Administration.TaskTypeTemplate.Interfaces
{
    public interface ITaskTypeTemplateQueryRepository
    {
        Task<bool> CheckIfTaskTypeTemplateExistsAsync(CreateTaskTypeTemplateCommand command, CancellationToken cancellationToken);
        Task<bool> CheckIfTaskTypeTemplateExistsAsync(UpdateTaskTypeTemplateCommand command, CancellationToken cancellationToken);
        Task<bool> CheckIfTaskTypeTemplateHasTasksInActiveElectionAsync(UpdateTaskTypeTemplateCommand command, CancellationToken cancellationToken);
        Task<bool> CheckIfTaskTypeTemplateHasTasksInActiveElectionAsync(DeleteTaskTypeTemplateCommand command, CancellationToken cancellationToken);
        Task<TaskTypeTemplateDetailResponse> GetTaskTypeTemplateAsync(Guid id, CancellationToken cancellationToken);
        Task<IList<TaskTypeTemplateListingItemResponse>> GetAllTaskTypeTemplatesAsync(CancellationToken cancellationToken);
    }
}
