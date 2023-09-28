using Valghalla.Internal.Application.Modules.Participant.Commands;

namespace Valghalla.Internal.Application.Modules.Participant.Interfaces
{
    public interface IParticipantEventLogCommandRepository
    {
        Task<Guid> CreateParticipantEventLogAsync(CreateParticipantEventLogCommand command, CancellationToken cancellationToken);
        Task UpdateParticipantEventLogAsync(UpdateParticipantEventLogCommand command, CancellationToken cancellationToken);
        Task DeleteParticipantEventLogAsync(DeleteParticipantEventLogCommand command, CancellationToken cancellationToken);
    }
}
