using MediatR;
using Microsoft.AspNetCore.Mvc;
using Valghalla.Application.Abstractions.Messaging;
using Valghalla.Internal.Application.Modules.App.Queries;

namespace Valghalla.Internal.API.Controllers.App
{
    [ApiController]
    [Route("_css")]
    public class AppCssController : ControllerBase
    {
        private readonly ISender sender;

        public AppCssController(ISender sender)
        {
            this.sender = sender;
        }

        [HttpGet("valghalla.css")]
        public async Task<IActionResult> GetCss(CancellationToken cancellationToken)
        {
            var query = new AppCssQuery();
            var result = await sender.Send(query, cancellationToken);

            if (!result.IsSuccess)
            {
                return NotFound(result.Message);
            }

            if (result is not Response<string> cssResponse)
            {
                return BadRequest();
            }

            return Content(cssResponse.Data, "text/css");
        }
    }
}
