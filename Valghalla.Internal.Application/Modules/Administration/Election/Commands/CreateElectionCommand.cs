using FluentValidation;
using Valghalla.Application.Abstractions.Messaging;
using Valghalla.Internal.Application.Modules.Administration.Election.Interfaces;
using Valghalla.Internal.Application.Modules.Administration.Election.Requests;

namespace Valghalla.Internal.Application.Modules.Administration.Election.Commands
{
    public sealed record CreateElectionCommand : ICommand<Response<Guid>>
    {
        public string Title { get; init; } = null!;
        public Guid ElectionTypeId { get; init; }
        public int LockPeriod { get; init; }
        public DateTime ElectionStartDate { get; init; }
        public DateTime ElectionEndDate { get; init; }
        public DateTime ElectionDate { get; init; }
        public IEnumerable<Guid> WorkLocationIds { get; init; } = Array.Empty<Guid>();
        public Guid? ConfirmationOfRegistrationCommunicationTemplateId { get; set; }
        public Guid? ConfirmationOfCancellationCommunicationTemplateId { get; set; }
        public Guid? InvitationCommunicationTemplateId { get; set; }
        public Guid? InvitationReminderCommunicationTemplateId { get; set; }
        public Guid? TaskReminderCommunicationTemplateId { get; set; }
        public Guid? RetractedInvitationCommunicationTemplateId { get; set; }
        public Guid? RemovedFromTaskCommunicationTemplateId { get; set; }
        public Guid? RemovedByValidationCommunicationTemplateId { get; set; }
        public IEnumerable<CreateElectionTaskTypeCommunicationTemplateRequest> ElectionTaskTypeCommunicationTemplates { get; init; } = Array.Empty<CreateElectionTaskTypeCommunicationTemplateRequest>();
    }

    public sealed class CreateElectionCommandValidator : AbstractValidator<CreateElectionCommand>
    {
        public CreateElectionCommandValidator(IElectionQueryRepository electionQueryRepository)
        {
            RuleFor(x => x.Title)
                .MaximumLength(Valghalla.Application.Constants.Validation.MaximumGeneralStringLength)
                .NotEmpty();

            RuleFor(x => x.ElectionTypeId)
                .NotEmpty();

            RuleFor(x => x.LockPeriod)
                .GreaterThan(0);

            RuleFor(x => x.ElectionStartDate)
                .NotEmpty()
                .LessThan(x => x.ElectionEndDate);

            RuleFor(x => x.ElectionEndDate)
                .NotEmpty()
                .GreaterThan(x => x.ElectionStartDate);

            RuleFor(x => x.ElectionDate)
                .NotEmpty();

            RuleFor(x => x.WorkLocationIds)
                .NotEmpty();

            RuleFor(x => x)
                .Must((command) => !electionQueryRepository.CheckIfElectionExistsAsync(command, default).Result)
                .WithMessage("administration.election.error.election_exist");
        }
    }

    internal class CreateElectionCommandHandler : ICommandHandler<CreateElectionCommand, Response<Guid>>
    {
        private readonly IElectionCommandRepository electionCommandRepository;

        public CreateElectionCommandHandler(IElectionCommandRepository electionCommandRepository)
        {
            this.electionCommandRepository = electionCommandRepository;
        }

        public async Task<Response<Guid>> Handle(CreateElectionCommand command, CancellationToken cancellationToken)
        {
            var id = await electionCommandRepository.CreateElectionAsync(command, cancellationToken);
            return Response.Ok(id);
        }
    }
}
