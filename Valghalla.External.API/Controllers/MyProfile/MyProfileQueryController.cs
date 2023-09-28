using MediatR;
using Microsoft.AspNetCore.Mvc;
using Valghalla.External.Application.Modules.MyProfile.Queries;
using Valghalla.Integration.Auth;

namespace Valghalla.External.API.Controllers.MyProfile
{
    [ApiController]
    [Route("api/myprofile")]
    [UserAuthorize(RoleEnum.Participant)]
    public class MyProfileQueryController : ControllerBase
    {
        private readonly ISender sender;

        public MyProfileQueryController(ISender sender)
        {
            this.sender = sender;
        }

        [HttpGet("getmyprofile")]
        public async Task<IActionResult> GetMyProfileAsync(CancellationToken cancellationToken)
        {
            var query = new GetMyProfileQuery();
            var response = await sender.Send(query, cancellationToken);
            return Ok(response);
        }

        [HttpGet("getmyprofilepermission")]
        public async Task<IActionResult> GetMyProfilePermissionAsync(CancellationToken cancellationToken)
        {
            var query = new GetMyProfilePermissionQuery();
            var response = await sender.Send(query, cancellationToken);
            return Ok(response);
        }
    }
}
