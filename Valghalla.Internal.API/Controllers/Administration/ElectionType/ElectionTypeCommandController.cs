using MediatR;
using Microsoft.AspNetCore.Mvc;
using Valghalla.Integration.Auth;
using Valghalla.Internal.API.Requests.Administration.ElectionType;
using Valghalla.Internal.Application.Abstractions.Messaging;
using Valghalla.Internal.Application.Modules.Administration.ElectionType.Commands;

namespace Valghalla.Internal.API.Controllers.Administration.ElectionType
{

    [ApiController]
    [Route("api/administration/electiontype")]
    [UserAuthorize(RoleEnum.Administrator)]
    public class ElectionTypeCommandController : ControllerBase
    {
        private readonly ISender sender;

        public ElectionTypeCommandController(ISender sender)
        {
            this.sender = sender;
        }

        [HttpPost("createelectiontype")]
        public async Task<IActionResult> CreateElectionTypeAsync([FromBody] CreateElectionTypeRequest request, CancellationToken cancellationToken)
        {
            var command = new CreateElectionTypeCommand(request.Title, request.ValidationRuleIds);
            var id = await sender.Send(command, cancellationToken);
            return Ok(id);
        }

        [HttpPut("updateelectiontype")]
        public async Task<IActionResult> UpdateElectionTypeAsync([FromBody] UpdateElectionTypeRequest request, CancellationToken cancellationToken)
        {
            var command = new UpdateElectionTypeCommand(request.Id, request.Title, request.ValidationRuleIds);
            var result = await sender.Send(command, cancellationToken);
            return Ok(result);
        }

        [HttpDelete("deleteelectiontype")]
        public async Task<IActionResult> DeleteElectionTypeAsync(Guid id, CancellationToken cancellationToken)
        {
            var command = new DeleteElectionTypeCommand(id).Apply(HttpContext);
            var result = await sender.Send(command, cancellationToken);
            return Ok(result);
        }

    }
}
