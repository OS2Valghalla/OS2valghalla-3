using Valghalla.Application.CPR;
using Valghalla.Internal.Application.Modules.Participant.Commands;

namespace Valghalla.Internal.Application.Modules.Participant.Interfaces
{
    public interface IParticipantCommandRepository
    {
        Task<Guid> CreateParticipantAsync(CreateParticipantCommand command, ParticipantPersonalRecord record, CancellationToken cancellationToken);
        Task UpdateParticipantAsync(UpdateParticipantCommand command, CancellationToken cancellationToken);
    }
}
