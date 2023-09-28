using FluentValidation;
using Valghalla.Application.Abstractions.Messaging;
using Valghalla.Application.Communication;
using Valghalla.Application.User;
using Valghalla.External.Application.Modules.Tasks.Interfaces;
using Valghalla.External.Application.Modules.Tasks.Responses;

namespace Valghalla.External.Application.Modules.Tasks.Commands
{
    public sealed record RejectTaskCommand(string HashValue, Guid? InvitationCode) : ICommand<Response>;

    public sealed class RejectTaskCommandValidator : AbstractValidator<RejectTaskCommand>
    {
        public RejectTaskCommandValidator()
        {
            RuleFor(x => x.HashValue)
                .NotEmpty();
        }
    }

    internal class RejectTaskCommandHandler : ICommandHandler<RejectTaskCommand, Response>
    {
        private readonly IUserContextProvider userContextProvider;
        private readonly ITaskQueryRepository taskQueryRepository;
        private readonly ITaskCommandRepository taskCommandRepository;
        private readonly ICommunicationService communicationService;

        public RejectTaskCommandHandler(
            IUserContextProvider userContextProvider,
            ITaskQueryRepository taskQueryRepository,
            ITaskCommandRepository taskCommandRepository,
            ICommunicationService communicationService)
        {
            this.userContextProvider = userContextProvider;
            this.taskQueryRepository = taskQueryRepository;
            this.taskCommandRepository = taskCommandRepository;
            this.communicationService = communicationService;
        }

        public async Task<Response> Handle(RejectTaskCommand command, CancellationToken cancellationToken)
        {
            var participantId = userContextProvider.CurrentUser.ParticipantId!.Value;
            var taskAssignment = await taskQueryRepository.GetTaskAssignmentAsync(command.HashValue, command.InvitationCode, participantId, cancellationToken);

            if (taskAssignment == null)
            {
                return Response.Ok(TaskConfirmationResult.CprInvalidResult());
            }

            await taskCommandRepository.RejectTaskAsync(command, participantId, cancellationToken);

            await communicationService.SendTaskCancellationAsync(participantId, taskAssignment.Id, cancellationToken);

            return Response.Ok(TaskConfirmationResult.SuccessResult());
        }
    }
}
