using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;
using Valghalla.Integration.Auth;
using Valghalla.Internal.Application.Modules.Administration.Link.Queries;

namespace Valghalla.Internal.API.Controllers.Administration.Link
{
    [ApiController]
    [Route("api/administration/link")]
    [UserAuthorize(RoleEnum.Administrator)]
    public class LinkQueryController : ControllerBase
    {
        private readonly ISender sender;

        public LinkQueryController(ISender sender)
        {
            this.sender = sender;
        }

        [HttpGet("gettasklink")]
        public async Task<IActionResult> GetTaskLinkAsync(Guid electionId, Guid taskId, CancellationToken cancellationToken)
        {
            //Generate the hash value based on the teamId input to ensure no two links can be identical
            string hashResult = await GenerateHashValue(taskId.ToString());

            var command = new GetTaskLinkQuery(hashResult, electionId);
            var id = await sender.Send(command, cancellationToken);

            //if (!id.IsSuccess && id.Message == "shared.error.common.not_found")
            //{
            //    var createCommand = new CreateTeamLinkCommand(electionId, hashResult, teamId.ToString());
            //    id = await sender.Send(createCommand, cancellationToken);
            //}

            return Ok(id);
        }

        [HttpGet("gettasksfilteredlink")]
        public async Task<IActionResult> GetTasksFiltereLinkAsync(Guid electionId, string taskFilterString, CancellationToken cancellationToken)
        {
            //Generate the hash value based on the teamId input to ensure no two links can be identical
            string hashResult = await GenerateHashValue(taskFilterString);

            var command = new GetTasksFilteredLinkQuery(hashResult, electionId);
            var id = await sender.Send(command, cancellationToken);

            //if (!id.IsSuccess && id.Message == "shared.error.common.not_found")
            //{
            //    var createCommand = new CreateTeamLinkCommand(electionId, hashResult, teamId.ToString());
            //    id = await sender.Send(createCommand, cancellationToken);
            //}

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
