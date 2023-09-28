using MediatR;
using Microsoft.AspNetCore.Mvc;
using Valghalla.Integration.Auth;
using Valghalla.Internal.Application.Modules.Shared.Communication.Queries;

namespace Valghalla.Internal.API.Controllers.Shared.Communication
{
    [ApiController]
    [Route("api/shared/communication")]
    [UserAuthorize(RoleEnum.Administrator)]
    public class CommunicationSharedQueryController: ControllerBase
    {
        private readonly ISender sender;

        public CommunicationSharedQueryController(ISender sender)
        {
            this.sender = sender;
        }

        [HttpGet("getcommunicationtemplates")]
        public async Task<IActionResult> GetCommunicationTemplatesAsync(CancellationToken cancellationToken)
        {
            var query = new GetCommunicationTemplatesSharedQuery();
            var result = await sender.Send(query, cancellationToken);
            return Ok(result);
        }
    }
}
