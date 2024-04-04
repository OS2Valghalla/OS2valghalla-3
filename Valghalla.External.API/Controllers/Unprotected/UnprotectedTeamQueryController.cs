using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Valghalla.External.Application.Modules.Unprotected.Queries;

namespace Valghalla.External.API.Controllers.Unprotected
{
    [ApiController]
    [Route("api/unprotected/team")]
    [AllowAnonymous]
    public class UnprotectedTeamQueryController : ControllerBase
    {
        private readonly ISender sender;

        public UnprotectedTeamQueryController(ISender sender)
        {
            this.sender = sender;
        }

        [HttpGet("getteambyteamlink")]
        public async Task<IActionResult> GetTeamByTeamLinkAsync(string hashValue, CancellationToken cancellationToken)
        {
            var query = new GetTeamByTeamLinkQuery(hashValue);
            var response = await sender.Send(query, cancellationToken);
            return Ok(response);
        }
    }
}
