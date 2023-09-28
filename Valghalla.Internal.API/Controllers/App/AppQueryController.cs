using MediatR;
using Microsoft.AspNetCore.Mvc;
using Valghalla.Integration.Auth;
using Valghalla.Internal.Application.Modules.App.Queries;

namespace Valghalla.Internal.API.Controllers.App
{
    [ApiController]
    [Route("api/app")]
    [UserAuthorize(RoleEnum.Administrator)]
    public class AppQueryController : ControllerBase
    {
        private readonly ISender sender;

        public AppQueryController(ISender sender)
        {
            this.sender = sender;
        }

        [HttpGet("context")]
        public async Task<IActionResult> GetAppContextAsync(Guid? electionId, CancellationToken cancellationToken)
        {
            var query = new GetAppContextQuery(electionId);
            var response = await sender.Send(query, cancellationToken);
            return Ok(response);
        }
    }
}
