using MediatR;
using Microsoft.AspNetCore.Mvc;
using Valghalla.Integration.Auth;
using Valghalla.Internal.Application.Modules.Shared.Team.Queries;

namespace Valghalla.Internal.API.Controllers.Shared.Team
{
    [ApiController]
    [Route("api/shared/team")]
    [UserAuthorize(RoleEnum.Administrator)]
    public class TeamSharedQueryController : ControllerBase
    {
        private readonly ISender sender;

        public TeamSharedQueryController(ISender sender)
        {
            this.sender = sender;
        }

        [HttpGet("getteams")]
        public async Task<IActionResult> GetTeamsAsync(CancellationToken cancellationToken)
        {
            var query = new GetTeamsSharedQuery();
            var result = await sender.Send(query, cancellationToken);
            return Ok(result);
        }
    }
}
