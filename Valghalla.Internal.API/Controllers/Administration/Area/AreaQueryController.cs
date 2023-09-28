using MediatR;
using Microsoft.AspNetCore.Mvc;
using Valghalla.Application.QueryEngine;
using Valghalla.Integration.Auth;
using Valghalla.Internal.Application.Modules.Administration.Area.Queries;

namespace Valghalla.Internal.API.Controllers.Administration.Area
{
    [ApiController]
    [Route("api/administration/area")]
    [UserAuthorize(RoleEnum.Administrator)]
    public class AreaQueryController : ControllerBase
    {
        private readonly ISender sender;

        public AreaQueryController(ISender sender)
        {
            this.sender = sender;
        }

        [HttpPost("queryarealisting")]
        public async Task<IActionResult> QueryAreaListingAsync(AreaListingQueryForm form, CancellationToken cancellationToken)
        {
            var response = await sender.Send(form, cancellationToken);
            return Ok(response);
        }

        [HttpPost("getarealistingqueryform")]
        public async Task<IActionResult> GetAreaListingQueryFormAsync(VoidQueryFormParameters form, CancellationToken cancellationToken)
        {
            var response = await sender.Send(form, cancellationToken);
            return Ok(response);
        }

        [HttpGet("getareadetails")]
        public async Task<IActionResult> GetAreaDetailsAsync(Guid id, CancellationToken cancellationToken)
        {
            var query = new GetAreaDetailsQuery(id);
            var response = await sender.Send(query, cancellationToken);
            return Ok(response);
        }

        [HttpGet("getallareas")]
        public async Task<IActionResult> GetAllAreasAsync(CancellationToken cancellationToken)
        {
            var query = new GetAllAreasQuery();
            var result = await sender.Send(query, cancellationToken);

            return Ok(result);
        }
    }
}
