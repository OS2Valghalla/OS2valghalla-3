using Valghalla.Internal.Application.Modules.Shared.Participant.Queries;
using Valghalla.Internal.Application.Modules.Shared.Participant.Responses;

namespace Valghalla.Internal.Application.Modules.Shared.Participant.Interfaces
{
    public interface IParticipantSharedQueryRepository
    {
        Task<IEnumerable<ParticipantSharedResponse>> GetPariticipantsAsync(GetParticipantsSharedQuery query, CancellationToken cancellationToken);
    }
}
