using FluentValidation;
using Valghalla.Application.Abstractions.Messaging;
using Valghalla.Internal.Application.Modules.Participant.Interfaces;

namespace Valghalla.Internal.Application.Modules.Participant.Commands
{
    public sealed record CreateParticipantEventLogCommand() : ICommand<Response<Guid>>
    {
        public Guid ParticipantId { get; init; }
        public string Text { get; init; } = string.Empty;
    }

    public sealed class CreateParticipantEventLogCommandValidator : AbstractValidator<CreateParticipantEventLogCommand>
    {
        public CreateParticipantEventLogCommandValidator()
        {
            RuleFor(x => x.ParticipantId)
                .NotEmpty();

            RuleFor(x => x.Text)
                .NotEmpty();
        }
    }

    internal class CreateParticipantEventLogCommandHandler : ICommandHandler<CreateParticipantEventLogCommand, Response<Guid>>
    {
        private readonly IParticipantEventLogCommandRepository participantEventLogCommandRepository;

        public CreateParticipantEventLogCommandHandler(IParticipantEventLogCommandRepository participantEventLogCommandRepository)
        {
            this.participantEventLogCommandRepository = participantEventLogCommandRepository;
        }

        public async Task<Response<Guid>> Handle(CreateParticipantEventLogCommand command, CancellationToken cancellationToken)
        {
            var id = await participantEventLogCommandRepository.CreateParticipantEventLogAsync(command, cancellationToken);
            return Response.Ok(id);
        }
    }
}
