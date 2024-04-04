using System.Security.Claims;

namespace Valghalla.Application.Saml
{
    public interface ISaml2AuthService
    {
        Task<string> GetLoginRedirectUrlAsync(CancellationToken cancellationToken);
        Task<string> LogoutAsync(ClaimsPrincipal principal, bool profileDeleted, CancellationToken cancellationToken);
        Task<string> SetupAssertionConsumerServiceAsync(bool isInternal, CancellationToken cancellationToken);
        Task<string> SetupLogoutResponseAsync(string logoutPath, CancellationToken cancellationToken);
    }
}
