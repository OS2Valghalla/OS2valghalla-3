using MediatR;

using Microsoft.AspNetCore.Mvc;

using Valghalla.Integration.Auth;
using Valghalla.Internal.Application.Modules.Tasks.Queries;

namespace Valghalla.Internal.API.Controllers.Tasks
{
    [ApiController]
    [Route("api/areatasks")]
    [UserAuthorize(RoleEnum.Administrator)]
    public class AreaTasksQueryController : ControllerBase
    {
        private readonly ISender sender;

        public AreaTasksQueryController(ISender sender)
        {
            this.sender = sender;
        }

        [HttpGet("getareasgeneralinfo")]
        public async Task<IActionResult> GetAreasGeneralInfoAsync(Guid electionId, CancellationToken cancellationToken)
        {
            var query = new GetElectionAreasGeneralInfoQuery(electionId);
            var result = await sender.Send(query, cancellationToken);
            return Ok(result);
        }

        [HttpGet("getareataskssummary")]
        public async Task<IActionResult> GetAreaTasksSummaryAsync(
            [FromQuery] Guid electionId,
            [FromQuery] List<DateTime>? selectedDates,
            [FromQuery] List<Guid>? selectedTeamIds,
            CancellationToken cancellationToken)
        {
            IList<DateTime>? normalizedDates = selectedDates?.Select(d => d.ToUniversalTime().Date).Distinct().ToList();
            var query = new GetElectionAreaTasksSummaryQuery(electionId, normalizedDates, selectedTeamIds);
            var result = await sender.Send(query, cancellationToken);
            return Ok(result);
        }
        [HttpGet("gettasksstatussummary")]
        public async Task<IActionResult> GetTasksStatusSummaryQuery(Guid electionId, CancellationToken cancellationToken)
        {
            var query = new GetParticipantsTasksStatusQuery(electionId);
            var result = await sender.Send(query, cancellationToken);
            return Ok(result);
        }
        [HttpGet("getrejectedtasks")]
        public async Task<IActionResult> GetRejectedTasksAsync(Guid electionId, CancellationToken cancellationToken)
        {
            var query = new GetRejectedTasksQuery(electionId);
            var result = await sender.Send(query, cancellationToken);
            return Ok(result);
        }
    }
}
