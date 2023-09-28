using MediatR;
using Microsoft.AspNetCore.Mvc;
using Valghalla.Integration.Auth;
using Valghalla.Internal.Application.Modules.Shared.Participant.Queries;

namespace Valghalla.Internal.API.Controllers.Shared.Participant
{
    [ApiController]
    [Route("api/shared/participant")]
    [UserAuthorize(RoleEnum.Administrator)]
    public class ParticipantSharedQueryController : ControllerBase
    {
        private readonly ISender sender;

        public ParticipantSharedQueryController(ISender sender)
        {
            this.sender = sender;
        }

        [HttpPost("getparticipants")]
        public async Task<IActionResult> GetParticipantsAsync(IEnumerable<Guid> values, CancellationToken cancellationToken)
        {
            var query = new GetParticipantsSharedQuery() { Values = values };
            var result = await sender.Send(query, cancellationToken);
            return Ok(result);
        }

        [HttpPost("queryparticipantsharedlisting")]
        public async Task<IActionResult> QueryParticipantSharedListingAsync(ParticipantSharedListingQueryForm query, CancellationToken cancellationToken)
        {
            var result = await sender.Send(query, cancellationToken);
            return Ok(result);
        }

        [HttpPost("getparticipantsharedlistingqueryform")]
        public async Task<IActionResult> GetParticipantSharedListingQueryFormAsync(ParticipantSharedListingQueryFormParameters query, CancellationToken cancellationToken)
        {
            var result = await sender.Send(query, cancellationToken);
            return Ok(result);
        }
    }
}
