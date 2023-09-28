using MediatR;
using Microsoft.AspNetCore.Mvc;
using Valghalla.Integration.Auth;
using Valghalla.Internal.API.Requests.Administration.Communication;
using Valghalla.Internal.Application.Modules.Administration.Communication.Queries;

namespace Valghalla.Internal.API.Controllers.Administration.Communication
{
    [ApiController]
    [Route("api/administration/communication")]
    [UserAuthorize(RoleEnum.Administrator)]
    public class CommunicationQueryController: ControllerBase
    {
        private readonly ISender sender;

        public CommunicationQueryController(ISender sender)
        {
            this.sender = sender;
        }

        [HttpGet("getcommunicationtemplate")]
        public async Task<IActionResult> GetCommunicationTemplateAsync(Guid id, CancellationToken cancellationToken)
        {
            var query = new GetCommunicationTemplateQuery(id);
            var result = await sender.Send(query, cancellationToken);
            return Ok(result);
        }

        [HttpPost("getparticipantsforsendinggroupmessage")]
        public async Task<IActionResult> GetParticipantsForSendingGroupMessageAsync([FromBody] GetParticipantsForSendingGroupMessageRequest request, CancellationToken cancellationToken)
        {
            var query = new GetParticipantsForSendingGroupMessageQuery(request.ElectionId, request.Filters.WorkLocationIds, request.Filters.TeamIds, request.Filters.TaskTypeIds, request.Filters.TaskStatuses, request.Filters.TaskDates);
            var result = await sender.Send(query, cancellationToken);
            return Ok(result);
        }

        [HttpPost("querycommunicationtemplatelisting")]
        public async Task<IActionResult> QueryCommunicationTemplateListingAsync(CommunicationTemplateListingQueryForm form, CancellationToken cancellationToken)
        {
            var response = await sender.Send(form, cancellationToken);
            return Ok(response);
        }

        [HttpPost("getcommunicationtemplatelistingqueryform")]
        public async Task<IActionResult> GetCommunicationTemplateListingQueryFormAsync(CommunicationTemplateListingQueryFormParameters form, CancellationToken cancellationToken)
        {
            var response = await sender.Send(form, cancellationToken);
            return Ok(response);
        }
    }
}
