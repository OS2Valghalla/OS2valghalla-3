using Valghalla.External.Application.Modules.Tasks.Commands;

namespace Valghalla.External.Application.Modules.Tasks.Interfaces
{
    public interface ITaskCommandRepository
    {
        Task AcceptTaskAsync(AcceptTaskCommand command, Guid participantId, CancellationToken cancellationToken);
        Task RejectTaskAsync(RejectTaskCommand command, Guid participantId, CancellationToken cancellationToken);
        Task UnregisterTaskAsync(UnregisterTaskCommand command, Guid participantId, CancellationToken cancellationToken);
    }
}
