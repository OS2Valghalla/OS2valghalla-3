using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Valghalla.External.Application.Modules.Shared.SpecialDiet.Queries;

namespace Valghalla.External.API.Controllers.Shared.SpecialDiet
{
    [ApiController]
    [Route("api/shared/specialdiet")]
    public class SpecialDietSharedQueryController : ControllerBase
    {
        private readonly ISender sender;

        public SpecialDietSharedQueryController(ISender sender)
        {
            this.sender = sender;
        }

        [HttpGet("getspecialdiets")]
        [AllowAnonymous]
        public async Task<IActionResult> GetSpecialDietsAsync(CancellationToken cancellationToken)
        {
            var query = new GetSpecialDietsSharedQuery();
            var result = await sender.Send(query, cancellationToken);
            return Ok(result);
        }
    }
}
