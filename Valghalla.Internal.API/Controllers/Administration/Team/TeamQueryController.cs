using MediatR;
using Microsoft.AspNetCore.Mvc;
using Valghalla.Integration.Auth;
using Valghalla.Internal.Application.Modules.Administration.Team.Queries;

namespace Valghalla.Internal.API.Controllers.Administration.Team
{
    [ApiController]
    [Route("api/administration/team")]
    [UserAuthorize(RoleEnum.Administrator)]
    public class TeamQueryController : ControllerBase
    {
        private readonly ISender sender;

        public TeamQueryController(ISender sender)
        {
            this.sender = sender;
        }

        [HttpPost("queryteamlisting")]
        public async Task<IActionResult> QueryTeamListingAsync(TeamListingQueryForm form, CancellationToken cancellationToken)
        {
            var result = await sender.Send(form, cancellationToken);
            return Ok(result);
        }

        [HttpPost("getteamlistingqueryform")]
        public async Task<IActionResult> GetTeamListingQueryForm(TeamListingQueryFormParameters @params, CancellationToken cancellationToken)
        {
            var result = await sender.Send(@params, cancellationToken);
            return Ok(result);
        }


        [HttpGet("getteam")]
        public async Task<IActionResult> GetTeamAsync(Guid teamId, CancellationToken cancellationToken)
        {
            var query = new GetTeamQuery(teamId);
            var result = await sender.Send(query, cancellationToken);

            return Ok(result);
        }

        [HttpGet("getallteams")]
        public async Task<IActionResult> GetAllTeamsAsync(CancellationToken cancellationToken)
        {
            var query = new GetAllTeamsQuery();
            var result = await sender.Send(query, cancellationToken);

            return Ok(result);
        }
    }
}
