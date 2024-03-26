using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Valghalla.Application.Abstractions.Messaging;
using Valghalla.External.API.Requests.Tasks;
using Valghalla.External.Application.Modules.Shared.FileStorage.Queries;
using Valghalla.External.Application.Modules.Tasks.Queries;
using Valghalla.External.Application.Modules.Unprotected.Responses;
using Valghalla.Integration.Auth;

namespace Valghalla.External.API.Controllers.Task
{
    [ApiController]
    [Route("api/tasks")]
    public class TaskQueryController : ControllerBase
    {
        private readonly ISender sender;
        private readonly FileExtensionContentTypeProvider contentTypeProvider;

        public TaskQueryController(ISender sender)
        {
            this.sender = sender;
            contentTypeProvider = new FileExtensionContentTypeProvider();
        }

        [HttpGet("gettaskpreview")]
        [AllowAnonymous]
        public async Task<IActionResult> GetTaskPreviewAsync(string hashValue, Guid? code, CancellationToken cancellationToken)
        {
            var query = new GetTaskPreviewQuery(hashValue, code);
            var response = await sender.Send(query, cancellationToken);
            return Ok(response);
        }

        [HttpGet("getmytasks")]
        [UserAuthorize(RoleEnum.Participant)]
        public async Task<IActionResult> GetMyTasksAsync(CancellationToken cancellationToken)
        {
            var query = new GetMyTasksQuery();
            var response = await sender.Send(query, cancellationToken);
            return Ok(response);
        }

        [HttpGet("download")]
        [UserAuthorize(RoleEnum.Participant)]
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

        [HttpPost("gettaskoverview")]
        [UserAuthorize(RoleEnum.Participant)]
        public async Task<IActionResult> GetTaskOverviewAsync(GetTaskOverviewRequest request, CancellationToken cancellationToken)
        {
            var query = new GetTaskOverviewQuery()
            {
                TaskDate = request.TaskDate,
                TaskTypeId = request.TaskTypeId,
                TeamId = request.TeamId,
                WorkLocationId = request.WorkLocationId,
            };

            var response = await sender.Send(query, cancellationToken);
            return Ok(response);
        }

        [HttpGet("gettaskoverviewfilteroptions")]
        [UserAuthorize(RoleEnum.Participant)]
        public async Task<IActionResult> GetTaskOverviewFilterOptionsAsync(CancellationToken cancellationToken)
        {
            var query = new GetTaskOverviewFilterOptionsQuery();
            var response = await sender.Send(query, cancellationToken);
            return Ok(response);
        }
    }
}
