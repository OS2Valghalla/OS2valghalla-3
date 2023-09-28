using MediatR;
using Microsoft.AspNetCore.Mvc;
using Valghalla.Integration.Auth;
using Valghalla.Internal.Application.Modules.Communication.CommunicationLog.Queries;

namespace Valghalla.Internal.API.Controllers.Communication.CommunicationLog
{
    [ApiController]
    [Route("api/communication/log")]
    [UserAuthorize(RoleEnum.Administrator)]
    public class CommunicationLogQueryController : ControllerBase
    {
        private readonly ISender sender;

        public CommunicationLogQueryController(ISender sender)
        {
            this.sender = sender;
        }

        [HttpPost("queryloglisting")]
        public async Task<IActionResult> QueryCommunicationLogListingAsync(CommunicationLogListingQueryForm form, CancellationToken cancellationToken)
        {
            var response = await sender.Send(form, cancellationToken);
            return Ok(response);
        }

        [HttpPost("getloglistingqueryform")]
        public async Task<IActionResult> GetCommunicationLogListingQueryFormAsync(CommunicationLogListingQueryFormParameters parameter, CancellationToken cancellationToken)
        {
            var response = await sender.Send(parameter, cancellationToken);
            return Ok(response);
        }

        [HttpGet("getlogdetails")]
        public async Task<IActionResult> GetCommunicationLogDetailsAsync(Guid id, CancellationToken cancellationToken)
        {
            var query = new GetCommunicationLogDetailsQuery(id);
            var response = await sender.Send(query, cancellationToken);
            return Ok(response);
        }
    }
}
