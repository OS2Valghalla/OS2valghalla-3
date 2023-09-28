using MediatR;
using Microsoft.AspNetCore.Mvc;
using Valghalla.Application.Abstractions.Messaging;
using Valghalla.External.Application.Modules.App.Queries;
using Valghalla.Integration.Auth;

namespace Valghalla.External.API.Controllers.App
{
    [ApiController]
    [Route("api/app")]
    public class AppQueryController : ControllerBase
    {
        private readonly ISender sender;

        public AppQueryController(ISender sender)
        {
            this.sender = sender;
        }

        [HttpGet("context")]
        public async Task<IActionResult> GetAppContextAsync(CancellationToken cancellationToken)
        {
            var query = new GetAppContextQuery();
            var response = await sender.Send(query, cancellationToken);
            return Ok(response);
        }

        [HttpGet("logo")]
        public async Task<IActionResult> GetAppLogoAsync(CancellationToken cancellationToken)
        {
            var query = new GetAppLogoQuery();
            var result = await sender.Send(query, cancellationToken);

            if (result is not Response<string> fileResponse)
            {
                return BadRequest();
            }

            if (string.IsNullOrEmpty(fileResponse.Data))
            {
                return NotFound();
            }

            return Content(fileResponse.Data, "image/svg+xml");
        }

        [HttpGet("userteams")]
        [UserAuthorize(RoleEnum.Participant)]
        public async Task<IActionResult> GetUserTeamAsync(CancellationToken cancellationToken)
        {
            var query = new GetUserTeamsQuery();
            var response = await sender.Send(query, cancellationToken);
            return Ok(response);
        }
    }
}
