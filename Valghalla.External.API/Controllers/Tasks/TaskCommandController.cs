using MediatR;
using Microsoft.AspNetCore.Mvc;
using Valghalla.External.Application.Modules.Tasks.Commands;
using Valghalla.Integration.Auth;

namespace Valghalla.External.API.Controllers.Tasks
{
    [ApiController]
    [Route("api/tasks")]
    [UserAuthorize(RoleEnum.Participant)]
    public class TaskCommandController : ControllerBase
    {
        private readonly ISender sender;

        public TaskCommandController(ISender sender)
        {
            this.sender = sender;
        }

        [HttpPost("accepttask")]
        public async Task<IActionResult> AcceptTaskAsync(string hashValue, Guid? code, CancellationToken cancellationToken)
        {
            var query = new AcceptTaskCommand(hashValue, code);
            var response = await sender.Send(query, cancellationToken);
            return Ok(response);
        }

        [HttpPost("rejecttask")]
        public async Task<IActionResult> RejectTaskAsync(string hashValue, Guid? code, CancellationToken cancellationToken)
        {
            var query = new RejectTaskCommand(hashValue, code);
            var response = await sender.Send(query, cancellationToken);
            return Ok(response);
        }

        [HttpPost("unregistertask")]
        public async Task<IActionResult> UnregisterTaskAsync(Guid taskAssignmentId, CancellationToken cancellationToken)
        {
            var query = new UnregisterTaskCommand(taskAssignmentId);
            var response = await sender.Send(query, cancellationToken);
            return Ok(response);
        }
    }
}
