using MediatR;
using Microsoft.AspNetCore.Mvc;
using Valghalla.Integration.Auth;
using Valghalla.Internal.Application.Modules.Administration.Web.Queries;

namespace Valghalla.Internal.API.Controllers.Administration.Web
{
    [ApiController]
    [Route("api/administration/web")]
    [UserAuthorize(RoleEnum.Administrator)]
    public class WebQueryController: ControllerBase
    {
        private readonly ISender sender;

        public WebQueryController(ISender sender)
        {
            this.sender = sender;
        }

        [HttpGet("front")]
        public async Task<IActionResult> GetFrontPageAsync(CancellationToken cancellationToken)
        {
            var command = new GetFrontPageQuery();
            var response = await sender.Send(command, cancellationToken);

            return Ok(response);
        }

        [HttpGet("faq")]
        public async Task<IActionResult> GetFAQPageAsync(CancellationToken cancellationToken)
        {
            var command = new GetFAQPageQuery();
            var response = await sender.Send(command, cancellationToken);

            return Ok(response);
        }

        [HttpGet("disclosurestatement")]
        public async Task<IActionResult> GetDisclosureStatementPageAsync(CancellationToken cancellationToken)
        {
            var command = new GetDisclosureStatementPageQuery();
            var response = await sender.Send(command, cancellationToken);

            return Ok(response);
        }

        [HttpGet("declarationofconsent")]
        public async Task<IActionResult> GetDeclarationOfConsentPageAsync(CancellationToken cancellationToken)
        {
            var command = new GetDeclarationOfConsentPageQuery();
            var response = await sender.Send(command, cancellationToken);

            return Ok(response);
        }

        [HttpGet("contactinformation")]
        public async Task<IActionResult> GetElectionCommitteeContactInformationAsync(CancellationToken cancellationToken)
        {
            var command = new GetElectionCommitteeContactInformationQuery();
            var response = await sender.Send(command, cancellationToken);

            return Ok(response);
        }
    }
}
