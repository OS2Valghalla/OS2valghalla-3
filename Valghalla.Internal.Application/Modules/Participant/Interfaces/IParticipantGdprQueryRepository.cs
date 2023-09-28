using Valghalla.Internal.Application.Modules.Participant.Commands;

namespace Valghalla.Internal.Application.Modules.Participant.Interfaces
{
    public interface IParticipantGdprQueryRepository
    {
        Task<bool> CheckIfParticipantHasAssignedTasksInActiveElectionAsync(DeleteParticipantCommand command, CancellationToken cancellationToken);
        Task<bool> CheckIfParticipantIsTeamResponsibleAsync(DeleteParticipantCommand command, CancellationToken cancellationToken);
        Task<bool> CheckIfParticipantIsWorkLocationResponsibleAsync(DeleteParticipantCommand command, CancellationToken cancellationToken);
        Task<bool> CheckIfParticipantsContainAssignedTasksInActiveElectionAsync(BulkDeleteParticipantsCommand command, CancellationToken cancellationToken);
        Task<bool> CheckIfParticipantsAreTeamResponsiblesAsync(BulkDeleteParticipantsCommand command, CancellationToken cancellationToken);
        Task<bool> CheckIfParticipantsAreWorkLocationResponsiblesAsync(BulkDeleteParticipantsCommand command, CancellationToken cancellationToken);
    }
}
