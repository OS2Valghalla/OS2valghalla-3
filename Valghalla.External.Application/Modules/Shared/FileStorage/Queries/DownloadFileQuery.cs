using FluentValidation;
using Valghalla.Application.Abstractions.Messaging;
using Valghalla.Application.Storage;
using Valghalla.External.Application.Modules.Unprotected.Responses;

namespace Valghalla.External.Application.Modules.Shared.FileStorage.Queries
{
    public sealed record DownloadFileQuery(Guid Id) : IQuery<Response>;

    public sealed class DownloadFileQueryValidator : AbstractValidator<DownloadFileQuery>
    {
        public DownloadFileQueryValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty();
        }
    }

    internal class DownloadFileQueryHandler : IQueryHandler<DownloadFileQuery>
    {
        private readonly IFileStorageService fileStorageService;

        public DownloadFileQueryHandler(IFileStorageService fileStorageService)
        {
            this.fileStorageService = fileStorageService;
        }

        public async Task<Response> Handle(DownloadFileQuery query, CancellationToken cancellationToken)
        {
            try
            {
                var (stream, fileName) = await fileStorageService.DownloadAsync(query.Id, cancellationToken);

                var response = new FileResponse()
                {
                    Stream = stream,
                    FileName = fileName,
                };

                return Response.Ok(response);
            }
            catch (Valghalla.Application.Exceptions.FileNotFoundException)
            {
                return Response.FailWithItemNotFoundError();
            }
        }
    }
}
