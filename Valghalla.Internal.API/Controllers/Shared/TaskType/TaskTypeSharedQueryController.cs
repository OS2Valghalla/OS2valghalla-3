using MediatR;
using Microsoft.AspNetCore.Mvc;
using Valghalla.Integration.Auth;
using Valghalla.Internal.Application.Modules.Shared.TaskType.Queries;

namespace Valghalla.Internal.API.Controllers.Shared.TaskType
{
    [ApiController]
    [Route("api/shared/tasktype")]
    [UserAuthorize(RoleEnum.Administrator)]
    public class TaskTypeSharedQueryController : ControllerBase
    {
        private readonly ISender sender;

        public TaskTypeSharedQueryController(ISender sender)
        {
            this.sender = sender;
        }

        [HttpGet("gettasktypes")]
        public async Task<IActionResult> GetTaskTypesAsync(CancellationToken cancellationToken)
        {
            var query = new GetTaskTypesSharedQuery();
            var result = await sender.Send(query, cancellationToken);
            return Ok(result);
        }
    }
}
