using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Valghalla.Application.Auth;
using Valghalla.Application.Saml;

namespace Valghalla.Internal.API.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly ISender sender;
        private readonly ISaml2AuthService saml2AuthService;
        private readonly IUserTokenManager userTokenManager;

        public AuthController(
            ISender sender,
            ISaml2AuthService saml2AuthService,
            IUserTokenManager userTokenManager)
        {
            this.sender = sender;
            this.saml2AuthService = saml2AuthService;
            this.userTokenManager = userTokenManager;
        }

        [HttpGet("ping")]
        public IActionResult Ping() => Ok();

        [HttpGet("login")]
        [AllowAnonymous]
        public async Task<IActionResult> LoginAsync(CancellationToken cancellationToken)
        {
            var (url, host) = await saml2AuthService.GetLoginRedirectUrlAsync(cancellationToken);

            if (url is not null && host is not null)
            {
                //if (await saml2AuthService.GetServiceHealthCheck(host, cancellationToken))
                //{
                //    return Redirect(url);
                //}
            }

            var fallbackUrl = await saml2AuthService.GetFallbackLoginRedirectUrlAsync(cancellationToken);
            return Redirect(fallbackUrl);
        }

        [HttpPost("AssertionConsumerService")]
        [AllowAnonymous]
        public async Task<IActionResult> SetupAssertionConsumerServiceAsync(CancellationToken cancellationToken)
        {
            var redirectUrl = await saml2AuthService.SetupAssertionConsumerServiceAsync(true, cancellationToken);
            return Redirect(redirectUrl);
        }

        [HttpPost("logout")]
        [AllowAnonymous]
        public async Task<IActionResult> LogoutAsync(CancellationToken cancellationToken)
        {
            var token = await userTokenManager.EnsureUserTokenAsync(cancellationToken);
            var principal = token?.ToClaimsPrincipal(includeSessionIndex: false);

            if (principal == null) return BadRequest();

            var redirectUrl = await saml2AuthService.LogoutAsync(principal, false, cancellationToken);

            userTokenManager.ExpireUserToken();

            return Content(redirectUrl);
        }

        [HttpGet("SingleLogout")]
        [AllowAnonymous]
        public async Task<IActionResult> SetupLogoutResponseAsync(CancellationToken cancellationToken)
        {
            var redirectUrl = await saml2AuthService.SetupLogoutResponseAsync("/_/logout", cancellationToken);

            return Redirect(redirectUrl);
        }
    }
}
