using MediatR;
using Microsoft.AspNetCore.Mvc;
using Valghalla.Integration.Auth;
using Valghalla.Internal.Application.Modules.Administration.User.Queries;

namespace Valghalla.Internal.API.Controllers.Administration.User
{
    [ApiController]
    [Route("api/administration/user")]
    [UserAuthorize(RoleEnum.Administrator)]
    public class UserQueryController : ControllerBase
    {
        private readonly ISender sender;

        public UserQueryController(ISender sender)
        {
            this.sender = sender;
        }

        [HttpGet("getuserroles")]
        public async Task<IActionResult> GetUserRolesAsync(CancellationToken cancellationToken)
        {
            var query = new GetUserRolesQuery();
            var result = await sender.Send(query, cancellationToken);

            return Ok(result);
        }

        [HttpGet("getusers")]
        public async Task<IActionResult> GetUsersAsync(CancellationToken cancellationToken)
        {
            var query = new GetUsersQuery();
            var result = await sender.Send(query, cancellationToken);

            return Ok(result);
        }
    }
}
