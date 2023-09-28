using Valghalla.Internal.Application.Modules.Administration.Link.Commands;

namespace Valghalla.Internal.Application.Modules.Administration.Link.Interfaces
{
    public interface ILinkCommandRepository
    {
        Task<string> CreateTaskLinkAsync(CreateTaskLinkCommand command, CancellationToken cancellationToken);
        Task<string> CreateTasksFilteredLinkAsync(CreateTasksFilteredLinkCommand command, CancellationToken cancellationToken);
    }
}
