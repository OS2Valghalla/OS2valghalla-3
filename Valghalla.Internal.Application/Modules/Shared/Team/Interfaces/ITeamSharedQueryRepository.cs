using Valghalla.Internal.Application.Modules.Shared.Team.Queries;
using Valghalla.Internal.Application.Modules.Shared.Team.Responses;

namespace Valghalla.Internal.Application.Modules.Shared.Team.Interfaces
{
    public interface ITeamSharedQueryRepository
    {
        Task<IEnumerable<TeamSharedResponse>> GetTeamsAsync(GetTeamsSharedQuery query, CancellationToken cancellationToken);
    }
}
