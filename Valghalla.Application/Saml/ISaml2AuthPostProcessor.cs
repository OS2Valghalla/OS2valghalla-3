using System.Security.Claims;

namespace Valghalla.Application.Saml
{
    public interface ISaml2AuthPostProcessor
    {
        Task<ClaimsPrincipal> HandleAsync(ClaimsPrincipal claimsPrincipal, CancellationToken cancellationToken);
    }
}
