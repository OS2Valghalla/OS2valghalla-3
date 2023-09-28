using MediatR;
using Microsoft.AspNetCore.Mvc;
using Valghalla.External.API.Requests.MyProfile;
using Valghalla.External.Application.Modules.MyProfile.Commands;
using Valghalla.Integration.Auth;

namespace Valghalla.External.API.Controllers.MyProfile
{
    [ApiController]
    [Route("api/myprofile")]
    [UserAuthorize(RoleEnum.Participant)]
    public class MyProfileCommandController : ControllerBase
    {
        private readonly ISender sender;

        public MyProfileCommandController(ISender sender)
        {
            this.sender = sender;
        }

        [HttpPost("updatemyprofile")]
        public async Task<IActionResult> UpdateMyProfileAsync(UpdateMyProfileRequest request, CancellationToken cancellationToken)
        {
            var command = new UpdateMyProfileCommand()
            {
                Email = request.Email,
                MobileNumber = request.MobileNumber,
                SpecialDietIds = request.SpecialDietIds,
            };

            var response = await sender.Send(command, cancellationToken);
            return Ok(response);
        }

        [HttpDelete("deletemyprofile")]
        public async Task<IActionResult> DeleteMyProfileAsync(CancellationToken cancellationToken)
        {
            var command = new DeleteMyProfileCommand();
            var response = await sender.Send(command, cancellationToken);
            return Ok(response);
        }
    }
}
