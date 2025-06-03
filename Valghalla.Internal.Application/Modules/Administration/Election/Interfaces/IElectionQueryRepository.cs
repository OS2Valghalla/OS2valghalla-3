using Valghalla.Internal.Application.Modules.Administration.Election.Commands;
using Valghalla.Internal.Application.Modules.Administration.Election.Queries;
using Valghalla.Internal.Application.Modules.Administration.Election.Responses;

namespace Valghalla.Internal.Application.Modules.Administration.Election.Interfaces
{
    public interface IElectionQueryRepository
    {
        Task<bool> CheckIfElectionExistsAsync(CreateElectionCommand command, CancellationToken cancellationToken);
        Task<bool> CheckIfElectionExistsAsync(UpdateElectionCommand command, CancellationToken cancellationToken);
        Task<bool> CheckIfElectionExistsAsync(DuplicateElectionCommand command, CancellationToken cancellationToken);
        Task<bool> CheckIfElectionIsActiveAsync(Guid electionId, CancellationToken cancellationToken);
        Task<ElectionDetailsResponse?> GetElectionAsync(GetElectionQuery query, CancellationToken cancellationToken);
        Task<List<ElectionDetailsResponse>?> GetElectionsAsync(GetElectionsQuery query, CancellationToken cancellationToken);
        Task<ElectionCommunicationConfigurationsResponse?> GetElectionCommunicationConfigurationsAsync(GetElectionCommunicationConfigurationsQuery query, CancellationToken cancellationToken);
    }
}
