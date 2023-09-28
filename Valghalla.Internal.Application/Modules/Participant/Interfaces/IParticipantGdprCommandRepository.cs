using Valghalla.Internal.Application.Modules.Participant.Commands;

namespace Valghalla.Internal.Application.Modules.Participant.Interfaces
{
    public interface IParticipantGdprCommandRepository
    {
        Task<string> DeleteParticipantAsync(DeleteParticipantCommand command, CancellationToken cancellationToken);
        Task<IEnumerable<string>> DeleteParticipantsAsync(BulkDeleteParticipantsCommand command, CancellationToken cancellationToken);
    }
}
