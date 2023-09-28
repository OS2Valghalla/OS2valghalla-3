using MediatR;
using Microsoft.AspNetCore.Mvc;
using Valghalla.External.Application.Modules.Unprotected.Queries;

namespace Valghalla.External.API.Controllers.Unprotected
{
    [ApiController]
    [Route("api/unprotected/web")]
    public class UnprotectedWebQueryController : ControllerBase
    {
        private readonly ISender sender;

        public UnprotectedWebQueryController(ISender sender)
        {
            this.sender = sender;
        }

        [HttpGet("front")]
        public async Task<IActionResult> GetFrontPageAsync(CancellationToken cancellationToken)
        {
            var query = new GetFrontPageQuery();
            var result = await sender.Send(query, cancellationToken);
            return Ok(result);
        }

        [HttpGet("faq")]
        public async Task<IActionResult> GetFAQPageAsync(CancellationToken cancellationToken)
        {
            var query = new GetFAQPageQuery();
            var result = await sender.Send(query, cancellationToken);
            return Ok(result);
        }
    }
}
