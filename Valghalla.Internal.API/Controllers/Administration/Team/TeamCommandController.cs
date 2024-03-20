using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;
using Valghalla.Integration.Auth;
using Valghalla.Internal.API.Requests.Administration.Team;
using Valghalla.Internal.Application.Abstractions.Messaging;
using Valghalla.Internal.Application.Modules.Administration.Team.Commands;
using Valghalla.Internal.Application.Modules.Team.Commands;
using Valghalla.Internal.Application.Modules.Team.Request;

namespace Valghalla.Internal.API.Controllers.Administration.Team
{
    [ApiController]
    [Route("api/administration/team")]
    [UserAuthorize(RoleEnum.Administrator)]
    public class TeamCommandController : ControllerBase
    {
        private readonly ISender sender;

        public TeamCommandController(ISender sender)
        {
            this.sender = sender;
        }

        [HttpPost("createteam")]
        public async Task<IActionResult> CreateTeamAsync([FromBody] CreateTeamRequest request, CancellationToken cancellationToken)
        {
            var command = new CreateTeamCommand()
            {
                Name = request.Name,
                ShortName = request.ShortName,
                Description = request.Description,
                ResponsibleIds = request.ResponsibleIds
            };

            var id = await sender.Send(command, cancellationToken);
            return Ok(id);
        }

        [HttpDelete("deleteteam")]
        public async Task<IActionResult> DeleteTeamAsync(Guid teamId, CancellationToken cancellationToken)
        {
            var command = new DeleteTeamCommand(teamId).Apply(HttpContext);
            var response = await sender.Send(command, cancellationToken);
            return Ok(response);
        }

        [HttpPut("updateteam")]
        public async Task<IActionResult> UpdateTeamAsync([FromBody] UpdateTeamRequest request, CancellationToken cancellationToken)
        {
            var command = new UpdateTeamCommand()
            {
                Id = request.Id,
                Name = request.Name,
                ShortName = request.ShortName,
                Description = request.Description,
                ResponsibleIds = request.ResponsibleIds
            };

            var response = await sender.Send(command, cancellationToken);
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
