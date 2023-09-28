using MediatR;
using Microsoft.AspNetCore.Mvc;
using Valghalla.Integration.Auth;
using Valghalla.Internal.Application.Modules.Administration.SpecialDiet.Queries;

namespace Valghalla.Internal.API.Controllers.Administration.SpecialDiet
{
    [ApiController]
    [Route("api/administration/specialdiet")]
    [UserAuthorize(RoleEnum.Administrator)]
    public class SpecialDietQueryController : ControllerBase
    {
        private readonly ISender sender;

        public SpecialDietQueryController(ISender sender)
        {
            this.sender = sender;
        }

        [HttpGet("getspecialdiet")]
        public async Task<IActionResult> GetSpecialDietAsync(Guid specialDietId, CancellationToken cancellationToken)
        {
            var query = new GetSpecialDietQuery(specialDietId);
            var result = await sender.Send(query, cancellationToken);

            return Ok(result);
        }

        [HttpPost("queryspecialdietlisting")]
        public async Task<IActionResult> QuerySpecialDietListingAsync(SpecialDietListingQueryForm form, CancellationToken cancellationToken)
        {
            var result = await sender.Send(form, cancellationToken);
            return Ok(result);
        }

        [HttpPost("getspecialdietlistingqueryform")]
        public async Task<IActionResult> GetSpecialDietListingQueryForm(SpecialDietListingQueryFormParameters @params, CancellationToken cancellationToken)
        {
            var result = await sender.Send(@params, cancellationToken);
            return Ok(result);
        }
    }
}
