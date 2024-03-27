using System.Security.Claims;
using Valghalla.Application.Auth;
using Valghalla.Application.Saml;
using Valghalla.Integration.Saml;

namespace Valghalla.External.API.Auth
{
    internal class Saml2AuthPostProcessor : ISaml2AuthPostProcessor
    {
        private readonly IUserTokenManager userTokenManager;

        public Saml2AuthPostProcessor(IUserTokenManager userTokenManager)
        {
            this.userTokenManager = userTokenManager;
        }

        public async Task<ClaimsPrincipal> HandleAsync(ClaimsPrincipal claimsPrincipal, CancellationToken cancellationToken)
        {
            var principal = TransformClaimsPrincipal(claimsPrincipal);

            var cpr = principal.GetCpr();

            if (string.IsNullOrEmpty(cpr))
            {
                throw new Exception("CPR could not be found in claims principal.");
            }

            await userTokenManager.EnsureUserTokenAsync(principal, cancellationToken);

            return principal;
        }

        private static ClaimsPrincipal TransformClaimsPrincipal(ClaimsPrincipal claimsPrincipal)
        {
            var claims = new List<Claim>();

            var identifier = claimsPrincipal.FindFirst(OioSaml3ClaimTypes.CprNumber)!;
            var name = claimsPrincipal.FindFirst(OioSaml3ClaimTypes.FullName)!;
            var serial = claimsPrincipal.FindFirst(OioSaml3ClaimTypes.CprUuid)!;

            if (!string.IsNullOrEmpty(identifier.Value))
            {
                claims.Add(new Claim(ClaimsPrincipalExtension.Cpr, identifier.Value));
            }

            if (!string.IsNullOrEmpty(name.Value))
            {
                claims.Add(new Claim(ClaimsPrincipalExtension.Name, name.Value));
            }

            if (!string.IsNullOrEmpty(serial.Value))
            {
                claims.Add(new Claim(ClaimsPrincipalExtension.Serial, serial.Value));
            }

            claimsPrincipal.AddIdentity(new ClaimsIdentity(claims));

            return claimsPrincipal;
        }
    }
}
