using Valghalla.External.Application.Modules.Team.Responses;
using Valghalla.External.Application.Modules.Unprotected.Queries;

namespace Valghalla.External.Application.Modules.Unprotected.Interfaces
{
    public interface IUnprotectedTeamQueryRepository
    {
        Task<TeamResponse> GetTeamByTeamLinkAsync(GetTeamByTeamLinkQuery query, CancellationToken cancellationToken);
    }
}
