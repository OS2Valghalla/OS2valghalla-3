using Valghalla.Internal.Application.Modules.Administration.WorkLocation.Commands;

namespace Valghalla.Internal.Application.Modules.Administration.WorkLocation.Interfaces
{
    public interface IWorkLocationCommandRepository
    {
        Task<Guid> CreateWorkLocationAsync(CreateWorkLocationCommand command, CancellationToken cancellationToken);
        Task<(IEnumerable<Guid>, IEnumerable<Guid>)> UpdateWorkLocationAsync(UpdateWorkLocationCommand command, CancellationToken cancellationToken);
        Task<(IEnumerable<Guid>, string)> DeleteWorkLocationAsync(DeleteWorkLocationCommand command, CancellationToken cancellationToken);
    }
}
