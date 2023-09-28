using FluentValidation;
using Valghalla.Application.Abstractions.Messaging;
using Valghalla.Internal.Application.Modules.Administration.Communication.Interfaces;

namespace Valghalla.Internal.Application.Modules.Administration.Communication.Commands
{
    public sealed record CreateCommunicationTemplateCommand() : ICommand<Response<Guid>>
    {
        public string Title { get; init; } = null!;
        public string? Subject { get; set; }
        public string? Content { get; set; }
        public int TemplateType { get; set; }
        public IEnumerable<Guid> FileReferenceIds { get; init; } = Enumerable.Empty<Guid>();
    }

    public sealed class CreateCommunicationTemplateCommandValidator : AbstractValidator<CreateCommunicationTemplateCommand>
    {
        public CreateCommunicationTemplateCommandValidator(ICommunicationQueryRepository communicationQueryRepository)
        {
            RuleFor(x => x.Title)
                .MaximumLength(Valghalla.Application.Constants.Validation.MaximumGeneralStringLength)
                .NotEmpty();

            RuleFor(x => x)
                .Must((command) => !communicationQueryRepository.CheckIfCommunicationTemplateExistsAsync(command, default).Result)
                .WithMessage("communication.error.communication_template_exist");
        }
    }

    internal class CreateCommunicationTemplateCommandHandler : ICommandHandler<CreateCommunicationTemplateCommand, Response<Guid>>
    {
        private readonly ICommunicationCommandRepository communicationCommandRepository;
        public CreateCommunicationTemplateCommandHandler(ICommunicationCommandRepository communicationCommandRepository)
        {
            this.communicationCommandRepository = communicationCommandRepository;
        }

        public async Task<Response<Guid>> Handle(CreateCommunicationTemplateCommand command, CancellationToken cancellationToken)
        {
            var id = await communicationCommandRepository.CreateCommunicationTemplateAsync(command, cancellationToken);

            return Response.Ok(id);
        }
    }
}
