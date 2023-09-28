using Valghalla.External.Application.Modules.Team.Responses;

namespace Valghalla.External.Application.Modules.Team.Interfaces
{
    public interface ITeamQueryRepository
    {
        Task<IList<TeamResponse>> GetMyTeamsAsync(Guid participantId, CancellationToken cancellationToken);
        Task<IList<TeamMemberResponse>> GetTeamMembersAsync(Guid teamId, Guid participantId, CancellationToken cancellationToken);
    }
}
