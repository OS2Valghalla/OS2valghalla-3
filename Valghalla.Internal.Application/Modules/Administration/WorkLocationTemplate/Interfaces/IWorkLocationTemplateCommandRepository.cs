using Valghalla.Internal.Application.Modules.Administration.WorkLocation.Commands;
using Valghalla.Internal.Application.Modules.Administration.WorkLocationTemplate.Commands;

namespace Valghalla.Internal.Application.Modules.Administration.WorkLocationTemplate.Interfaces
{
    public interface IWorkLocationTemplateCommandRepository
    {
        Task<Guid> CreateWorkLocationTemplateAsync(CreateWorkLocationTemplateCommand command, CancellationToken cancellationToken);
        Task<int> UpdateWorkLocationTemplateAsync(UpdateWorkLocationTemplateCommand command, CancellationToken cancellationToken);
        Task<int> DeleteWorkLocationTemplateAsync(DeleteWorkLocationTemplateCommand command, CancellationToken cancellationToken);
    }
}
