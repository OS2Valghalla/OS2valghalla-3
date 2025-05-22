using MediatR;

using Microsoft.AspNetCore.Mvc;

using Valghalla.Integration.Auth;
using Valghalla.Internal.Application.Modules.Tasks.Commands;
using Valghalla.Internal.Application.Modules.Tasks.Requests;

namespace Valghalla.Internal.API.Controllers.Tasks
{
    [ApiController]
    [Route("api/worklocationtasks")]
    [UserAuthorize(RoleEnum.Administrator)]
    public class WorkLocationTasksCommandController : ControllerBase
    {
        private readonly ISender sender;

        public WorkLocationTasksCommandController(ISender sender)
        {
            this.sender = sender;
        }

        [HttpPut("distributeworklocationtasks")]
        public async Task<IActionResult> DistributeWorkLocationTasksAsync([FromBody] ElectionWorkLocationTasksDistributingRequest request, CancellationToken cancellationToken)
        {
            var command = new DistributeElectionWorkLocationTasksCommand()
            {
                ElectionId = request.ElectionId,
                WorkLocationId = request.WorkLocationId,
                DistributingTasks = request.DistributingTasks
            };

            var result = await sender.Send(command, cancellationToken);
            return Ok(result);
        }

        [HttpPost("assignparticipanttotask")]
        public async Task<IActionResult> AssignParticipantToTaskAsync([FromBody] AssignParticipantToTaskRequest request, CancellationToken cancellationToken)
        {
            var command = new AssignParticipantToTaskCommand()
            {
                ElectionId = request.ElectionId,
                TaskAssignmentId = request.TaskAssignmentId,
                ParticipantId = request.ParticipantId,
                TaskTypeId = request.TaskTypeId
            };

            var result = await sender.Send(command, cancellationToken);
            return Ok(result);
        }

        [HttpPost("removeparticipantfromtask")]
        public async Task<IActionResult> RemoveParticipantFromTaskAsync([FromBody] RemoveParticipantFromTaskRequest request, CancellationToken cancellationToken)
        {
            var command = new RemoveParticipantFromTaskCommand()
            {
                ElectionId = request.ElectionId,
                TaskAssignmentId = request.TaskAssignmentId
            };

            var result = await sender.Send(command, cancellationToken);
            return Ok(result);
        }

        [HttpPost("replyforparticipant")]
        public async Task<IActionResult> ReplyForParticipantAsync([FromBody] ReplyForParticipantRequest request, CancellationToken cancellationToken)
        {
            var command = new ReplyForParticipantCommand()
            {
                ElectionId = request.ElectionId,
                TaskAssignmentId = request.TaskAssignmentId,
                Accepted = request.Accepted,
                Email = request.Email,
                MobileNumber = request.MobileNumber,
                SpecialDietIds = request.SpecialDietIds,
            };

            var result = await sender.Send(command, cancellationToken);
            return Ok(result);
        }
        [HttpPost("movetasks")]
        public async Task<IActionResult> MoveTasksAsync([FromBody] MoveTasksRequest request, CancellationToken cancellationToken)
        {
            var command = new MoveTasksCommand()
            {
                TaskIds = request.TaskIds,
                SourceTeamId = request.sourceTeamId,
                TargetTeamId = request.targetTeamId
            };
            var result = await sender.Send(command, cancellationToken);
            return Ok(result);
        }
    }
}
