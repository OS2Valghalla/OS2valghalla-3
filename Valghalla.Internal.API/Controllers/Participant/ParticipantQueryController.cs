using MediatR;
using Microsoft.AspNetCore.Mvc;
using Valghalla.Integration.Auth;
using Valghalla.Internal.Application.Modules.Participant.Queries;

namespace Valghalla.Internal.API.Controllers.Person
{
    [ApiController]
    [Route("api/participant")]
    [UserAuthorize(RoleEnum.Administrator)]
    public class ParticipantQueryController : ControllerBase
    {
        private readonly ISender sender;

        public ParticipantQueryController(ISender sender)
        {
            this.sender = sender;
        }

        [HttpPost("queryparticipantlisting")]
        public async Task<IActionResult> QueryParticipantListingAsync(ParticipantListingQueryForm query, CancellationToken cancellationToken)
        {
            var result = await sender.Send(query, cancellationToken);
            return Ok(result);
        }

        [HttpPost("getparticipantlistingqueryform")]
        public async Task<IActionResult> GetParticipantListingQueryFormAsync(ParticipantListingQueryFormParameters query, CancellationToken cancellationToken)
        {
            var result = await sender.Send(query, cancellationToken);
            return Ok(result);
        }

        [HttpGet("getparticipantdetails")]
        public async Task<IActionResult> GetParticipantDetailsAsync(Guid id, CancellationToken cancellationToken)
        {
            var query = new GetParticipantDetailsQuery(id);
            var result = await sender.Send(query, cancellationToken);
            return Ok(result);
        }

        [HttpGet("getparticipantpersonalrecord")]
        public async Task<IActionResult> GetParticipantPersonalRecordAsync(string cpr, CancellationToken cancellationToken)
        {
            var query = new GetParticipantPersonalRecordQuery(cpr);
            var result = await sender.Send(query, cancellationToken);
            return Ok(result);
        }

        [HttpGet("getteamresponsiblerights")]
        public async Task<IActionResult> GetTeamResponsibleRightsAsync(Guid id, CancellationToken cancellationToken)
        {
            var query = new GetTeamResponsibleRightsQuery(id);
            var result = await sender.Send(query, cancellationToken);
            return Ok(result);
        }

        [HttpPost("queryparticipanteventloglisting")]
        public async Task<IActionResult> QueryParticipantEventLogListingAsync(ParticipantEventLogListingQueryForm query, CancellationToken cancellationToken)
        {
            var result = await sender.Send(query, cancellationToken);
            return Ok(result);
        }

        [HttpGet("getworklocationresponsiblerights")]
        public async Task<IActionResult> GetWorkLocationResponsibleRightsAsync(Guid id, CancellationToken cancellationToken)
        {
            var query = new GetWorkLocationResponsibleRightsQuery(id);
            var result = await sender.Send(query, cancellationToken);
            return Ok(result);
        }

        [HttpGet("getparticipanttasks")]
        public async Task<IActionResult> GetParticipantTasksAsync(Guid id, CancellationToken cancellationToken)
        {
            var query = new GetParticipantTasksQuery(id);
            var result = await sender.Send(query, cancellationToken);
            return Ok(result);
        }
    }
}
