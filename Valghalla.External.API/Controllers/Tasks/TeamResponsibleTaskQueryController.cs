using MediatR;
using Microsoft.AspNetCore.Mvc;
using Valghalla.External.Application.Modules.Tasks.Queries;
using Valghalla.External.Application.Modules.Tasks.Request;
using Valghalla.Integration.Auth;

namespace Valghalla.External.API.Controllers.Task
{
    [ApiController]
    [Route("api/teamResponsibleTasks")]
    [UserAuthorize(RoleEnum.TeamResponsible)]
    public class TeamResponsibleTaskQueryController : ControllerBase
    {
        private readonly ISender sender;

        public TeamResponsibleTaskQueryController(ISender sender)
        {
            this.sender = sender;
        }

        [HttpGet("getteamresponsibletasksfiltersoptions")]
        public async Task<IActionResult> GetTeamResponsibleTasksFiltersOptionsAsync(CancellationToken cancellationToken)
        {
            var query = new GetTeamResponsibleTasksFiltersOptionsQuery();
            var response = await sender.Send(query, cancellationToken);
            return Ok(response);
        }

        [HttpPost("getteamresponsibletasks")]
        public async Task<IActionResult> GetTeamResponsibleTasksAsync([FromBody] GetTeamResponsibleTasksRequest request, CancellationToken cancellationToken)
        {
            var query = new GetTeamResponsibleTasksQuery(request.TeamId, request.WorkLocationId, request.TaskTypeId, request.TaskDate);
            var response = await sender.Send(query, cancellationToken);
            return Ok(response);
        }

        [HttpGet("getteamresponsibletaskpreview")]
        public async Task<IActionResult> GetTeamResponsibleTaskPreviewAsync(string hashValue, Guid? code, CancellationToken cancellationToken)
        {
            var query = new GetTeamResponsibleTaskPreviewQuery(hashValue, code);
            var response = await sender.Send(query, cancellationToken);
            return Ok(response);
        }

    }
}
