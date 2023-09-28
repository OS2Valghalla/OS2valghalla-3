using MediatR;
using Microsoft.AspNetCore.Mvc;
using Valghalla.Application.AuditLog;
using Valghalla.Integration.Auth;
using Valghalla.Internal.API.Requests.Administration.Link;
using Valghalla.Internal.Application.Modules.Tasks.Queries;

namespace Valghalla.Internal.API.Controllers.Tasks
{
    [ApiController]
    [Route("api/filteredtasks")]
    [UserAuthorize(RoleEnum.Administrator)]
    public class FilteredTasksQueryController : ControllerBase
    {
        private readonly ISender sender;
        private readonly IAuditLogService auditLogService;

        public FilteredTasksQueryController(ISender sender, IAuditLogService auditLogService)
        {
            this.sender = sender;
            this.auditLogService = auditLogService;
        }

        [HttpGet("gettasksfiltersoptions")]
        public async Task<IActionResult> GetTasksFiltersOptionsAsync(Guid electionId, CancellationToken cancellationToken)
        {
            var query = new GetTasksFiltersOptionsQuery(electionId);
            var result = await sender.Send(query, cancellationToken);
            return Ok(result);
        }

        [HttpPost("getavailabletasksbyfilters")]
        public async Task<IActionResult> GetAvailableTasksByFiltersAsync([FromBody] CreateTasksFilteredLinkRequest request, CancellationToken cancellationToken)
        {
            var query = new GetAvailableTasksByFiltersQuery(request.ElectionId, request.TasksFilter);
            var result = await sender.Send(query, cancellationToken);
            return Ok(result);
        }

        [HttpPost("getparticipantstasks")]
        public async Task<IActionResult> GetParticipantsTasksQuery([FromBody] GetParticipantsTasksRequest request, CancellationToken cancellationToken)
        {
            var query = new GetParticipantsTasksQuery(request.ElectionId, request.TasksFilter);
            var result = await sender.Send(query, cancellationToken);
            return Ok(result);
        }

        [HttpPost("auditlogexport")]
        public async Task<IActionResult> AuditLogParticipantListExporter(CancellationToken cancellationToken)
        {
            await auditLogService.AddAuditLogAsync(new ParticipantListExportAuditLog(), cancellationToken);
            return Ok();
        }
    }
}
