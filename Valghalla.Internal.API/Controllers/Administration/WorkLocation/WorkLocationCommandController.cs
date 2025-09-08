using MediatR;

using Microsoft.AspNetCore.Mvc;

using Valghalla.Integration.Auth;
using Valghalla.Internal.API.Requests.Administration.WorkLocation;
using Valghalla.Internal.Application.Abstractions.Messaging;
using Valghalla.Internal.Application.Modules.Administration.WorkLocation.Commands;

namespace Valghalla.Internal.API.Controllers.Administration.WorkLocation
{
    [ApiController]
    [Route("api/administration/worklocation")]
    [UserAuthorize(RoleEnum.Administrator)]
    public class WorkLocationCommandController : ControllerBase
    {
        private readonly ISender sender;

        public WorkLocationCommandController(ISender sender)
        {
            this.sender = sender;
        }

        [HttpPost("createworklocation")]
        public async Task<IActionResult> CreateWorkLocationAsync([FromBody] CreateWorkLocationRequest request, CancellationToken cancellationToken)
        {
            var command = new CreateWorkLocationCommand()
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

        [HttpPut("updateworklocation")]
        public async Task<IActionResult> UpdateWorkLocationAsync([FromBody] UpdateWorkLocationRequest request, CancellationToken cancellationToken)
        {
            var command = new UpdateWorkLocationCommand()
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

        [HttpDelete("deleteworklocation")]
        public async Task<IActionResult> DeleteWorkLocationAsync(Guid id, CancellationToken cancellationToken)
        {
            var command = new DeleteWorkLocationCommand(id).Apply(HttpContext);
            var response = await sender.Send(command, cancellationToken);
            return Ok(response);
        }
    }
}
