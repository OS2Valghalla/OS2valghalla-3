using MediatR;
using Microsoft.AspNetCore.Mvc;
using Valghalla.Integration.Auth;
using Valghalla.Internal.API.Requests.Administration.WorkLocationTemplate;
using Valghalla.Internal.Application.Abstractions.Messaging;
using Valghalla.Internal.Application.Modules.Administration.WorkLocationTemplate.Commands;

namespace Valghalla.Internal.API.Controllers.Administration.WorkLocationTemplate
{
    [ApiController]
    [Route("api/administration/worklocationtemplate")]
    [UserAuthorize(RoleEnum.Administrator)]
    public class WorkLocationTemplateCommandController : ControllerBase
    {
        private readonly ISender sender;

        public WorkLocationTemplateCommandController(ISender sender)
        {
            this.sender = sender;
        }

        [HttpPost("createworklocationtemplate")]
        public async Task<IActionResult> CreateWorkLocationTemplateAsync([FromBody] CreateWorkLocationTemplateRequest request, CancellationToken cancellationToken)
        {
            var command = new CreateWorkLocationTemplateCommand()
            {
                Title = request.Title,
                AreaId = request.AreaId,
                Address = request.Address,
                PostalCode = request.PostalCode,
                City = request.City,
                VoteLocation = request.VoteLocation,
                TaskTypeIds = request.TaskTypeIds,
                TeamIds = request.TeamIds,
                ResponsibleIds = request.ResponsibleIds
            };

            var id = await sender.Send(command, cancellationToken);
            return Ok(id);
        }

        [HttpPut("updateworklocationtemplate")]
        public async Task<IActionResult> UpdateWorkLocationTemplateAsync([FromBody] UpdateWorkLocationTemplateRequest request, CancellationToken cancellationToken)
        {
            var command = new UpdateWorkLocationTemplateCommand()
            {
                Id = request.Id,
                Title = request.Title,
                AreaId = request.AreaId,
                Address = request.Address,
                PostalCode = request.PostalCode,
                City = request.City,
                VoteLocation = request.VoteLocation,
                TaskTypeIds = request.TaskTypeIds,
                TeamIds = request.TeamIds,
                ResponsibleIds = request.ResponsibleIds
            };

            var result = await sender.Send(command, cancellationToken);
            return Ok(result);
        }

        [HttpDelete("deleteworklocationtemplate")]
        public async Task<IActionResult> DeleteWorkLocationTemplateAsync(Guid id, CancellationToken cancellationToken)
        {
            var command = new DeleteWorkLocationTemplateCommand(id).Apply(HttpContext);
            var response = await sender.Send(command, cancellationToken);
            return Ok(response);
        }
    }
}
