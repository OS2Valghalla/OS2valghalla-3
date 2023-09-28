using MediatR;
using Microsoft.AspNetCore.Mvc;
using Valghalla.External.Application.Modules.Team.Queries;
using Valghalla.Integration.Auth;

namespace Valghalla.External.API.Controllers.Team
{
    [ApiController]
    [Route("api/team")]
    [UserAuthorize(RoleEnum.TeamResponsible)]
    public class TeamQueryController: ControllerBase
    {
        private readonly ISender sender;

        public TeamQueryController(ISender sender)
        {
            this.sender = sender;
        }

        [HttpGet("getmyteams")]
        public async Task<IActionResult> GetMyTeamsAsync(CancellationToken cancellationToken)
        {
            var query = new GetMyTeamsQuery();
            var response = await sender.Send(query, cancellationToken);
            return Ok(response);
        }

        [HttpGet("getteammembers")]
        public async Task<IActionResult> GetTeamMembersAsync(Guid teamId, CancellationToken cancellationToken)
        {
            var query = new GetTeamMembersQuery(teamId);
            var response = await sender.Send(query, cancellationToken);
            return Ok(response);
        }
    }
}
