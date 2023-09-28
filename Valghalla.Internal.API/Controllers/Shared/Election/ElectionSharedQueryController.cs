using MediatR;
using Microsoft.AspNetCore.Mvc;
using Valghalla.Integration.Auth;
using Valghalla.Internal.Application.Modules.Shared.Election.Queries;

namespace Valghalla.Internal.API.Controllers.Shared.Election
{
    [ApiController]
    [Route("api/shared/election")]
    [UserAuthorize(RoleEnum.Administrator)]
    public class ElectionSharedQueryController : ControllerBase
    {
        private readonly ISender sender;

        public ElectionSharedQueryController(ISender sender)
        {
            this.sender = sender;
        }

        [HttpGet("getelections")]
        public async Task<IActionResult> GetElectionsAsync(CancellationToken cancellationToken)
        {
            var query = new GetElectionsSharedQuery();
            var result = await sender.Send(query, cancellationToken);
            return Ok(result);
        }
    }
}
