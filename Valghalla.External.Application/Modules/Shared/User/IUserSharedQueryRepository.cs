using Valghalla.Application.User;

namespace Valghalla.External.Application.Modules.Shared.User
{
    public interface IUserSharedQueryRepository
    {
        Task<UserInfo?> GetUserInfoAsync(string cprNumber, CancellationToken cancellationToken);
    }
}
