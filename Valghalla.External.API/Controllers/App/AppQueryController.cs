using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Valghalla.Application.Abstractions.Messaging;
using Valghalla.Application.Exceptions;
using Valghalla.Application.User;
using Valghalla.External.Application.Modules.App.Queries;
using Valghalla.Integration.Auth;

namespace Valghalla.External.API.Controllers.App
{
    [ApiController]
    [Route("api/app")]
    public class AppQueryController : ControllerBase
    {
        private readonly ISender sender;
        private readonly IUserContextProvider userContextProvider;

        public AppQueryController(ISender sender, IUserContextProvider userContextProvider)
        {
            this.sender = sender;
            this.userContextProvider = userContextProvider;
        }

        [HttpGet("context")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAppContextAsync(CancellationToken cancellationToken)
        {
            var query = new GetAppContextQuery();
            var response = await sender.Send(query, cancellationToken);
            return Ok(response);
        }

        [HttpGet("user")]
        public IActionResult GetUser()
        {
            var user = userContextProvider.CurrentUser ?? throw new UserException("Could not get current user info.");

            var response = Valghalla.Application.Abstractions.Messaging.Response.Ok(new UserInfo()
            {
                Id = user.UserId,
                RoleIds = user.RoleIds,
                Name = user.Name,
            });

            return Ok(response);
        }

        [HttpGet("logo")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAppLogoAsync(CancellationToken cancellationToken)
        {
            var query = new GetAppLogoQuery();
            var result = await sender.Send(query, cancellationToken);

            if (result is not Response<string> fileResponse)
            {
                return BadRequest();
            }

            if (string.IsNullOrEmpty(fileResponse.Data))
            {
                return NotFound();
            }

            return Content(fileResponse.Data, "image/svg+xml");
        }

        [HttpGet("userteams")]
        [UserAuthorize(RoleEnum.Participant)]
        public async Task<IActionResult> GetUserTeamAsync(CancellationToken cancellationToken)
        {
            var query = new GetUserTeamsQuery();
            var response = await sender.Send(query, cancellationToken);
            return Ok(response);
        }
    }
}
