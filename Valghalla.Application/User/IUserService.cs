using System.Security.Claims;

namespace Valghalla.Application.User
{
    public interface IUserService
    {
        Task<UserInfo?> GetUserInfoAsync(ClaimsPrincipal claimsPrincipal, CancellationToken cancellationToken);
    }
}
