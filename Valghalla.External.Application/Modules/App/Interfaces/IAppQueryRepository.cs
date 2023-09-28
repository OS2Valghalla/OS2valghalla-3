using Valghalla.Application.Web;
using Valghalla.External.Application.Modules.App.Queries;
using Valghalla.External.Application.Modules.App.Responses;

namespace Valghalla.External.Application.Modules.App.Interfaces
{
    public interface IAppQueryRepository
    {
        Task<bool> CheckIfElectionIsActivatedAsync(CancellationToken cancellationToken);
        Task<UserResponse?> GetUserAsync(GetExternalUserQuery query, CancellationToken cancellationToken);
        Task<IEnumerable<UserTeamResponse>> GetUserTeamsAsync(Guid participantId, CancellationToken cancellationToken);
        Task<ElectionCommitteeContactInformationPage> GetWebPageAsync(CancellationToken cancellationToken);
        Task<bool> CheckIfFAQPageActivatedAsync(CancellationToken cancellationToken);
    }
}
