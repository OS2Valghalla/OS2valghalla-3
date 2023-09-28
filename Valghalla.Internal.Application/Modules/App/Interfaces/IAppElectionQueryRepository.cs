using Valghalla.Internal.Application.Modules.App.Queries;
using Valghalla.Internal.Application.Modules.App.Responses;

namespace Valghalla.Internal.Application.Modules.App.Interfaces
{
    public interface IAppElectionQueryRepository
    {
        Task<AppElectionResponse?> GetDefaultElectionToWorkOnAsync(CancellationToken cancellationToken);
        Task<AppElectionResponse?> GetElectionToWorkOnAsync(Guid id, CancellationToken cancellationToken);
        Task<IEnumerable<AppElectionResponse>> GetElectionsToWorkOnAsync(GetElectionsToWorkOnQuery query, CancellationToken cancellationToken);
    }
}
