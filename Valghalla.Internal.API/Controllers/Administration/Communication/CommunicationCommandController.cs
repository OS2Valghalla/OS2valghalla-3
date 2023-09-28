using MediatR;
using Microsoft.AspNetCore.Mvc;
using Valghalla.Integration.Auth;
using Valghalla.Internal.API.Requests.Administration.Communication;
using Valghalla.Internal.Application.Abstractions.Messaging;
using Valghalla.Internal.Application.Modules.Administration.Communication.Commands;
using Valghalla.Internal.Application.Modules.Administration.Communication.Queries;

namespace Valghalla.Internal.API.Controllers.Administration.Communication
{
    [ApiController]
    [Route("api/administration/communication")]
    [UserAuthorize(RoleEnum.Administrator)]
    public class CommunicationCommandController: ControllerBase
    {
        private readonly ISender sender;

        public CommunicationCommandController(ISender sender)
        {
            this.sender = sender;
        }

        [HttpPost("createcommunicationtemplate")]
        public async Task<IActionResult> CreateCommunicationTemplateAsync([FromBody] CreateCommunicationTemplateRequest request, CancellationToken cancellationToken)
        {
            var command = new CreateCommunicationTemplateCommand
            {
                Title = request.Title,
                Subject = request.Subject,
                Content = request.Content,
                TemplateType = request.TemplateType,
                FileReferenceIds = request.FileReferenceIds 
            };
            var id = await sender.Send(command, cancellationToken);
            return Ok(id);
        }

        [HttpPut("updatecommunicationtemplate")]
        public async Task<IActionResult> UpdateCommunicationTemplateAsync([FromBody] UpdateCommunicationTemplateRequest request, CancellationToken cancellationToken)
        {
            var command = new UpdateCommunicationTemplateCommand()
            {
                Id = request.Id,
                Title = request.Title,
                Subject = request.Subject,
                Content = request.Content,
                TemplateType = request.TemplateType,
                FileReferenceIds = request.FileReferenceIds,
            };

            var response = await sender.Send(command, cancellationToken);
            return Ok(response);
        }

        [HttpDelete("deletecommunicationtemplate")]
        public async Task<IActionResult> DeleteCommunicationTemplateAsync(Guid id, CancellationToken cancellationToken)
        {
            var command = new DeleteCommunicationTemplateCommand(id).Apply(HttpContext);
            var response = await sender.Send(command, cancellationToken);
            return Ok(response);
        }

        [HttpPost("sendgroupmessage")]
        public async Task<IActionResult> SendGroupMessageAsync([FromBody] SendGroupMessageRequest request, CancellationToken cancellationToken)
        {
            var command = new SendGroupMessageCommand()
            {
                ElectionId = request.ElectionId,
                Subject = request.Subject,
                Content = request.Content,
                TemplateType = request.TemplateType,
                FileReferenceIds = request.FileReferenceIds,
                WorkLocationIds = request.WorkLocationIds,
                TeamIds = request.TeamIds,
                TaskTypeIds = request.TaskTypeIds,
                TaskStatuses = request.TaskStatuses,
                TaskDates = request.TaskDates
            };

            var response = await sender.Send(command, cancellationToken);
            return Ok(response);
        }
    }
}
