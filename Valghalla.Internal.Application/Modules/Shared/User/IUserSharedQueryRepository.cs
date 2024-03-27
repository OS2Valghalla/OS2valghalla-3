using Valghalla.Application.User;

namespace Valghalla.Internal.Application.Modules.Shared.User
{
    public interface IUserSharedQueryRepository
    {
        Task<UserInfo?> GetUserInfoAsync(string cvrNumber, string serial, CancellationToken cancellationToken);
    }
}
