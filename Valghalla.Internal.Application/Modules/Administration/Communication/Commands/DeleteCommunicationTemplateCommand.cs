using FluentValidation;
using Valghalla.Application.Abstractions.Messaging;
using Valghalla.Application.Confirmations;
using Valghalla.Internal.Application.Modules.Administration.Communication.Interfaces;

namespace Valghalla.Internal.Application.Modules.Administration.Communication.Commands
{
    public sealed record DeleteCommunicationTemplateCommand(Guid Id) : ConfirmationCommand<Response>;

    public sealed class DeleteCommunicationTemplateCommandValidator : AbstractValidator<DeleteCommunicationTemplateCommand>
    {
        public DeleteCommunicationTemplateCommandValidator(ICommunicationQueryRepository communicationQueryRepository)
        {
            RuleFor(x => x.Id)
                .NotEmpty();

            RuleFor(x => x.Id)
              .Must((id) => !communicationQueryRepository.CheckIsDefaultCommunicationTemplateAsync(id, default).Result)
              .WithMessage("communication.error.cannot_delete_default_communication_template");
        }
    }

    public sealed class DeleteCommunicationTemplateCommandConfirmator : Confirmator<DeleteCommunicationTemplateCommand>
    {
        public override string Title => "communication.delete_communication_template_dialog.title";
        public override string Message => "communication.delete_communication_template_dialog.content";
        public DeleteCommunicationTemplateCommandConfirmator(ICommunicationQueryRepository repository)
        {
            WhenAsync(repository.CheckIfCommunicationTemplateUsedInElectionAsync)
                .WithMessage("communication.delete_communication_template_dialog.delete_communication_template_used_in_election");
        }
    }

    internal class DeleteCommunicationTemplateCommandHandler : ICommandHandler<DeleteCommunicationTemplateCommand, Response>
    {
        private readonly ICommunicationCommandRepository communicationCommandRepository;

        public DeleteCommunicationTemplateCommandHandler(ICommunicationCommandRepository communicationCommandRepository)
        {
            this.communicationCommandRepository = communicationCommandRepository;
        }

        public async Task<Response> Handle(DeleteCommunicationTemplateCommand command, CancellationToken cancellationToken)
        {
            await communicationCommandRepository.DeleteCommunicationTemplateAsync(command, cancellationToken);
            return Response.Ok();
        }
    }
}
