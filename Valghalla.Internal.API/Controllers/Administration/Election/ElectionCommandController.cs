using MediatR;
using Microsoft.AspNetCore.Mvc;
using Valghalla.Integration.Auth;
using Valghalla.Internal.API.Requests.Administration.Election;
using Valghalla.Internal.Application.Abstractions.Messaging;
using Valghalla.Internal.Application.Modules.Administration.Election.Commands;

namespace Valghalla.Internal.API.Controllers.Administration.Election
{
    [ApiController]
    [Route("api/administration/election")]
    [UserAuthorize(RoleEnum.Administrator)]
    public class ElectionCommandController : ControllerBase
    {
        private readonly ISender sender;

        public ElectionCommandController(ISender sender)
        {
            this.sender = sender;
        }


        [HttpPost("createelection")]
        public async Task<IActionResult> CreateElectionAsync([FromBody] CreateElectionRequest request, CancellationToken cancellationToken)
        {
            var command = new CreateElectionCommand()
            {
                Title = request.Title,
                ElectionTypeId = request.ElectionTypeId,
                LockPeriod = request.LockPeriod,
                ElectionStartDate = request.ElectionStartDate.ToUniversalTime(),
                ElectionEndDate = request.ElectionEndDate.ToUniversalTime(),
                ElectionDate = request.ElectionDate.ToUniversalTime(),
                WorkLocationIds = request.WorkLocationIds,
                ConfirmationOfRegistrationCommunicationTemplateId = request.ConfirmationOfRegistrationCommunicationTemplateId,
                ConfirmationOfCancellationCommunicationTemplateId = request.ConfirmationOfCancellationCommunicationTemplateId,
                InvitationCommunicationTemplateId = request.InvitationCommunicationTemplateId,
                InvitationReminderCommunicationTemplateId = request.InvitationReminderCommunicationTemplateId,
                TaskReminderCommunicationTemplateId = request.TaskReminderCommunicationTemplateId,
                RetractedInvitationCommunicationTemplateId = request.RetractedInvitationCommunicationTemplateId,
                ElectionTaskTypeCommunicationTemplates = request.ElectionTaskTypeCommunicationTemplates
            };

            var id = await sender.Send(command, cancellationToken);
            return Ok(id);
        }

        [HttpPut("updateelection")]
        public async Task<IActionResult> UpdateElectionAsync([FromBody] UpdateElectionRequest request, CancellationToken cancellationToken)
        {
            var command = new UpdateElectionCommand()
            {
                Id = request.Id,
                Title = request.Title,
                LockPeriod = request.LockPeriod,
            };

            var result = await sender.Send(command, cancellationToken);
            return Ok(result);
        }


        [HttpPost("duplicateelection")]
        public async Task<IActionResult> DuplicateElectionAsync([FromBody] DuplicateElectionRequest request, CancellationToken cancellationToken)
        {
            var command = new DuplicateElectionCommand()
            {
                SourceElectionId = request.SourceElectionId,
                Title = request.Title,
                ElectionTypeId = request.ElectionTypeId,
                LockPeriod = request.LockPeriod,
                ElectionDate = request.ElectionDate.ToUniversalTime(),                
                DaysBeforeElectionDate = request.DaysBeforeElectionDate,
                DaysAfterElectionDate = request.DaysAfterElectionDate,
                WorkLocationIds = request.WorkLocationIds,
                ConfirmationOfRegistrationCommunicationTemplateId = request.ConfirmationOfRegistrationCommunicationTemplateId,
                ConfirmationOfCancellationCommunicationTemplateId = request.ConfirmationOfCancellationCommunicationTemplateId,
                InvitationCommunicationTemplateId = request.InvitationCommunicationTemplateId,
                InvitationReminderCommunicationTemplateId = request.InvitationReminderCommunicationTemplateId,
                TaskReminderCommunicationTemplateId = request.TaskReminderCommunicationTemplateId,
                RetractedInvitationCommunicationTemplateId = request.RetractedInvitationCommunicationTemplateId,
                ElectionTaskTypeCommunicationTemplates = request.ElectionTaskTypeCommunicationTemplates
            };

            var id = await sender.Send(command, cancellationToken);
            return Ok(id);
        }

        [HttpPut("updateelectioncommunicationconfigurations")]
        public async Task<IActionResult> UpdateElectionCommunicationConfigurationsAsync([FromBody] UpdateElectionCommunicationConfigurationsRequest request, CancellationToken cancellationToken)
        {
            var command = new UpdateElectionCommunicationConfigurationsCommand()
            {
                Id = request.Id,
                ConfirmationOfRegistrationCommunicationTemplateId = request.ConfirmationOfRegistrationCommunicationTemplateId,
                ConfirmationOfCancellationCommunicationTemplateId = request.ConfirmationOfCancellationCommunicationTemplateId,
                InvitationCommunicationTemplateId = request.InvitationCommunicationTemplateId,
                InvitationReminderCommunicationTemplateId = request.InvitationReminderCommunicationTemplateId,
                TaskReminderCommunicationTemplateId = request.TaskReminderCommunicationTemplateId,
                RetractedInvitationCommunicationTemplateId = request.RetractedInvitationCommunicationTemplateId,
                ElectionTaskTypeCommunicationTemplates = request.ElectionTaskTypeCommunicationTemplates
            };

            var result = await sender.Send(command, cancellationToken);
            return Ok(result);
        }

        [HttpDelete("deleteelection")]
        public async Task<IActionResult> DeleteElectionAsync(Guid id, CancellationToken cancellationToken)
        {
            var command = new DeleteElectionCommand(id).Apply(HttpContext);
            var response = await sender.Send(command, cancellationToken);
            return Ok(response);
        }

        [HttpPost("activateelection")]
        public async Task<IActionResult> ActivateElectionAsync(Guid id, CancellationToken cancellationToken)
        {
            var command = new ActivateElectionCommand(id).Apply(HttpContext);
            var result = await sender.Send(command, cancellationToken);
            return Ok(result);
        }

        [HttpPost("deactivateelection")]
        public async Task<IActionResult> DeactivateElectionAsync(Guid id, CancellationToken cancellationToken)
        {
            var command = new DeactivateElectionCommand(id).Apply(HttpContext);
            var result = await sender.Send(command, cancellationToken);
            return Ok(result);
        }
    }
}
