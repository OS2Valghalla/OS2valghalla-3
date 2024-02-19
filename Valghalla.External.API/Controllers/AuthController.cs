using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Valghalla.Application.Saml;
using Valghalla.Application.User;
using Valghalla.External.API.Auth;
using Valghalla.Integration.Saml;

namespace Valghalla.External.API.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly ISender sender;
        private readonly ISaml2AuthService saml2AuthService;
        private readonly IUserContextProvider userContextProvider;

        public AuthController(
            ISender sender,
            ISaml2AuthService saml2AuthService,
            IUserContextProvider userContextProvider)
        {
            this.sender = sender;
            this.saml2AuthService = saml2AuthService;
            this.userContextProvider = userContextProvider;
        }

        [HttpGet("state")]
        public IActionResult GetAuthState()
        {
            if (saml2AuthService.IsAuthenticated())
            {
                if (userContextProvider.Registered)
                {
                    return Ok();
                }

                return Unauthorized();
            }

            saml2AuthService.ClearClientSession();

            return Content(GetLoginUrl());
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
            var redirectUrl = await saml2AuthService.SetupAssertionConsumerServiceAsync(TransformClaims, false, cancellationToken);
            saml2AuthService.SaveClientSession();

            return Redirect(redirectUrl);
        }

        [HttpPost("logout")]
        public async Task<IActionResult> LogoutAsync([FromQuery] bool profileDeleted, CancellationToken cancellationToken)
        {
            var redirectUrl = await saml2AuthService.LogoutAsync(profileDeleted, cancellationToken);

            return Content(redirectUrl);
        }

        [HttpGet("SingleLogout")]
        public async Task<IActionResult> SetupLogoutResponseAsync(CancellationToken cancellationToken)
        {
            var redirectUrl = await saml2AuthService.SetupLogoutResponseAsync("/log-ud", cancellationToken);
            saml2AuthService.ClearClientSession();

            return Redirect(redirectUrl);
        }

        private static ClaimsPrincipal TransformClaims(ClaimsPrincipal claimsPrincipal)
        {
            var claims = new List<Claim>();

            var identifier = claimsPrincipal.FindFirst(OioSaml3ClaimTypes.CprNumber)!;
            var name = claimsPrincipal.FindFirst(OioSaml3ClaimTypes.FullName)!;
            var serial = claimsPrincipal.FindFirst(OioSaml3ClaimTypes.CprUuid)!;

            if (!string.IsNullOrEmpty(identifier.Value))
            {
                claims.Add(new Claim(AppClaimTypes.Cpr, identifier.Value));
            }

            if (!string.IsNullOrEmpty(name.Value))
            {
                claims.Add(new Claim(AppClaimTypes.Name, name.Value));
            }

            if (!string.IsNullOrEmpty(serial.Value))
            {
                claims.Add(new Claim(AppClaimTypes.Serial, serial.Value));
            }

            claimsPrincipal.AddIdentity(new ClaimsIdentity(claims));


            return claimsPrincipal;
        }

        private string GetLoginUrl() => $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}{Valghalla.Application.Constants.Authentication.LoginPath}";
    }
}
