using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Valghalla.Application.Abstractions.Messaging;
using Valghalla.Integration.Auth;
using Valghalla.Internal.Application.Modules.Shared.FileStorage.Queries;
using Valghalla.Internal.Application.Modules.Shared.FileStorage.Responses;

namespace Valghalla.Internal.API.Controllers.Shared.FileStorage
{
    [ApiController]
    [Route("api/shared/filestorage")]
    [UserAuthorize(RoleEnum.Administrator)]
    public class FileStorageQueryController : ControllerBase
    {
        private readonly ISender sender;
        private readonly FileExtensionContentTypeProvider contentTypeProvider;

        public FileStorageQueryController(ISender sender)
        {
            this.sender = sender;
            contentTypeProvider = new FileExtensionContentTypeProvider();
        }

        [HttpGet("download")]
        public async Task<IActionResult> DownloadFileAsync(Guid id, CancellationToken cancellationToken)
        {
            var command = new DownloadFileQuery(id);
            var response = await sender.Send(command, cancellationToken);

            if (response is Response<FileResponse> fileResponse)
            {
                var stream = fileResponse.Data!.Stream;
                var fileName = fileResponse.Data!.FileName;
                contentTypeProvider.TryGetContentType(fileName, out var contentType);
                return File(stream, contentType ?? "application/octet-stream", fileName);
            }

            return NotFound();
        }
    }
}
