using MediatR;
using Microsoft.AspNetCore.Mvc;
using Valghalla.Integration.Auth;
using Valghalla.Internal.API.Requests.Participant;
using Valghalla.Internal.Application.Abstractions.Messaging;
using Valghalla.Internal.Application.Modules.Participant.Commands;

namespace Valghalla.Internal.API.Controllers.Participant
{
    [ApiController]
    [Route("api/participant")]
    [UserAuthorize(RoleEnum.Administrator)]
    public class ParticipantCommandController : ControllerBase
    {
        private readonly ISender sender;

        public ParticipantCommandController(ISender sender)
        {
            this.sender = sender;
        }


        [HttpPost("createparticipant")]
        public async Task<IActionResult> CreateParticipantAsync([FromBody] CreateParticipantRequest request, CancellationToken cancellationToken)
        {
            var command = new CreateParticipantCommand()
            {
                Cpr = request.Cpr,
                MobileNumber = request.MobileNumber,
                Email = request.Email,
                SpecialDietIds = request.SpecialDietIds ?? Array.Empty<Guid>(),
                TeamIds = request.TeamIds,
                TaskId = request.TaskId,
                ElectionId = request.ElectionId,
            };

            var id = await sender.Send(command, cancellationToken);
            return Ok(id);
        }

        [HttpPut("updateparticipant")]
        public async Task<IActionResult> UpdateParticipantAsync([FromBody] UpdateParticipantRequest request, CancellationToken cancellationToken)
        {
            var command = new UpdateParticipantCommand()
            {
                Id = request.Id,
                MobileNumber = request.MobileNumber,
                Email = request.Email,
                SpecialDietIds = request.SpecialDietIds ?? Array.Empty<Guid>(),
                TeamIds = request.TeamIds
            };

            var result = await sender.Send(command, cancellationToken);
            return Ok(result);
        }

        [HttpDelete("deleteparticipant")]
        public async Task<IActionResult> DeleteParticipantAsync(Guid id, CancellationToken cancellationToken)
        {
            var command = new DeleteParticipantCommand(id).Apply(HttpContext);
            var result = await sender.Send(command, cancellationToken);
            return Ok(result);
        }

        [HttpPost("createparticipanteventlog")]
        public async Task<IActionResult> CreateParticipantEventLogAsync([FromBody] CreateParticipantEventLogRequest request, CancellationToken cancellationToken)
        {
            var command = new CreateParticipantEventLogCommand()
            {
                ParticipantId = request.ParticipantId,
                Text = request.Text
            };

            var response = await sender.Send(command, cancellationToken);
            return Ok(response);
        }

        [HttpPut("updateparticipanteventlog")]
        public async Task<IActionResult> UpdateParticipantEventLogAsync([FromBody] UpdateParticipantEventLogRequest request, CancellationToken cancellationToken)
        {
            var command = new UpdateParticipantEventLogCommand()
            {
                Id = request.Id,
                Text = request.Text
            };

            var response = await sender.Send(command, cancellationToken);
            return Ok(response);
        }

        [HttpDelete("deleteparticipanteventlog")]
        public async Task<IActionResult> DeleteParticipantEventLogAsync(Guid id, CancellationToken cancellationToken)
        {
            var command = new DeleteParticipantEventLogCommand(id);
            var response = await sender.Send(command, cancellationToken);
            return Ok(response);
        }

        [HttpPost("bulkdeleteparticipants")]
        public async Task<IActionResult> BulkDeleteParticipantsAsync(IEnumerable<Guid> participantIds, CancellationToken cancellationToken)
        {
            var command = new BulkDeleteParticipantsCommand(participantIds).Apply(HttpContext);
            var result = await sender.Send(command, cancellationToken);
            return Ok(result);
        }
    }
}
