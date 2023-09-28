using MediatR;
using Microsoft.AspNetCore.Mvc;
using Valghalla.Integration.Auth;
using Valghalla.Internal.Application.Modules.Shared.ElectionType.Queries;

namespace Valghalla.Internal.API.Controllers.Shared.ElectionType
{
    [ApiController]
    [Route("api/shared/electiontype")]
    [UserAuthorize(RoleEnum.Administrator)]
    public class ElectionTypeSharedQueryController : ControllerBase
    {
        private readonly ISender sender;

        public ElectionTypeSharedQueryController(ISender sender)
        {
            this.sender = sender;
        }

        [HttpGet("getelectiontypes")]
        public async Task<IActionResult> GetElectionTypesAsync(CancellationToken cancellationToken)
        {
            var query = new GetElectionTypesSharedQuery();
            var result = await sender.Send(query, cancellationToken);
            return Ok(result);
        }
    }
}
