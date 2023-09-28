using Valghalla.Application.CPR;

namespace Valghalla.External.Application.Modules.Registration.Interfaces
{
    public interface IRegistrationCommandRepository
    {
        Task<Guid> CreateParticipantAsync(string cpr, string? mobileNumber, string? email, IEnumerable<Guid> specialDietIds, Guid teamId, ParticipantPersonalRecord record, CancellationToken cancellationToken);
        Task JoinTeamAsync(Guid participantId, Guid teamId, CancellationToken cancellationToken);
    }
}
