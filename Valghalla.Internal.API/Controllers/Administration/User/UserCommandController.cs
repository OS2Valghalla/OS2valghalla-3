using MediatR;
using Microsoft.AspNetCore.Mvc;
using Valghalla.Application.User;
using Valghalla.Integration.Auth;
using Valghalla.Internal.API.Requests.Administration.User;
using Valghalla.Internal.Application.Modules.Administration.User.Commands;

namespace Valghalla.Internal.API.Controllers.Administration.User
{
    [ApiController]
    [Route("api/administration/user")]
    [UserAuthorize(RoleEnum.Administrator)]
    public class UserCommandController : ControllerBase
    {
        private readonly ISender sender;
        private readonly IUserContextProvider userContextProvider;

        public UserCommandController(ISender sender, IUserContextProvider userContextProvider)
        {
            this.sender = sender;
            this.userContextProvider = userContextProvider;
        }

        [HttpPut("updateuser")]
        public async Task<IActionResult> UpdateUserAsync([FromBody] UpdateUserRequest request, CancellationToken cancellationToken)
        {
            var command = new UpdateUserCommand(request.Id, request.RoleId);
            var result = await sender.Send(command, cancellationToken);

            return Ok(result);
        }

        [HttpDelete("deleteuser")]
        public async Task<IActionResult> DeleteUserAsync(Guid id, CancellationToken cancellationToken)
        {
            var currentUserId = userContextProvider.CurrentUser?.UserId;

            if (currentUserId == null)
                return Unauthorized();

            var command = new DeleteUserCommand(id, currentUserId.Value);
            var result = await sender.Send(command, cancellationToken);

            return Ok(result);
        }
    }
}
