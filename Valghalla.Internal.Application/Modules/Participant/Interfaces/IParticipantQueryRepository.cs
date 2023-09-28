using Valghalla.Internal.Application.Modules.Participant.Commands;
using Valghalla.Internal.Application.Modules.Participant.Queries;
using Valghalla.Internal.Application.Modules.Participant.Responses;

namespace Valghalla.Internal.Application.Modules.Participant.Interfaces
{
    public interface IParticipantQueryRepository
    {
        Task<bool> CheckIfParticipantExistsAsync(string cprNumber, CancellationToken cancellationToken);
        Task<bool> CheckIfParticipantHasAssociatedTasksAsync(UpdateParticipantCommand command, CancellationToken cancellationToken);
        Task<ParticipantDetailResponse?> GetParticipantDetailsAsync(GetParticipantDetailsQuery query, CancellationToken cancellationToken);
        Task<IEnumerable<Guid>> GetTeamResponsibleRightsAsync(GetTeamResponsibleRightsQuery query, CancellationToken cancellationToken);
        Task<IEnumerable<Guid>> GetWorkLocationResponsibleRightsAsync(GetWorkLocationResponsibleRightsQuery query, CancellationToken cancellationToken);
        Task<IList<ParticipantTaskResponse>> GetParticipantTasksAsync(GetParticipantTasksQuery query, CancellationToken cancellationToken);
    }
}