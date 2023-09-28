using MediatR;
using Microsoft.AspNetCore.Mvc;
using Valghalla.Integration.Auth;
using Valghalla.Internal.Application.Modules.App.Queries;

namespace Valghalla.Internal.API.Controllers.App
{
    [ApiController]
    [Route("api/app")]
    [UserAuthorize(RoleEnum.Administrator)]
    public class AppElectionQueryController : ControllerBase
    {
        private readonly ISender sender;

        public AppElectionQueryController(ISender sender)
        {
            this.sender = sender;
        }

        [HttpGet("getelectionstoworkon")]
        public async Task<IActionResult> GetElectionsToWorkOnAsync(CancellationToken cancellationToken)
        {
            var query = new GetElectionsToWorkOnQuery();
            var result = await sender.Send(query, cancellationToken);

            return Ok(result);
        }
    }
}
