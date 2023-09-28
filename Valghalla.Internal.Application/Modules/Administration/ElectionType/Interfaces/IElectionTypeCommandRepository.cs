using Valghalla.Internal.Application.Modules.Administration.ElectionType.Commands;

namespace Valghalla.Internal.Application.Modules.Administration.ElectionType.Interfaces
{
    public interface IElectionTypeCommandRepository
    {
        Task<Guid> CreateElectionTypeAsync(CreateElectionTypeCommand command, CancellationToken cancellationToken);
        Task UpdateElectionTypeAsync(UpdateElectionTypeCommand command, CancellationToken cancellationToken);
        Task DeleteElectionTypeAsync(DeleteElectionTypeCommand command, CancellationToken cancellationToken);
    }
}
