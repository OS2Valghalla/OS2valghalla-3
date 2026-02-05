using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

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
        private readonly ILogger<AuthController> logger;

        public AuthController(
            ISender sender,
            ISaml2AuthService saml2AuthService,
            IUserTokenManager userTokenManager,
            ILogger<AuthController> logger)
        {
            this.sender = sender;
            this.saml2AuthService = saml2AuthService;
            this.userTokenManager = userTokenManager;
            this.logger = logger;
        }

        [HttpGet("ping")]
        public IActionResult Ping() => Ok();

        [HttpGet("login")]
        [AllowAnonymous]
        public async Task<IActionResult> LoginAsync(CancellationToken cancellationToken)
        {
            try
            {
                logger.LogDebug("[EXTERNAL-SAML2] Starting login flow from AuthController");

                var url = await saml2AuthService.GetLoginRedirectUrlAsync(cancellationToken);

                logger.LogDebug("[EXTERNAL-SAML2] Login redirect URL generated successfully (length: {UrlLength})", url.Length);

                return Redirect(url);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "[EXTERNAL-SAML2] Error in LoginAsync: {ErrorMessage}", ex.Message);
                throw;
            }
        }

        [HttpPost("AssertionConsumerService")]
        [AllowAnonymous]
        public async Task<IActionResult> SetupAssertionConsumerServiceAsync(CancellationToken cancellationToken)
        {
            try
            {
                logger.LogDebug("[EXTERNAL-SAML2] Starting SetupAssertionConsumerServiceAsync from AuthController");

                var redirectUrl = await saml2AuthService.SetupAssertionConsumerServiceAsync(false, cancellationToken);

                logger.LogInformation("[EXTERNAL-SAML2] User successfully authenticated and assertion processed");
                logger.LogDebug("[EXTERNAL-SAML2] Redirect URL: {RedirectUrl}", redirectUrl);

                return Redirect(redirectUrl);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "[EXTERNAL-SAML2] Error in SetupAssertionConsumerServiceAsync: {ErrorMessage}", ex.Message);
                throw;
            }
        }

        [HttpPost("logout")]
        [AllowAnonymous]
        public async Task<IActionResult> LogoutAsync([FromQuery] bool profileDeleted, CancellationToken cancellationToken)
        {
            try
            {
                logger.LogDebug("[EXTERNAL-SAML2] Starting logout flow. ProfileDeleted: {ProfileDeleted}", profileDeleted);

                var token = await userTokenManager.EnsureUserTokenAsync(cancellationToken);
                var principal = token?.ToClaimsPrincipal();

                if (principal == null)
                {
                    logger.LogWarning("[EXTERNAL-SAML2] No user principal found for logout");
                    return BadRequest();
                }

                logger.LogDebug("[EXTERNAL-SAML2] User principal found, proceeding with SAML2 logout");

                var redirectUrl = await saml2AuthService.LogoutAsync(principal, profileDeleted, cancellationToken);

                userTokenManager.ExpireUserToken();

                logger.LogInformation("[EXTERNAL-SAML2] User logout completed successfully. ProfileDeleted: {ProfileDeleted}", profileDeleted);

                return Content(redirectUrl);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "[EXTERNAL-SAML2] Error in LogoutAsync: {ErrorMessage}", ex.Message);
                throw;
            }
        }

        [HttpGet("SingleLogout")]
        [AllowAnonymous]
        public async Task<IActionResult> SetupLogoutResponseAsync(CancellationToken cancellationToken)
        {
            try
            {
                logger.LogDebug("[EXTERNAL-SAML2] Starting SetupLogoutResponseAsync (Single Logout) from AuthController");

                var redirectUrl = await saml2AuthService.SetupLogoutResponseAsync("/log-ud", cancellationToken);

                logger.LogInformation("[EXTERNAL-SAML2] Single logout response processed successfully");
                logger.LogDebug("[EXTERNAL-SAML2] Logout redirect URL: {RedirectUrl}", redirectUrl);

                return Redirect(redirectUrl);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "[EXTERNAL-SAML2] Error in SetupLogoutResponseAsync: {ErrorMessage}", ex.Message);
                throw;
            }
        }
    }
}
