using MediatR;

using Microsoft.AspNetCore.Mvc;

using Valghalla.Integration.Auth;
using Valghalla.Internal.API.Requests.Administration.TaskType;
using Valghalla.Internal.Application.Abstractions.Messaging;
using Valghalla.Internal.Application.Modules.Administration.TaskType.Commands;

namespace Valghalla.Internal.API.Controllers.Administration.TaskType
{
    [ApiController]
    [Route("api/administration/tasktype")]
    [UserAuthorize(RoleEnum.Administrator)]
    public class TaskTypeCommandController : ControllerBase
    {
        private readonly ISender sender;

        public TaskTypeCommandController(ISender sender)
        {
            this.sender = sender;
        }

        [HttpPost("createtasktype")]
        public async Task<IActionResult> CreateTaskTypeAsync([FromBody] CreateTaskTypeRequest request, CancellationToken cancellationToken)
        {
            var command = new CreateTaskTypeCommand()
            {
                Title = request.Title,
                ShortName = request.ShortName,
                Description = request.Description,
                StartTime = request.StartTime,
                EndTime = request.EndTime,
                Payment = request.Payment,
                ValidationNotRequired = request.ValidationNotRequired,
                Trusted = request.Trusted,
                SendingReminderEnabled = request.SendingReminderEnabled,
                FileReferenceIds = request.FileReferenceIds,
            };

            var id = await sender.Send(command, cancellationToken);
            return Ok(id);
        }

        [HttpDelete("deletetasktype")]
        public async Task<IActionResult> DeleteTaskTypeAsync(Guid id, CancellationToken cancellationToken)
        {
            var command = new DeleteTaskTypeCommand(id).Apply(HttpContext);
            var response = await sender.Send(command, cancellationToken);
            return Ok(response);
        }

        [HttpPut("updatetasktype")]
        public async Task<IActionResult> UpdateTaskTypeAsync([FromBody] UpdateTaskTypeRequest request, CancellationToken cancellationToken)
        {
            var command = new UpdateTaskTypeCommand()
            {
                Id = request.Id,
                Title = request.Title,
                ShortName = request.ShortName,
                Description = request.Description,
                StartTime = request.StartTime,
                EndTime = request.EndTime,
                Payment = request.Payment,
                ValidationNotRequired = request.ValidationNotRequired,
                Trusted = request.Trusted,
                SendingReminderEnabled = request.SendingReminderEnabled,
                FileReferenceIds = request.FileReferenceIds,
            };

            command.Apply(HttpContext);

            var response = await sender.Send(command, cancellationToken);
            return Ok(response);
        }
    }
}
