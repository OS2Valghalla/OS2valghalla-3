using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Valghalla.Integration.Auth;
using Valghalla.Internal.API.Requests.Administration.Link;
using Valghalla.Internal.Application.Modules.Administration.Link.Commands;

namespace Valghalla.Internal.API.Controllers.Administration.Link
{
    [ApiController]
    [Route("api/administration/link")]
    [UserAuthorize(RoleEnum.Administrator)]
    public class LinkCommandController : ControllerBase
    {
        private readonly ISender sender;

        public LinkCommandController(ISender sender) 
        {
            this.sender = sender;
        }

        /// <summary>
        /// Generate a task specific link id to be shared between the internal and external application.
        /// The link id is a hash value based on the "request" input (should contain the election id and the task filter)
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("createtasksfilteredlink")]
        public async Task<IActionResult> CreateTasksFilteredLinkAsync([FromBody] CreateTasksFilteredLinkRequest request, CancellationToken cancellationToken)
        {
            request.TasksFilter.ElectionId = request.ElectionId;
            string textTasksFilter = JsonSerializer.Serialize(request.TasksFilter);
            //Generate the hash value based on the taskId input to ensure no two links can be identical
            string hashResult = await GenerateHashValue(textTasksFilter);

            var createCommand = new CreateTasksFilteredLinkCommand(request.ElectionId, hashResult, textTasksFilter);
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
