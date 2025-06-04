using MediatR;
using Microsoft.AspNetCore.Mvc;
using Valghalla.Integration.Auth;
using Valghalla.Internal.Application.Modules.Shared.WorkLocationTemplate.Queries;

namespace Valghalla.Internal.API.Controllers.Shared.WorkLocationTemplate
{
    [ApiController]
    [Route("api/shared/worklocationtemplate")]
    [UserAuthorize(RoleEnum.Administrator)]
    public class WorkLocationTemplateSharedQueryController : ControllerBase
    {
        private readonly ISender sender;

        public WorkLocationTemplateSharedQueryController(ISender sender)
        {
            this.sender = sender;
        }

        [HttpGet("getworklocationtemplates")]
        public async Task<IActionResult> GetWorkLocationTemplatesAsync(CancellationToken cancellationToken)
        {
            var query = new GetWorkLocationTemplatesSharedQuery();
            var result = await sender.Send(query, cancellationToken);
            return Ok(result);
        }

        [HttpGet("getworklocationtemplate")]
        public async Task<IActionResult> GetWorkLocationTemplateAsync(Guid workLocationId, Guid? electionId, CancellationToken cancellationToken)
        {
            var query = new GetWorkLocationTemplateSharedQuery(workLocationId, electionId);
            var result = await sender.Send(query, cancellationToken);
            return Ok(result);
        }
    }
}
