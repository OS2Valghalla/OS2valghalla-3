using MediatR;
using Microsoft.AspNetCore.Mvc;
using Valghalla.External.Application.Modules.Registration.Queries;
using Valghalla.Integration.Auth;

namespace Valghalla.External.API.Controllers.Registration
{
    [ApiController]
    [Route("api/registration")]
    public class RegistrationQueryController : ControllerBase
    {
        private readonly ISender sender;

        public RegistrationQueryController(ISender sender)
        {
            this.sender = sender;
        }

        [HttpGet("getmyprofileregistration")]
        [UserAuthorize(RoleEnum.Participant)]
        public async Task<IActionResult> GetMyProfileRegistrationAsync(CancellationToken cancellationToken)
        {
            var query = new GetMyProfileRegistrationQuery();
            var response = await sender.Send(query, cancellationToken);
            return Ok(response);
        }

        [HttpGet("getteamaccessstatus")]
        [UserAuthorize(RoleEnum.Participant)]
        public async Task<IActionResult> GetTeamAccessStatusAsync(string hashValue, CancellationToken cancellationToken)
        {
            var query = new GetTeamAccessStatusQuery(hashValue);
            var response = await sender.Send(query, cancellationToken);
            return Ok(response);
        }
    }
}
