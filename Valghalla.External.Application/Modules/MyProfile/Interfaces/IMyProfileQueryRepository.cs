using Valghalla.External.Application.Modules.MyProfile.Responses;

namespace Valghalla.External.Application.Modules.MyProfile.Interfaces
{
    public interface IMyProfileQueryRepository
    {
        Task<MyProfileResponse?> GetMyProfileAsync(Guid participantId, CancellationToken cancellationToken);
        Task<bool> CheckIfMyProfileHasAssignedTaskLocked(Guid participantId, CancellationToken cancellationToken);
        Task<bool> CheckIfMyProfileHasCompletedTask(Guid participantId, CancellationToken cancellationToken);
    }
}
