using FluentValidation;
using Valghalla.Application;
using Valghalla.Application.Abstractions.Messaging;
using Valghalla.Internal.Application.Modules.Participant.Interfaces;

namespace Valghalla.Internal.Application.Modules.Participant.Commands
{
    public sealed record UpdateParticipantCommand : ICommand<Response>
    {
        public Guid Id { get; init; }
        public string? MobileNumber { get; init; }
        public string? Email { get; init; }
        public IEnumerable<Guid> SpecialDietIds { get; init; } = Enumerable.Empty<Guid>();
        public IEnumerable<Guid> TeamIds { get; init; } = Enumerable.Empty<Guid>();
    }

    public sealed class UpdateParticipantCommandValidator : AbstractValidator<UpdateParticipantCommand>
    {
        public UpdateParticipantCommandValidator(IParticipantQueryRepository participantQueryRepository)
        {
            RuleFor(x => x.Id)
                .NotEmpty();

            When(x => !string.IsNullOrEmpty(x.MobileNumber), () =>
            {
                RuleFor(x => x.MobileNumber).Length(Constants.Validation.MobileNumberLength);
            });

            When(x => !string.IsNullOrEmpty(x.Email), () =>
            {
                RuleFor(x => x.Email).EmailAddress();
            });

            RuleFor(x => x.TeamIds)
                .NotEmpty();

            RuleFor(x => x)
                .Must((command) => !participantQueryRepository.CheckIfParticipantHasAssociatedTasksAsync(command, default).Result)
                .WithMessage("participant.error.participant_has_associated_tasks");
        }
    }

    internal class UpdateParticipantCommandHandler : ICommandHandler<UpdateParticipantCommand, Response>
    {
        private readonly IParticipantCommandRepository participantCommandRepository;

        public UpdateParticipantCommandHandler(IParticipantCommandRepository participantCommandRepository)
        {
            this.participantCommandRepository = participantCommandRepository;
        }

        public async Task<Response> Handle(UpdateParticipantCommand command, CancellationToken cancellationToken)
        {
            await participantCommandRepository.UpdateParticipantAsync(command, cancellationToken);
            return Response.Ok();
        }
    }
}
