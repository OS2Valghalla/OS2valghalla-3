using MediatR;
using Microsoft.AspNetCore.Mvc;
using Valghalla.Integration.Auth;
using Valghalla.Internal.Application.Modules.Shared.WorkLocation.Queries;

namespace Valghalla.Internal.API.Controllers.Shared.WorkLocation
{
    [ApiController]
    [Route("api/shared/worklocation")]
    [UserAuthorize(RoleEnum.Administrator)]
    public class WorkLocationSharedQueryController : ControllerBase
    {
        private readonly ISender sender;

        public WorkLocationSharedQueryController(ISender sender)
        {
            this.sender = sender;
        }

        [HttpGet("getworklocations")]
        public async Task<IActionResult> GetWorkLocationsAsync(CancellationToken cancellationToken)
        {
            var query = new GetWorkLocationsSharedQuery();
            var result = await sender.Send(query, cancellationToken);
            return Ok(result);
        }

        [HttpGet("getworklocation")]
        public async Task<IActionResult> GetWorkLocationAsync(Guid workLocationId, Guid? electionId, CancellationToken cancellationToken)
        {
            var query = new GetWorkLocationSharedQuery(workLocationId, electionId);
            var result = await sender.Send(query, cancellationToken);
            return Ok(result);
        }
    }
}
