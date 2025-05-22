using FluentValidation;

using Valghalla.Application.Abstractions.Messaging;
using Valghalla.Application.Communication;
using Valghalla.Application.User;
using Valghalla.External.Application.Modules.Tasks.Interfaces;
using Valghalla.External.Application.Modules.Tasks.Responses;

namespace Valghalla.External.Application.Modules.Tasks.Commands
{
    public sealed record UnregisterTaskCommand(Guid TaskAssignmentId, string HashValue) : ICommand<Response>;
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
        private readonly ITaskQueryRepository taskQueryRepository;
        private readonly ICommunicationService communicationService;

        public UnregisterTaskCommandHandler(
            IUserContextProvider userContextProvider,
            ITaskCommandRepository taskCommandRepository,
            ICommunicationService communicationService,
            ITaskQueryRepository taskQueryRepository)
        {
            this.userContextProvider = userContextProvider;
            this.taskCommandRepository = taskCommandRepository;
            this.communicationService = communicationService;
            this.taskQueryRepository = taskQueryRepository;
        }

        public async Task<Response> Handle(UnregisterTaskCommand command, CancellationToken cancellationToken)
        {
            var participantId = userContextProvider.CurrentUser.ParticipantId!.Value;
            var taskAssignment = await taskQueryRepository.GetTaskAssignmentAsync(command.HashValue, participantId, cancellationToken);

            if (taskAssignment == null)
            {
                return Response.Ok(TaskConfirmationResult.CprInvalidResult());
            }
            await taskCommandRepository.UnregisterTaskAsync(command, participantId, cancellationToken);

            await communicationService.SendTaskCancellationAsync(participantId, taskAssignment.Id, cancellationToken);

            return Response.Ok(TaskConfirmationResult.SuccessResult());
        }
    }
}
