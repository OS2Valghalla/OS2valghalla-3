using Valghalla.External.Application.Modules.MyProfile.Commands;

namespace Valghalla.External.Application.Modules.MyProfile.Interfaces
{
    public interface IMyProfileCommandRepository
    {
        Task UpdateMyProfileAsync(Guid participantId , UpdateMyProfileCommand command, CancellationToken cancellationToken);
        Task DeleteMyProfileAsync(Guid participantId, CancellationToken cancellationToken);
    }
}
