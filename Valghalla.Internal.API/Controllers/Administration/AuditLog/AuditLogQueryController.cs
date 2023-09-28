using MediatR;
using Microsoft.AspNetCore.Mvc;
using Valghalla.Integration.Auth;
using Valghalla.Internal.Application.Modules.Administration.AuditLog.Queries;

namespace Valghalla.Internal.API.Controllers.Administration.AuditLog
{
    [ApiController]
    [Route("api/administration/auditlog")]
    [UserAuthorize(RoleEnum.Administrator)]
    public class AuditLogQueryController : ControllerBase
    {
        private readonly ISender sender;

        public AuditLogQueryController(ISender sender)
        {
            this.sender = sender;
        }

        [HttpPost("queryauditloglisting")]
        public async Task<IActionResult> QueryAuditLogListingAsync(AuditLogListingQueryForm query, CancellationToken cancellationToken)
        {
            var result = await sender.Send(query, cancellationToken);
            return Ok(result);
        }

        [HttpPost("getauditloglistingqueryform")]
        public async Task<IActionResult> GetAuditLogListingQueryFormAsync(AuditLogListingQueryFormParameters query, CancellationToken cancellationToken)
        {
            var result = await sender.Send(query, cancellationToken);
            return Ok(result);
        }
    }
}
