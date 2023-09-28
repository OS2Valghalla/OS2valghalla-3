using MediatR;
using Microsoft.AspNetCore.Mvc;
using Valghalla.External.Application.Modules.WorkLocation.Queries;
using Valghalla.Integration.Auth;

namespace Valghalla.External.API.Controllers.WorkLocation
{
    [ApiController]
    [Route("api/worklocation")]
    [UserAuthorize(RoleEnum.WorkLocationResponsible)]
    public class WorkLocationQueryController : ControllerBase
    {
        private readonly ISender sender;

        public WorkLocationQueryController(ISender sender)
        {
            this.sender = sender;
        }

        [HttpGet("getmyworklocations")]
        public async Task<IActionResult> GetMyWorkLocationsAsync(CancellationToken cancellationToken)
        {
            var query = new GetMyWorkLocationsQuery();
            var response = await sender.Send(query, cancellationToken);
            return Ok(response);
        }

        [HttpGet("getworklocationdates")]
        public async Task<IActionResult> GetWorkLocationDatesAsync(Guid workLocationId, CancellationToken cancellationToken)
        {
            var query = new GetWorkLocationDatesQuery(workLocationId);
            var response = await sender.Send(query, cancellationToken);
            return Ok(response);
        }

        [HttpGet("getworklocationdetails")]
        public async Task<IActionResult> GetWorkLocationDetailsAsync(Guid workLocationId, DateTime taskDate, CancellationToken cancellationToken)
        {
            var query = new GetWorkLocationDetailsQuery(workLocationId, taskDate);
            var response = await sender.Send(query, cancellationToken);
            return Ok(response);
        }
    }
}
