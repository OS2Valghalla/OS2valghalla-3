using MediatR;
using Microsoft.AspNetCore.Mvc;
using Valghalla.Application.Auth;
using Valghalla.Application.User;
using Valghalla.External.API.Auth;
using Valghalla.External.API.Requests.Registration;
using Valghalla.External.Application.Modules.Registration.Commands;
using Valghalla.Integration.Auth;

namespace Valghalla.External.API.Controllers.Registration
{
    [ApiController]
    [Route("api/registration")]
    public class RegistrationCommandController : ControllerBase
    {
        private readonly ISender sender;
        private readonly IUserContextProvider userContextProvider;
        private readonly UserContextInternalProvider userContextInternalProvider;

        public RegistrationCommandController(
            ISender sender,
            IUserContextProvider userContextProvider,
            UserContextInternalProvider userContextInternalProvider)
        {
            this.sender = sender;
            this.userContextProvider = userContextProvider;
            this.userContextInternalProvider = userContextInternalProvider;
        }

        [HttpPost("registerwithteam")]
        public async Task<IActionResult> RegisterParticipantWithTeamAsync(ParticipantRegisterRequest request, CancellationToken cancellationToken)
        {
            if (userContextProvider.Registered)
            {
                return BadRequest();
            }

            var cpr = HttpContext.User.GetCpr();

            if (string.IsNullOrEmpty(cpr))
            {
                return BadRequest();
            }

            // database audit required a user context for tracking so we will set app user in here
            userContextInternalProvider.SetUserContext(UserContext.App);

            var command = new RegisterParticipantWithTeamCommand()
            {
                Cpr = cpr,
                HashValue = request.HashValue,
                Email = request.Email,
                MobileNumber = request.MobileNumber,
                SpecialDietIds = request.SpecialDietIds,
            };

            var response = await sender.Send(command, cancellationToken);
            return Ok(response);
        }

        [HttpPost("registerwithtask")]
        public async Task<IActionResult> RegisterParticipantWithTaskAsync([FromBody] ParticipantRegisterRequest request, [FromQuery] Guid? code, CancellationToken cancellationToken)
        {
            if (userContextProvider.Registered)
            {
                return BadRequest();
            }

            var cpr = HttpContext.User.GetCpr();

            if (string.IsNullOrEmpty(cpr))
            {
                return BadRequest();
            }

            // database audit required a user context for tracking so we will set app user in here
            userContextInternalProvider.SetUserContext(UserContext.App);

            var command = new RegisterParticipantWithTaskCommand()
            {
                Cpr = cpr,
                HashValue = request.HashValue,
                InvitationCode = code,
                Email = request.Email,
                MobileNumber = request.MobileNumber,
                SpecialDietIds = request.SpecialDietIds,
            };

            var response = await sender.Send(command, cancellationToken);
            return Ok(response);
        }

        [HttpPost("jointeam")]
        [UserAuthorize(RoleEnum.Participant)]
        public async Task<IActionResult> JoinTeamAsync(string hashValue, CancellationToken cancellationToken)
        {
            var command = new JoinTeamCommand(hashValue);
            var response = await sender.Send(command, cancellationToken);
            return Ok(response);
        }
    }
}
