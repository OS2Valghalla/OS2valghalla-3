using MediatR;
using System.Security.Claims;
using Valghalla.Application.Abstractions.Messaging;
using Valghalla.Application.Auth;
using Valghalla.Application.Exceptions;
using Valghalla.Application.Saml;
using Valghalla.Application.User;
using Valghalla.Internal.Application.Modules.Administration.User.Commands;

namespace Valghalla.Internal.API.Auth
{
    internal class Saml2AuthPostProcessor : ISaml2AuthPostProcessor
    {
        private readonly ISender sender;
        private readonly IUserService userService;
        private readonly IUserTokenManager userTokenManager;
        private readonly ILogger<Saml2AuthPostProcessor> logger;

        public Saml2AuthPostProcessor(
            ISender sender,
            IUserService userService,
            IUserTokenManager userTokenManager,
            ILogger<Saml2AuthPostProcessor> logger)
        {
            this.sender = sender;
            this.userService = userService;
            this.userTokenManager = userTokenManager;
            this.logger = logger;
        }

        public async Task<ClaimsPrincipal> HandleAsync(ClaimsPrincipal claimsPrincipal, CancellationToken cancellationToken)
        {
            var principal = TransformClaimsPrincipal(claimsPrincipal);

            var userInfo = await userService.GetUserInfoAsync(principal, cancellationToken);

            if (userInfo == null)
            {
                var name = principal.GetName();
                var cvr = principal.GetCvr();
                var serial = principal.GetSerial();

                await CreateUserAsync(name ?? "Unknown", cvr, serial, cancellationToken);
            }

            await userTokenManager.EnsureUserTokenAsync(principal, cancellationToken);

            return principal;
        }

        private async Task CreateUserAsync(string name, string cvrNumber, string serial, CancellationToken cancellationToken)
        {
            try
            {
                var command = new CreateUserCommand(SystemRole.Administrator.Id, name, cvrNumber, serial);
                var response = await sender.Send(command, cancellationToken);

                if (response is not Response<Guid> userResponse)
                {
                    throw new UnableToCastToUserResponseException();
                }
            }
            catch (Exception ex)
            {
                throw new UserException(ex);
            }
        }

        private ClaimsPrincipal TransformClaimsPrincipal(ClaimsPrincipal claimsPrincipal)
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

                if (key == SamlConstants.NAME_KEY)
                {
                    userName = value;
                    claims.Add(new Claim(ClaimsPrincipalExtension.Name, value));
                }
                else if (key == SamlConstants.CVR_KEY)
                {
                    claims.Add(new Claim(ClaimsPrincipalExtension.Cvr, value));
                }
                else if (key == SamlConstants.SERIAL_KEY)
                {
                    userSerial = value;
                    claims.Add(new Claim(ClaimsPrincipalExtension.Serial, value));
                }
            }

            claimsPrincipal.AddIdentity(new ClaimsIdentity(claims));

            logger.LogInformation($"User signed in - Name: {userName} - Serial: {userSerial}");

            return claimsPrincipal;
        }
    }
}
