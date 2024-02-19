using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Valghalla.Application.Saml
{
    public interface ISaml2AuthService
    {
        bool IsAuthenticated();
        void ClearClientSession();
        void SaveClientSession();
        Task<string> GetLoginRedirectUrlAsync(CancellationToken cancellationToken);
        Task<string> LogoutAsync(bool profileDeleted, CancellationToken cancellationToken);
        Task<string> SetupAssertionConsumerServiceAsync(Func<ClaimsPrincipal, ClaimsPrincipal> transform, bool isInternal, CancellationToken cancellationToken);
        Task<string> SetupLogoutResponseAsync(string logoutPath, CancellationToken cancellationToken);
    }
}
