using MediatR;
using Microsoft.AspNetCore.Mvc;
using Valghalla.Integration.Auth;
using Valghalla.Internal.Application.Modules.Administration.WorkLocation.Queries;

namespace Valghalla.Internal.API.Controllers.Administration.WorkLocation
{
    [ApiController]
    [Route("api/administration/worklocation")]
    [UserAuthorize(RoleEnum.Administrator)]
    public class WorkLocationQueryController : ControllerBase
    {
        private readonly ISender sender;

        public WorkLocationQueryController(ISender sender)
        {
            this.sender = sender;
        }

        [HttpGet("getworklocation")]
        public async Task<IActionResult> GetWorkLocationAsync(Guid workLocationId, CancellationToken cancellationToken)
        {
            var query = new GetWorkLocationQuery( workLocationId);
            var result = await sender.Send(query, cancellationToken);
            return Ok(result);
        }

        [HttpPost("queryworklocationlisting")]
        public async Task<IActionResult> QueryWorkLocationListingAsync(WorkLocationListingQueryForm query, CancellationToken cancellationToken)
        {
            var result = await sender.Send(query, cancellationToken);
            return Ok(result);
        }

        [HttpPost("getworklocationlistingqueryform")]
        public async Task<IActionResult> GetWorkLocationListingQueryForm(WorkLocationListingQueryFormParameters @param, CancellationToken cancellationToken)
        {
            var result = await sender.Send(@param, cancellationToken);
            return Ok(result);
        }
    }
}
