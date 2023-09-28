using FluentValidation;
using Valghalla.Application.Abstractions.Messaging;
using Valghalla.Application.User;
using Valghalla.External.Application.Modules.Tasks.Interfaces;
using Valghalla.External.Application.Modules.Tasks.Responses;

namespace Valghalla.External.Application.Modules.Tasks.Commands
{
    public sealed record UnregisterTaskCommand(Guid TaskAssignmentId) : ICommand<Response>;

    public sealed class UnregisterTaskCommandValidator : AbstractValidator<UnregisterTaskCommand>
    {
        public UnregisterTaskCommandValidator()
        {
            RuleFor(x => x.TaskAssignmentId)
                .NotEmpty();
        }
    }

    internal class UnregisterTaskCommandHandler : ICommandHandler<UnregisterTaskCommand, Response>
    {
        private readonly IUserContextProvider userContextProvider;
        private readonly ITaskCommandRepository taskCommandRepository;

        public UnregisterTaskCommandHandler(
            IUserContextProvider userContextProvider,
            ITaskCommandRepository taskCommandRepository)
        {
            this.userContextProvider = userContextProvider;
            this.taskCommandRepository = taskCommandRepository;
        }

        public async Task<Response> Handle(UnregisterTaskCommand command, CancellationToken cancellationToken)
        {
            var participantId = userContextProvider.CurrentUser.ParticipantId!.Value;

            await taskCommandRepository.UnregisterTaskAsync(command, participantId, cancellationToken);

            return Response.Ok(TaskConfirmationResult.SuccessResult());
        }
    }
}
