using MediatR;
using Microsoft.AspNetCore.Mvc;
using Valghalla.Integration.Auth;
using Valghalla.Internal.Application.Modules.Tasks.Queries;

namespace Valghalla.Internal.API.Controllers.Tasks
{
    [ApiController]
    [Route("api/worklocationtasks")]
    [UserAuthorize(RoleEnum.Administrator)]
    public class WorkLocationTasksQueryController: ControllerBase
    {
        private readonly ISender sender;

        public WorkLocationTasksQueryController(ISender sender)
        {
            this.sender = sender;
        }

        [HttpGet("gettaskassignment")]
        public async Task<IActionResult> GetTaskAssignmentAsync(Guid taskAssignmentId, Guid electionId, CancellationToken cancellationToken)
        {
            var query = new GetTaskAssignmentQuery(taskAssignmentId, electionId);
            var result = await sender.Send(query, cancellationToken);
            return Ok(result);
        }

        [HttpGet("getworklocationtaskssummary")]
        public async Task<IActionResult> GetWorkLocationTasksSummaryAsync(Guid workLocationId, Guid electionId, CancellationToken cancellationToken)
        {
            var query = new GetElectionWorkLocationTasksSummaryQuery(workLocationId, electionId);
            var result = await sender.Send(query, cancellationToken);
            return Ok(result);
        }


        [HttpGet("getteamtasks")]
        public async Task<IActionResult> GetTeamTasksAsync(Guid teamId, Guid workLocationId, Guid electionId, bool? isGettingRejectedTasks, CancellationToken cancellationToken)
        {
            var query = new GetTeamTasksQuery(teamId, workLocationId, electionId, isGettingRejectedTasks);
            var result = await sender.Send(query, cancellationToken);
            return Ok(result);
        }
    }
}
