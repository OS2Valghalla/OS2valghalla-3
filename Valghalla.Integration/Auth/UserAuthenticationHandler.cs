using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text.Encodings.Web;
using Valghalla.Application.Auth;

namespace Valghalla.Integration.Auth
{
    internal class UserAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private readonly IUserTokenManager userTokenManager;

        public UserAuthenticationHandler
            (IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock,
            IUserTokenManager userTokenManager) : base(options, logger, encoder, clock)
        {
            this.userTokenManager = userTokenManager;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!AuthenticationUtilities.IsApiEndpoint(Context) || AuthenticationUtilities.IsAnonymousEndpoint(Context))
            {
                await Task.CompletedTask;
                return AuthenticateResult.NoResult();
            }    

            var token = await userTokenManager.EnsureUserTokenAsync(Context.RequestAborted);

            if (token == null)
            {
                return AuthenticateResult.Fail("Token expired or does not exist.");
            }

            var ticket = new AuthenticationTicket(token.ToClaimsPrincipal(), Scheme.Name);
            return AuthenticateResult.Success(ticket);
        }
    }
}
