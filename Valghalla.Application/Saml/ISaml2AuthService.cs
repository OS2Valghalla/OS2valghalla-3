using System.Security.Claims;

namespace Valghalla.Application.Saml
{
    public interface ISaml2AuthService
    {
        Task<(string, string)> GetLoginRedirectUrlAsync(CancellationToken cancellationToken);
        Task<string> GetFallbackLoginRedirectUrlAsync(CancellationToken cancellationToken);
        Task<string> LogoutAsync(ClaimsPrincipal principal, bool profileDeleted, CancellationToken cancellationToken);
        Task<string> SetupAssertionConsumerServiceAsync(bool isInternal, CancellationToken cancellationToken);
        Task<string> SetupLogoutResponseAsync(string logoutPath, CancellationToken cancellationToken);
        Task<bool> GetServiceHealthCheck(string url, CancellationToken cancellationToken);
    }
}
