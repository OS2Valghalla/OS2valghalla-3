using Valghalla.Internal.Application.Modules.Administration.Team.Commands;
using Valghalla.Internal.Application.Modules.Administration.Team.Queries;
using Valghalla.Internal.Application.Modules.Administration.Team.Responses;

namespace Valghalla.Internal.Application.Modules.Administration.Team.Interfaces
{
    public interface ITeamQueryRepository
    {
        Task<bool> CheckIfTeamExistsAsync(CreateTeamCommand command, CancellationToken cancellationToken);
        Task<bool> CheckIfTeamExistsAsync(UpdateTeamCommand command, CancellationToken cancellationToken);
        Task<bool> CheckIfTeamHasTasksAsync(DeleteTeamCommand command, CancellationToken cancellationToken);
        Task<bool> CheckIfTeamUsedInActiveElectionAsync(Guid id, CancellationToken cancellationToken);
        Task<TeamDetailResponse?> GetTeamAsync(GetTeamQuery query, CancellationToken cancellationToken);
        Task<IList<ListTeamsItemResponse>> GetAllTeamsAsync(CancellationToken cancellationToken);
    }
}
