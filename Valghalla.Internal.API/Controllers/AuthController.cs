using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Valghalla.Application.Saml;
using Valghalla.Application.User;
using Valghalla.Internal.API.Auth;

namespace Valghalla.Internal.API.Controllers
{
    [ApiController]
    [Route("api/auth")]
    [AllowAnonymous]
    public class AuthController : ControllerBase
    {
        private readonly ISender sender;
        private readonly ISaml2AuthService saml2AuthService;
        private readonly IUserContextProvider userContextProvider;
        private readonly ILogger<AuthController> logger;

        public AuthController(
            ISender sender,
            ISaml2AuthService saml2AuthService,
            IUserContextProvider userContextProvider,
            ILogger<AuthController> logger)
        {
            this.sender = sender;
            this.saml2AuthService = saml2AuthService;
            this.userContextProvider = userContextProvider;
            this.logger = logger;
        }

        [HttpGet("state")]
        public IActionResult GetAuthState()
        {
            if (!saml2AuthService.IsAuthenticated())
            {
                saml2AuthService.ClearClientSession();

                return Content(GetLoginUrl());
            }

            if (userContextProvider.Registered)
            {
                return Ok();
            }

            return Unauthorized();
        }

        [HttpGet("login")]
        public async Task<IActionResult> LoginAsync(CancellationToken cancellationToken)
        {
            var url = await saml2AuthService.GetLoginRedirectUrlAsync(cancellationToken);
            return Redirect(url);
        }

        [HttpPost("AssertionConsumerService")]
        public async Task<IActionResult> SetupAssertionConsumerServiceAsync(CancellationToken cancellationToken)
        {
            var redirectUrl = await saml2AuthService.SetupAssertionConsumerServiceAsync(TransformClaims, true, cancellationToken);
            return Redirect(redirectUrl);
        }

        [HttpPost("logout")]
        public async Task<IActionResult> LogoutAsync(CancellationToken cancellationToken)
        {
            if (!saml2AuthService.IsAuthenticated())
            {
                return BadRequest();
            }

            var redirectUrl = await saml2AuthService.LogoutAsync(false, cancellationToken);

            logger.LogInformation($"User signed out - Name: {userContextProvider.CurrentUser.Name} - Serial: {userContextProvider.CurrentUser.Serial}");

            return Content(redirectUrl);
        }

        [HttpGet("SingleLogout")]
        public async Task<IActionResult> SetupLogoutResponseAsync(CancellationToken cancellationToken)
        {
            var redirectUrl = await saml2AuthService.SetupLogoutResponseAsync("/_/logout", cancellationToken);
            return Redirect(redirectUrl);
        }

        private ClaimsPrincipal TransformClaims(ClaimsPrincipal claimsPrincipal)
        {
            var claims = new List<Claim>();

            var identifier = claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier)!;
            var keyValues = identifier.Value.Split(",", StringSplitOptions.RemoveEmptyEntries);
            var userName = string.Empty;
            var userSerial = string.Empty;

            foreach (var keyValue in keyValues)
            {
                var arrays = keyValue.Split("=");

                if (arrays.Length != 2) continue;

                var key = arrays[0];
                var value = arrays[1];

                if (key == Constants.NAME_KEY)
                {
                    userName = value;
                    claims.Add(new Claim(AppClaimTypes.Name, value));
                }
                else if (key == Constants.CVR_KEY)
                {
                    claims.Add(new Claim(AppClaimTypes.Cvr, value));
                }
                else if (key == Constants.SERIAL_KEY)
                {
                    userSerial = value;
                    claims.Add(new Claim(AppClaimTypes.Serial, value));
                }
            }

            claimsPrincipal.AddIdentity(new ClaimsIdentity(claims));

            logger.LogInformation($"User signed in - Name: {userName} - Serial: {userSerial}");

            return claimsPrincipal;
        }

        private string GetLoginUrl() => $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}{Valghalla.Application.Constants.Authentication.LoginPath}";
    }
}
