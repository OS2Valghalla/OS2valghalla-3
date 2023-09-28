using FluentValidation;
using Valghalla.Application.Abstractions.Messaging;
using Valghalla.Internal.Application.Modules.Participant.Interfaces;

namespace Valghalla.Internal.Application.Modules.Participant.Commands
{
    public sealed record DeleteParticipantEventLogCommand(Guid Id) : ICommand<Response> { }

    public sealed class DeleteParticipantEventLogCommandValidator : AbstractValidator<DeleteParticipantEventLogCommand>
    {
        public DeleteParticipantEventLogCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty();
        }
    }

    internal class DeleteParticipantEventLogCommandHandler : ICommandHandler<DeleteParticipantEventLogCommand, Response>
    {
        private readonly IParticipantEventLogCommandRepository participantEventLogCommandRepository;

        public DeleteParticipantEventLogCommandHandler(IParticipantEventLogCommandRepository participantEventLogCommandRepository)
        {
            this.participantEventLogCommandRepository = participantEventLogCommandRepository;
        }

        public async Task<Response> Handle(DeleteParticipantEventLogCommand command, CancellationToken cancellationToken)
        {
            await participantEventLogCommandRepository.DeleteParticipantEventLogAsync(command, cancellationToken);
            return Response.Ok();
        }
    }
}
