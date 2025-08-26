using MediatR;
using Microsoft.AspNetCore.Mvc;
using Valghalla.Application.QueryEngine;
using Valghalla.Integration.Auth;
using Valghalla.Internal.Application.Modules.Administration.TaskType.Queries;

namespace Valghalla.Internal.API.Controllers.Administration.TaskType
{
    [ApiController]
    [Route("api/administration/tasktype")]
    [UserAuthorize(RoleEnum.Administrator)]
    public class TaskTypeQueryController : ControllerBase
    {
        private readonly ISender sender;

        public TaskTypeQueryController(ISender sender)
        {
            this.sender = sender;
        }

        [HttpGet("gettasktypedetails")]
        public async Task<IActionResult> GetTaskTypeDetailsAsync(Guid id, CancellationToken cancellationToken)
        {
            var query = new GetTaskTypeDetailsQuery(id);
            var result = await sender.Send(query, cancellationToken);

            return Ok(result);
        }

        [HttpPost("querytasktypelisting")]
        public async Task<IActionResult> QueryTaskTypeListingAsync(TaskTypeListingQueryForm form, CancellationToken cancellationToken)
        {
            var response = await sender.Send(form, cancellationToken);
            return Ok(response);
        }

        [HttpPost("gettasktypelistingqueryform")]
        public async Task<IActionResult> GetTaskTypeListingQueryFormAsync(VoidQueryFormParameters form, CancellationToken cancellationToken)
        {
            var response = await sender.Send(form, cancellationToken);
            return Ok(response);
        }

        [HttpGet("getalltasktypes")]
        public async Task<IActionResult> GetAllTaskTypesAsync(CancellationToken cancellationToken)
        {
            var query = new GetAllTaskTypesQuery();
            var result = await sender.Send(query, cancellationToken);

            return Ok(result);
        }        
    }
}
