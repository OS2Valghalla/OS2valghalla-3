using MediatR;
using Microsoft.AspNetCore.Mvc;
using Valghalla.Integration.Auth;
using Valghalla.Internal.API.Requests.Administration.Web;
using Valghalla.Internal.Application.Modules.Administration.Web.Commands;

namespace Valghalla.Internal.API.Controllers.Administration.Web
{
    [ApiController]
    [Route("api/administration/web")]
    [UserAuthorize(RoleEnum.Administrator)]
    public class WebCommandController : ControllerBase
    {
        private readonly ISender sender;

        public WebCommandController(ISender sender)
        {
            this.sender = sender;
        }


        [HttpPut("front")]
        public async Task<IActionResult> UpdateFrontPageAsync([FromBody] UpdateFrontPageRequest request, CancellationToken cancellationToken)
        {
            var command = new UpdateFrontPageCommand() { PageContent = request.PageContent, Title = request.Title };
            var response = await sender.Send(command, cancellationToken);

            return Ok(response);
        }

        [HttpPut("faq")]
        public async Task<IActionResult> UpdateFAQPageAsync([FromBody] UpdateFAQPageRequest request, CancellationToken cancellationToken)
        {
            var command = new UpdateFAQPageCommand() { PageContent = request.PageContent, IsActivated = request.IsActivated };
            var response = await sender.Send(command, cancellationToken);

            return Ok(response);
        }

        [HttpPut("disclosurestatement")]
        public async Task<IActionResult> UpdateDisclosureStatementPageAsync([FromBody] UpdateDisclosureStatementPageRequest request, CancellationToken cancellationToken)
        {
            var command = new UpdateDisclosureStatementPageCommand() { PageContent = request.PageContent };
            var response = await sender.Send(command, cancellationToken);

            return Ok(response);
        }

        [HttpPut("declarationofconsent")]
        public async Task<IActionResult> UpdateDeclarationOfConsentPageAsync([FromBody] UpdateDeclarationOfConsentPageRequest request, CancellationToken cancellationToken)
        {
            var command = new UpdateDeclarationOfConsentPageCommand() { PageContent = request.PageContent, IsActivated = request.IsActivated };
            var response = await sender.Send(command, cancellationToken);

            return Ok(response);
        }

        [HttpPut("contactinformation")]
        public async Task<IActionResult> UpdateElectionCommitteeContactInformationAsync([FromBody] UpdateElectionCommitteeContactInformationRequest request, CancellationToken cancellationToken)
        {
            var command = new UpdateElectionCommitteeContactInformationPageCommand()
            {
                MunicipalityName = request.MunicipalityName,
                ElectionResponsibleApartment = request.ElectionResponsibleApartment,
                Address = request.Address,
                PostalCode = request.PostalCode,
                City = request.City,
                TelephoneNumber = request.TelephoneNumber,
                DigitalPost = request.DigitalPost,
                Email = request.Email,
                LogoFileReferenceId = request.LogoFileReferenceId
            };
            var response = await sender.Send(command, cancellationToken);

            return Ok(response);
        }
    }
}
