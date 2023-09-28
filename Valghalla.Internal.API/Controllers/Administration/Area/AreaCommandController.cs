using MediatR;
using Microsoft.AspNetCore.Mvc;
using Valghalla.Integration.Auth;
using Valghalla.Internal.API.Requests.Administration.Area;
using Valghalla.Internal.Application.Abstractions.Messaging;
using Valghalla.Internal.Application.Modules.Administration.Area.Commands;

namespace Valghalla.Internal.API.Controllers.Administration.Area
{
    [ApiController]
    [Route("api/administration/area")]
    [UserAuthorize(RoleEnum.Administrator)]
    public class AreaCommandController : ControllerBase
    {
        private readonly ISender sender;

        public AreaCommandController(ISender sender)
        {
            this.sender = sender;
        }

        [HttpPost("createarea")]
        public async Task<IActionResult> CreateAreaAsync([FromBody] CreateAreaRequest request, CancellationToken cancellationToken)
        {
            var command = new CreateAreaCommand(request.Name, request.Description);
            var id = await sender.Send(command, cancellationToken);
            return Ok(id);
        }

        [HttpPut("updatearea")]
        public async Task<IActionResult> UpdateAreaAsync([FromBody] UpdateAreaRequest request, CancellationToken cancellationToken)
        {
            var command = new UpdateAreaCommand(request.Id, request.Name, request.Description);
            var result = await sender.Send(command, cancellationToken);
            return Ok(result);
        }

        [HttpDelete("deletearea")]
        public async Task<IActionResult> DeleteAreaAsync(Guid id, CancellationToken cancellationToken)
        {
            var command = new DeleteAreaCommand(id).Apply(HttpContext);
            var response = await sender.Send(command, cancellationToken);
            return Ok(response);
        }
    }
}
