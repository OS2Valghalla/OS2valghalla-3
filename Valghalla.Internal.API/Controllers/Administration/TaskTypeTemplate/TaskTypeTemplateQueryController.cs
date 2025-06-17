using MediatR;
using Microsoft.AspNetCore.Mvc;
using Valghalla.Application.QueryEngine;
using Valghalla.Integration.Auth;
using Valghalla.Internal.Application.Modules.Administration.TaskTypeTemplate.Queries;

namespace Valghalla.Internal.API.Controllers.Administration.TaskTypeTemplate
{
    [ApiController]
    [Route("api/administration/TaskTypeTemplate")]
    [UserAuthorize(RoleEnum.Administrator)]
    public class TaskTypeTemplateQueryController : ControllerBase
    {
        private readonly ISender sender;

        public TaskTypeTemplateQueryController(ISender sender)
        {
            this.sender = sender;
        }

        [HttpGet("getTaskTypeTemplatedetails")]
        public async Task<IActionResult> GetTaskTypeTemplateDetailsAsync(Guid id, CancellationToken cancellationToken)
        {
            var query = new GetTaskTypeTemplateDetailsQuery(id);
            var result = await sender.Send(query, cancellationToken);

            return Ok(result);
        }

        [HttpPost("queryTaskTypeTemplatelisting")]
        public async Task<IActionResult> QueryTaskTypeTemplateListingAsync(TaskTypeTemplateListingQueryForm form, CancellationToken cancellationToken)
        {
            var response = await sender.Send(form, cancellationToken);
            return Ok(response);
        }

        [HttpPost("getTaskTypeTemplatelistingqueryform")]
        public async Task<IActionResult> GetTaskTypeTemplateListingQueryFormAsync(VoidQueryFormParameters form, CancellationToken cancellationToken)
        {
            var response = await sender.Send(form, cancellationToken);
            return Ok(response);
        }

        [HttpGet("getallTaskTypeTemplates")]
        public async Task<IActionResult> GetAllTaskTypeTemplatesAsync(CancellationToken cancellationToken)
        {
            var query = new GetAllTaskTypeTemplatesQuery();
            var result = await sender.Send(query, cancellationToken);

            return Ok(result);
        }        
    }
}
