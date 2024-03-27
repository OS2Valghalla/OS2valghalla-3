using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Valghalla.Application.Auth;
using Valghalla.Application.Saml;

namespace Valghalla.External.API.Controllers
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
            var url = await saml2AuthService.GetLoginRedirectUrlAsync(cancellationToken);
            return Redirect(url);
        }

        [HttpPost("AssertionConsumerService")]
        [AllowAnonymous]
        public async Task<IActionResult> SetupAssertionConsumerServiceAsync(CancellationToken cancellationToken)
        {
            var redirectUrl = await saml2AuthService.SetupAssertionConsumerServiceAsync(false, cancellationToken);
            return Redirect(redirectUrl);
        }

        [HttpPost("logout")]
        [AllowAnonymous]
        public async Task<IActionResult> LogoutAsync([FromQuery] bool profileDeleted, CancellationToken cancellationToken)
        {
            var token = await userTokenManager.EnsureUserTokenAsync(cancellationToken);
            var principal = token?.ToClaimsPrincipal();

            if (principal == null) return BadRequest();

            var redirectUrl = await saml2AuthService.LogoutAsync(principal, profileDeleted, cancellationToken);

            userTokenManager.ExpireUserToken();

            return Content(redirectUrl);
        }

        [HttpGet("SingleLogout")]
        [AllowAnonymous]
        public async Task<IActionResult> SetupLogoutResponseAsync(CancellationToken cancellationToken)
        {
            var redirectUrl = await saml2AuthService.SetupLogoutResponseAsync("/log-ud", cancellationToken);
            return Redirect(redirectUrl);
        }
    }
}
