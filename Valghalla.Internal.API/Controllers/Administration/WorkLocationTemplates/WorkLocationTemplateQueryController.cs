using MediatR;

using Microsoft.AspNetCore.Mvc;

using Valghalla.Integration.Auth;
using Valghalla.Internal.Application.Modules.Administration.Election.Queries;
using Valghalla.Internal.Application.Modules.Administration.WorkLocationTemplate.Queries;

namespace Valghalla.Internal.API.Controllers.Administration.WorkLocationTemplate
{
    [ApiController]
    [Route("api/administration/worklocationtemplate")]
    [UserAuthorize(RoleEnum.Administrator)]
    public class WorkLocationTemplateQueryController : ControllerBase
    {
        private readonly ISender sender;

        public WorkLocationTemplateQueryController(ISender sender)
        {
            this.sender = sender;
        }

        [HttpGet("getWorkLocationTemplate")]
        public async Task<IActionResult> GetWorkLocationTemplateAsync(Guid WorkLocationTemplateId, CancellationToken cancellationToken)
        {
            var query = new GetWorkLocationTemplateQuery( WorkLocationTemplateId);
            var result = await sender.Send(query, cancellationToken);
            return Ok(result);
        }
        [HttpGet("getWorkLocationTemplates")]
        public async Task<IActionResult> GetWorkLocationTemplatesAsync(CancellationToken cancellationToken)
        {
            var query = new GetWorkLocationTemplatesQuery();
            var result = await sender.Send(query, cancellationToken);
            return Ok(result);
        }

        [HttpPost("queryWorkLocationTemplatelisting")]
        public async Task<IActionResult> QueryWorkLocationTemplateListingAsync(WorkLocationTemplateListingQueryForm query, CancellationToken cancellationToken)
        {
            var result = await sender.Send(query, cancellationToken);
            return Ok(result);
        }

        [HttpPost("getWorkLocationTemplatelistingqueryform")]
        public async Task<IActionResult> GetWorkLocationTemplateListingQueryForm(WorkLocationTemplateListingQueryFormParameters @param, CancellationToken cancellationToken)
        {
            var result = await sender.Send(@param, cancellationToken);
            return Ok(result);
        }
    }
}
