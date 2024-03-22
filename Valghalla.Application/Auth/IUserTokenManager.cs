using System.Security.Claims;

namespace Valghalla.Application.Auth
{
    public interface IUserTokenManager
    {
        void ExpireUserToken();
        Task<UserToken?> EnsureUserTokenAsync(CancellationToken cancellationToken);
        Task<UserToken?> EnsureUserTokenAsync(ClaimsPrincipal principal, CancellationToken cancellationToken);
    }
}
