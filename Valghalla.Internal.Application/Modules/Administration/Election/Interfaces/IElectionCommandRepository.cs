using Valghalla.Internal.Application.Modules.Administration.Election.Commands;

namespace Valghalla.Internal.Application.Modules.Administration.Election.Interfaces
{
    public interface IElectionCommandRepository
    {
        Task<Guid> CreateElectionAsync(CreateElectionCommand command, CancellationToken cancellationToken);
        Task UpdateElectionAsync(UpdateElectionCommand command, CancellationToken cancellationToken);
        Task<Guid> DuplicateElectionAsync(DuplicateElectionCommand command, CancellationToken cancellationToken);
        Task DeleteElectionAsync(DeleteElectionCommand command, CancellationToken cancellationToken);
        Task UpdateElectionCommunicationConfigurationsAsync(UpdateElectionCommunicationConfigurationsCommand command, CancellationToken cancellationToken);
        Task ActivateElectionAsync(ActivateElectionCommand command, CancellationToken cancellationToken);
        Task DeactivateElectionAsync(DeactivateElectionCommand command, CancellationToken cancellationToken);

    }
}
