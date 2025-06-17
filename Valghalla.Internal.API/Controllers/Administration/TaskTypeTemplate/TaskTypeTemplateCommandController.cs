using MediatR;

using Microsoft.AspNetCore.Mvc;

using Valghalla.Integration.Auth;
using Valghalla.Internal.API.Requests.Administration.TaskTypeTemplate;
using Valghalla.Internal.Application.Abstractions.Messaging;
using Valghalla.Internal.Application.Modules.Administration.TaskTypeTemplate.Commands;

namespace Valghalla.Internal.API.Controllers.Administration.TaskTypeTemplate
{
    [ApiController]
    [Route("api/administration/TaskTypeTemplate")]
    [UserAuthorize(RoleEnum.Administrator)]
    public class TaskTypeTemplateCommandController : ControllerBase
    {
        private readonly ISender sender;

        public TaskTypeTemplateCommandController(ISender sender)
        {
            this.sender = sender;
        }

        [HttpPost("createTaskTypeTemplate")]
        public async Task<IActionResult> CreateTaskTypeTemplateAsync([FromBody] CreateTaskTypeTemplateRequest request, CancellationToken cancellationToken)
        {
            var command = new CreateTaskTypeTemplateCommand()
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

        [HttpDelete("deleteTaskTypeTemplate")]
        public async Task<IActionResult> DeleteTaskTypeTemplateAsync(Guid id, CancellationToken cancellationToken)
        {
            var command = new DeleteTaskTypeTemplateCommand(id).Apply(HttpContext);
            var response = await sender.Send(command, cancellationToken);
            return Ok(response);
        }

        [HttpPut("updateTaskTypeTemplate")]
        public async Task<IActionResult> UpdateTaskTypeTemplateAsync([FromBody] UpdateTaskTypeTemplateRequest request, CancellationToken cancellationToken)
        {
            var command = new UpdateTaskTypeTemplateCommand()
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
                FileReferenceIds = request.FileReferenceIds
            };

            command.Apply(HttpContext);

            var response = await sender.Send(command, cancellationToken);
            return Ok(response);
        }
    }
}
