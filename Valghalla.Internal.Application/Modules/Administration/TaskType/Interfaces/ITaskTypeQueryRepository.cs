using Valghalla.Internal.Application.Modules.Administration.TaskType.Commands;
using Valghalla.Internal.Application.Modules.Administration.TaskType.Responses;

namespace Valghalla.Internal.Application.Modules.Administration.TaskType.Interfaces
{
    public interface ITaskTypeQueryRepository
    {
        Task<bool> CheckIfTaskTypeExistsAsync(CreateTaskTypeCommand command, CancellationToken cancellationToken);
        Task<bool> CheckIfTaskTypeExistsAsync(UpdateTaskTypeCommand command, CancellationToken cancellationToken);
        Task<bool> CheckIfTaskTypeHasTasksInActiveElectionAsync(UpdateTaskTypeCommand command, CancellationToken cancellationToken);
        Task<bool> CheckIfTaskTypeHasTasksInActiveElectionAsync(DeleteTaskTypeCommand command, CancellationToken cancellationToken);
        Task<TaskTypeDetailResponse?> GetTaskTypeAsync(Guid id, CancellationToken cancellationToken);
        Task<IList<TaskTypeListingItemResponse>> GetAllTaskTypesAsync(CancellationToken cancellationToken);
        Task<IList<TaskTypeListingItemResponse>> GetAllTaskTypesByElectionIdAsync(Guid electionId, CancellationToken cancellationToken);
    }
}
