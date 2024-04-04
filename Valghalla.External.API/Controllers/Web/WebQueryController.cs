using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Valghalla.External.Application.Modules.Web.Queries;

namespace Valghalla.External.API.Controllers.Web
{
    [ApiController]
    [Route("api/web")]
    [AllowAnonymous]
    public class WebQueryController : ControllerBase
    {
        private readonly ISender sender;

        public WebQueryController(ISender sender)
        {
            this.sender = sender;
        }

        [HttpGet("disclosurestatement")]
        public async Task<IActionResult> GetDisclosureStatementPageAsync(CancellationToken cancellationToken)
        {
            var query = new GetDisclosureStatementPageQuery();
            var response = await sender.Send(query, cancellationToken);
            return Ok(response);
        }

        [HttpGet("declarationofconsent")]
        public async Task<IActionResult> GetDeclarationOfConsentPageAsync(CancellationToken cancellationToken)
        {
            var query = new GetDeclarationOfConsentPageQuery();
            var response = await sender.Send(query, cancellationToken);
            return Ok(response);
        }
    }
}
