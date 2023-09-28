using FluentValidation;
using Valghalla.Application.Abstractions.Messaging;
using Valghalla.Internal.Application.Modules.Participant.Interfaces;

namespace Valghalla.Internal.Application.Modules.Participant.Commands
{
    public sealed record UpdateParticipantEventLogCommand() : ICommand<Response>
    {
        public Guid Id { get; init; }
        public string Text { get; init; } = string.Empty;
    }

    public sealed class UpdateParticipantEventLogCommandValidator : AbstractValidator<UpdateParticipantEventLogCommand>
    {
        public UpdateParticipantEventLogCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty();

            RuleFor(x => x.Text)
                .NotEmpty();
        }
    }

    internal class UpdateParticipantEventLogCommandHandler : ICommandHandler<UpdateParticipantEventLogCommand, Response>
    {
        private readonly IParticipantEventLogCommandRepository participantEventLogCommandRepository;

        public UpdateParticipantEventLogCommandHandler(IParticipantEventLogCommandRepository participantEventLogCommandRepository)
        {
            this.participantEventLogCommandRepository = participantEventLogCommandRepository;
        }

        public async Task<Response> Handle(UpdateParticipantEventLogCommand command, CancellationToken cancellationToken)
        {
            await participantEventLogCommandRepository.UpdateParticipantEventLogAsync(command, cancellationToken);
            return Response.Ok();
        }
    }
}
