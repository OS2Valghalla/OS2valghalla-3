using FluentValidation;
using Valghalla.Application;
using Valghalla.Application.Abstractions.Messaging;
using Valghalla.Application.Storage;

namespace Valghalla.Internal.Application.Modules.Shared.FileStorage.Commands
{
    public sealed record UploadFileCommand(Stream Stream, string FileName, string Type) : ICommand<Response<Guid>>;

    public sealed class UploadFileCommandValidator : AbstractValidator<UploadFileCommand>
    {
        public UploadFileCommandValidator()
        {
            RuleFor(x => x.Stream)
                .NotEmpty();

            RuleFor(x => x.FileName)
                .NotEmpty();

            RuleFor(x => x.Type)
                .NotEmpty();

            When(x => x.Type == "tasktype", () =>
            {
                RuleFor(x => x.Stream)
                    .Must(x => x.Length <= 10485760) // 10MB
                    .WithMessage("administration.task_type.error.max_file_size_limit");
            });

            When(x => x.Type == "contactinfo", () =>
            {
                RuleFor(x => x.Stream)
                    .Must(x => x.Length <= 10485760)
                    .WithMessage("administration.web.error.max_file_size_limit");
            });

            When(x => x.Type == "communicationtemplate", () =>
            {
                RuleFor(x => x.Stream)
                    .Must(x => x.Length <= 10485760)
                    .WithMessage("communication.error.max_file_size_limit");
            });
        }
    }

    internal class UploadFileCommandHandler : ICommandHandler<UploadFileCommand, Response<Guid>>
    {
        private readonly IFileStorageService fileStorageService;

        public UploadFileCommandHandler(IFileStorageService fileStorageService)
        {
            this.fileStorageService = fileStorageService;
        }

        public async Task<Response<Guid>> Handle(UploadFileCommand command, CancellationToken cancellationToken)
        {
            var fileId = command.Type == "contactinfo" ? Constants.FileStorage.MunicipalityLogo : Guid.NewGuid();

            await fileStorageService.UploadAsync(command.Stream, fileId, command.FileName, cancellationToken);

            return Response.Ok(fileId);
        }
    }
}
