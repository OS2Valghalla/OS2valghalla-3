using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Valghalla.External.Application.Modules.Unprotected.Queries;
using Valghalla.External.Application.Modules.Unprotected.Request;

namespace Valghalla.External.API.Controllers.Unprotected
{

    [ApiController]
    [Route("api/unprotected/tasks")]
    public class UnprotectedTasksQueryController : ControllerBase
    {
        private readonly ISender sender;
        private readonly FileExtensionContentTypeProvider contentTypeProvider;

        public UnprotectedTasksQueryController(ISender sender)
        {
            this.sender = sender;
            contentTypeProvider = new FileExtensionContentTypeProvider();
        }

        [HttpGet("gettasksfiltersoptions")]
        public async Task<IActionResult> GetTasksFiltersOptionsAsync(string hashValue, CancellationToken cancellationToken)
        {
            var query = new GetTasksFiltersOptionsQuery(hashValue);
            var result = await sender.Send(query, cancellationToken);
            return Ok(result);
        }

        [HttpPost("getavailabletasksbyfilters")]
        public async Task<IActionResult> GetUnprotectedAvailableTasksByFiltersAsync([FromBody] GetUnprotectedTasksByFiltersRequest request, CancellationToken cancellationToken)
        {
            var query = new GetUnprotectedAvailableTasksByFiltersQuery(request.HashValue, request.TasksFilter);
            var result = await sender.Send(query, cancellationToken);
            return Ok(result);
        }
    }
}
