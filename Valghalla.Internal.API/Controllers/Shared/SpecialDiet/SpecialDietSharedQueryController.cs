using MediatR;
using Microsoft.AspNetCore.Mvc;
using Valghalla.Integration.Auth;
using Valghalla.Internal.Application.Modules.Shared.SpecialDiet.Queries;

namespace Valghalla.Internal.API.Controllers.Shared.SpecialDiet
{
    [ApiController]
    [Route("api/shared/specialdiet")]
    [UserAuthorize(RoleEnum.Administrator)]
    public class SpecialDietSharedQueryController : ControllerBase
    {
        private readonly ISender sender;

        public SpecialDietSharedQueryController(ISender sender)
        {
            this.sender = sender;
        }

        [HttpGet("getspecialdiets")]
        public async Task<IActionResult> GetSpecialDietsAsync(CancellationToken cancellationToken)
        {
            var query = new GetSpecialDietsSharedQuery();
            var result = await sender.Send(query, cancellationToken);
            return Ok(result);
        }
    }
}
