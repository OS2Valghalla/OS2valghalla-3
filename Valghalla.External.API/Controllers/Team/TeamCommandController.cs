using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;
using Valghalla.External.Application.Modules.Tasks.Request;
using Valghalla.External.Application.Modules.Team.Commands;
using Valghalla.External.Application.Modules.Team.Request;
using Valghalla.Integration.Auth;

namespace Valghalla.External.API.Controllers.Team
{
    [ApiController]
    [Route("api/team")]
    [UserAuthorize(RoleEnum.TeamResponsible)]
    public class TeamCommandController : ControllerBase
    {
        private readonly ISender sender;

        public TeamCommandController(ISender sender)
        {
            this.sender = sender;
        }

        [HttpPost("removemember")]
        public async Task<IActionResult> RemoveTeamMemberAsync([FromBody] RemoveTeamMemberRequest request, CancellationToken cancellationToken)
        {
            var query = new RemoveTeamMemberCommand(request.TeamId, request.ParticipantId);
            var response = await sender.Send(query, cancellationToken);
            return Ok(response);
        }

        [HttpPost("createteamlink")]
        public async Task<IActionResult> CreateTeamLinkAsync([FromBody] CreateTeamLinkRequest request, CancellationToken cancellationToken)
        {
            string hashResult = await GenerateHashValue(request.TeamId.ToString());

            var createCommand = new CreateTeamLinkCommand(hashResult, request.TeamId.ToString());
            var id = await sender.Send(createCommand, cancellationToken);

            return Ok(id);
        }

        private async Task<string> GenerateHashValue(string input)
        {
            string hashResult = string.Empty;
            using (var hashProcess = SHA256.Create())
            {
                var byteArrayResultOfRawData = new MemoryStream(Encoding.UTF8.GetBytes(input));
                var byteArrayResult = await hashProcess.ComputeHashAsync(byteArrayResultOfRawData);
                hashResult = string.Concat(Array.ConvertAll(byteArrayResult, h => h.ToString("X2")));
            }

            return hashResult;
        }
    }
}
