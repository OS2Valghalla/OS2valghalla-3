using MediatR;
using Microsoft.AspNetCore.Mvc;
using Valghalla.Integration.Auth;
using Valghalla.Internal.API.Requests.Administration.SpecialDiet;
using Valghalla.Internal.Application.Abstractions.Messaging;
using Valghalla.Internal.Application.Modules.Administration.SpecialDiet.Commands;

namespace Valghalla.Internal.API.Controllers.Administration.SpecialDiet
{
    [ApiController]
    [Route("api/administration/specialdiet")]
    [UserAuthorize(RoleEnum.Administrator)]
    public class SpecialDietCommandController : ControllerBase
    {
        private readonly ISender sender;

        public SpecialDietCommandController(ISender sender)
        {
            this.sender = sender;
        }

        [HttpPost("createspecialdiet")]
        public async Task<IActionResult> CreateSpecialDietAsync([FromBody] CreateSpecialDietRequest request, CancellationToken cancellationToken)
        {
            var command = new CreateSpecialDietCommand(request.Title);
            var id = await sender.Send(command, cancellationToken);
            return Ok(id);
        }

        [HttpPut("updatespecialdiet")]
        public async Task<IActionResult> UpdateSpecialDietAsync([FromBody] UpdateSpecialDietRequest request, CancellationToken cancellationToken)
        {
            var command = new UpdateSpecialDietCommand(request.Id, request.Title);
            var result = await sender.Send(command, cancellationToken);
            return Ok(result);
        }

        [HttpDelete("deletespecialdiet")]
        public async Task<IActionResult> DeleteSpecialDietAsync(Guid id, CancellationToken cancellationToken)
        {
            var command = new DeleteSpecialDietCommand(id).Apply(HttpContext);
            var response = await sender.Send(command, cancellationToken);
            return Ok(response);
        }
    }
}
