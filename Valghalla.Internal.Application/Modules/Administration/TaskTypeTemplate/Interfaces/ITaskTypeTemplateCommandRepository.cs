using Valghalla.Internal.Application.Modules.Administration.TaskTypeTemplate.Commands;

namespace Valghalla.Internal.Application.Modules.Administration.TaskTypeTemplate.Interfaces
{
    public interface ITaskTypeTemplateCommandRepository
    {
        Task<Guid> CreateTaskTypeTemplateAsync(CreateTaskTypeTemplateCommand command, CancellationToken cancellationToken);
        Task DeleteTaskTypeTemplateAsync(DeleteTaskTypeTemplateCommand command, CancellationToken cancellationToken);
        Task UpdateTaskTypeTemplateAsync(UpdateTaskTypeTemplateCommand command, CancellationToken cancellationToken);
    }
}
