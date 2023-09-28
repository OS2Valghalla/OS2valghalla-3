using Valghalla.External.Application.Modules.Registration.Queries;
using Valghalla.External.Application.Modules.Registration.Responses;

namespace Valghalla.External.Application.Modules.Registration.Interfaces
{
    public interface IRegistrationQueryRepository
    {
        Task<MyProfileRegistrationResponse> GetMyProfileRegistrationAsync(Guid participantId, CancellationToken cancellationToken);
        Task<Guid> GetTeamIdFromLink(string hashValue, CancellationToken cancellationToken);
        Task<Guid?> GetTeamIdFromTask(string hashValue, Guid? invitationCode, CancellationToken cancellationToken);
        Task<bool> CheckIfTeamExistsFromLink(string hashValue, CancellationToken cancellationToken);
        Task<bool> CheckIfTeamExistsFromTask(string hashValue, CancellationToken cancellationToken);
        Task<bool> CheckIfParticipantExistsAsync(string cprNumber, CancellationToken cancellationToken);
        Task<bool> CheckIfCurrentUserJoinedTeam(GetTeamAccessStatusQuery query, Guid participantId, CancellationToken cancellationToken);
    }
}
