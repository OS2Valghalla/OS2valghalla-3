using FluentValidation;
using Valghalla.Application.Abstractions.Messaging;
using Valghalla.Application.Communication;
using Valghalla.Application.TaskValidation;
using Valghalla.Application.User;
using Valghalla.External.Application.Modules.Tasks.Interfaces;
using Valghalla.External.Application.Modules.Tasks.Responses;

namespace Valghalla.External.Application.Modules.Tasks.Commands
{
    public sealed record AcceptTaskCommand(string HashValue, Guid? InvitationCode, bool taskInvitation) : ICommand<Response>;

    public sealed class AcceptTaskCommandValidator : AbstractValidator<AcceptTaskCommand>
    {
        public AcceptTaskCommandValidator()
        {
            RuleFor(x => x.HashValue)
                .NotEmpty();
        }
    }

    internal class AcceptTaskCommandHandler : ICommandHandler<AcceptTaskCommand, Response>
    {
        private readonly IUserContextProvider userContextProvider;
        private readonly ITaskQueryRepository taskQueryRepository;
        private readonly ITaskCommandRepository taskCommandRepository;
        private readonly ITaskValidationService taskValidationService;
        private readonly ICommunicationService communicationService;

        public AcceptTaskCommandHandler(
            IUserContextProvider userContextProvider,
            ITaskQueryRepository taskQueryRepository,
            ITaskCommandRepository taskCommandRepository,
            ITaskValidationService taskValidationService,
            ICommunicationService communicationService)
        {
            this.userContextProvider = userContextProvider;
            this.taskQueryRepository = taskQueryRepository;
            this.taskCommandRepository = taskCommandRepository;
            this.taskValidationService = taskValidationService;
            this.communicationService = communicationService;
        }

        public async Task<Response> Handle(AcceptTaskCommand command, CancellationToken cancellationToken)
        {
            var participantId = userContextProvider.CurrentUser.ParticipantId!.Value;
            var taskAssignment = await taskQueryRepository.GetTaskAssignmentAsync(command.HashValue, command.InvitationCode,command.taskInvitation, participantId, cancellationToken);

            if (taskAssignment == null)
            {
                return Response.Ok(TaskConfirmationResult.CprInvalidResult());
            }
            
            var conflicted = false;
            if (!command.taskInvitation)
            {
                conflicted = await taskQueryRepository.CheckIfTaskHasConflicts(
                    participantId,
                    taskAssignment.TaskDate,
                    taskAssignment.StartTime,
                    taskAssignment.EndTime,
                    command.InvitationCode,
                    cancellationToken
                );
            }
            if (conflicted)
            {
                return Response.Ok(TaskConfirmationResult.ConflictResult());
            }

            var validationResult = await taskValidationService.ExecuteAsync(taskAssignment.Id, taskAssignment.ElectionId, participantId, cancellationToken);

            if (!validationResult.Succeed)
            {
                return Response.Ok(TaskConfirmationResult.ValidationFailedResult(validationResult));
            }

            await taskCommandRepository.AcceptTaskAsync(command, participantId, cancellationToken);

            await communicationService.SendTaskRegistrationAsync(participantId, taskAssignment.Id, cancellationToken);

            return Response.Ok(TaskConfirmationResult.SuccessResult());
        }
    }
}
