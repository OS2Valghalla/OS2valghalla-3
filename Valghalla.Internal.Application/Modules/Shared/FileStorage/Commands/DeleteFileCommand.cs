using FluentValidation;
using Valghalla.Application.Abstractions.Messaging;
using Valghalla.Application.Storage;

namespace Valghalla.Internal.Application.Modules.Shared.FileStorage.Commands
{
    public sealed record DeleteFileCommand(Guid Id) : ICommand<Response>;

    public class DeleteFileCommandValidator : AbstractValidator<DeleteFileCommand>
    {
        public DeleteFileCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty();
        }
    }

    internal class DeleteFileCommandHandler : ICommandHandler<DeleteFileCommand, Response>
    {
        private readonly IFileStorageService fileStorageService;

        public DeleteFileCommandHandler(IFileStorageService fileStorageService)
        {
            this.fileStorageService = fileStorageService;
        }

        public async Task<Response> Handle(DeleteFileCommand command, CancellationToken cancellationToken)
        {
            try
            {
                await fileStorageService.DeleteAsync(command.Id, cancellationToken);
                return Response.Ok();
            }
            catch (Valghalla.Application.Exceptions.FileNotFoundException)
            {
                return Response.FailWithItemNotFoundError();
            }
        }
    }
}
