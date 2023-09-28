using Valghalla.Internal.Application.Modules.Administration.TaskType.Commands;

namespace Valghalla.Internal.Application.Modules.Administration.TaskType.Interfaces
{
    public interface ITaskTypeCommandRepository
    {
        Task<Guid> CreateTaskTypeAsync(CreateTaskTypeCommand command, CancellationToken cancellationToken);
        Task DeleteTaskTypeAsync(DeleteTaskTypeCommand command, CancellationToken cancellationToken);
        Task UpdateTaskTypeAsync(UpdateTaskTypeCommand command, CancellationToken cancellationToken);
    }
}
