using FluentValidation;
using Valghalla.Application.Abstractions.Messaging;
using Valghalla.Internal.Application.Modules.Administration.Communication.Interfaces;

namespace Valghalla.Internal.Application.Modules.Administration.Communication.Commands
{
    public sealed record UpdateCommunicationTemplateCommand() : ICommand<Response>
    {
        public Guid Id { get; init; }
        public string Title { get; init; } = null!;
        public string? Subject { get; set; }
        public string? Content { get; set; }
        public int TemplateType { get; set; }
        public IEnumerable<Guid> FileReferenceIds { get; init; } = Enumerable.Empty<Guid>();
    }

    public sealed class UpdateCommunicationTemplateCommandValidator : AbstractValidator<UpdateCommunicationTemplateCommand>
    {
        public UpdateCommunicationTemplateCommandValidator(ICommunicationQueryRepository communicationQueryRepository)
        {
            RuleFor(x => x.Id)
               .NotEmpty();

            RuleFor(x => x.Title)
                .MaximumLength(Valghalla.Application.Constants.Validation.MaximumGeneralStringLength)
                .NotEmpty();

            RuleFor(x => x)
                .Must((command) => !communicationQueryRepository.CheckIfCommunicationTemplateExistsAsync(command, default).Result)
                .WithMessage("communication.error.communication_template_exist");
        }
    }

    internal class UpdateCommunicationTemplateCommandHandler : ICommandHandler<UpdateCommunicationTemplateCommand, Response>
    {
        private readonly ICommunicationCommandRepository communicationCommandRepository;
        public UpdateCommunicationTemplateCommandHandler(ICommunicationCommandRepository communicationCommandRepository)
        {
            this.communicationCommandRepository = communicationCommandRepository;
        }

        public async Task<Response> Handle(UpdateCommunicationTemplateCommand command, CancellationToken cancellationToken)
        {
            await communicationCommandRepository.UpdateCommunicationTemplateAsync(command, cancellationToken);

            return Response.Ok();
        }
    }
}
