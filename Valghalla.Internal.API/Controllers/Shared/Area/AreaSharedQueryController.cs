using MediatR;
using Microsoft.AspNetCore.Mvc;
using Valghalla.Integration.Auth;
using Valghalla.Internal.Application.Modules.Shared.Area.Queries;

namespace Valghalla.Internal.API.Controllers.Shared.Area
{

    [ApiController]
    [Route("api/shared/area")]
    [UserAuthorize(RoleEnum.Administrator)]
    public class AreaSharedQueryController : ControllerBase
    {
        private readonly ISender sender;

        public AreaSharedQueryController(ISender sender)
        {
            this.sender = sender;
        }

        [HttpGet("getareas")]
        public async Task<IActionResult> GetAreasAsync(CancellationToken cancellationToken)
        {
            var query = new GetAreasSharedQuery();
            var result = await sender.Send(query, cancellationToken);
            return Ok(result);
        }
    }
}
