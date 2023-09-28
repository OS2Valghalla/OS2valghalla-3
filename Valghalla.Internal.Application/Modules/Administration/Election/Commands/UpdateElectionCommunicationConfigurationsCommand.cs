using FluentValidation;
using Valghalla.Application.Abstractions.Messaging;
using Valghalla.Internal.Application.Modules.Administration.Election.Interfaces;
using Valghalla.Internal.Application.Modules.Administration.Election.Requests;

namespace Valghalla.Internal.Application.Modules.Administration.Election.Commands
{
    public sealed record UpdateElectionCommunicationConfigurationsCommand : ICommand<Response>
    {
        public Guid Id { get; init; }
        public Guid? ConfirmationOfRegistrationCommunicationTemplateId { get; set; }
        public Guid? ConfirmationOfCancellationCommunicationTemplateId { get; set; }
        public Guid? InvitationCommunicationTemplateId { get; set; }
        public Guid? InvitationReminderCommunicationTemplateId { get; set; }
        public Guid? TaskReminderCommunicationTemplateId { get; set; }
        public Guid? RetractedInvitationCommunicationTemplateId { get; set; }
        public IList<UpdateElectionTaskTypeCommunicationTemplateRequest> ElectionTaskTypeCommunicationTemplates { get; set; } = new List<UpdateElectionTaskTypeCommunicationTemplateRequest>();
    }

    public sealed class UpdateElectionCommunicationConfigurationsCommandValidator : AbstractValidator<UpdateElectionCommunicationConfigurationsCommand>
    {
        public UpdateElectionCommunicationConfigurationsCommandValidator(IElectionQueryRepository electionQueryRepository)
        {
            RuleFor(x => x.Id)
                .NotEmpty();
        }
    }

    internal class UpdateElectionCommunicationConfigurationsCommandHandler : ICommandHandler<UpdateElectionCommunicationConfigurationsCommand, Response>
    {
        private readonly IElectionCommandRepository electionCommandRepository;

        public UpdateElectionCommunicationConfigurationsCommandHandler(IElectionCommandRepository electionCommandRepository)
        {
            this.electionCommandRepository = electionCommandRepository;
        }

        public async Task<Response> Handle(UpdateElectionCommunicationConfigurationsCommand command, CancellationToken cancellationToken)
        {
            await electionCommandRepository.UpdateElectionCommunicationConfigurationsAsync(command, cancellationToken);
            return Response.Ok();
        }
    }
}
