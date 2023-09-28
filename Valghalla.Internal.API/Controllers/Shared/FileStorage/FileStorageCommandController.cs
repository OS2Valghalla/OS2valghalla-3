using MediatR;
using Microsoft.AspNetCore.Mvc;
using Valghalla.Integration.Auth;
using Valghalla.Internal.Application.Modules.Shared.FileStorage.Commands;

namespace Valghalla.Internal.API.Controllers.Shared.FileStorage
{
    [ApiController]
    [Route("api/shared/filestorage")]
    [UserAuthorize(RoleEnum.Administrator)]
    public class FileStorageCommandController : ControllerBase
    {
        private readonly ISender sender;

        public FileStorageCommandController(ISender sender)
        {
            this.sender = sender;
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadFileAsync([FromForm] IFormFile content, [FromQuery] string type, CancellationToken cancellationToken)
        {
            var stream = content.OpenReadStream();
            var fileName = content.FileName;

            var command = new UploadFileCommand(stream, fileName, type);
            var id = await sender.Send(command, cancellationToken);

            await stream.DisposeAsync();

            return Ok(id);
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteFileAsync(Guid id, CancellationToken cancellationToken)
        {
            var command = new DeleteFileCommand(id);
            var result = await sender.Send(command, cancellationToken);
            return Ok(result);
        }
    }
}
