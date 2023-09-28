using FluentValidation;
using Valghalla.Application.Abstractions.Messaging;
using Valghalla.Application.Communication;
using Valghalla.Application.TaskValidation;
using Valghalla.Internal.Application.Modules.Tasks.Interfaces;

namespace Valghalla.Internal.Application.Modules.Tasks.Commands
{
    public sealed record AssignParticipantToTaskCommand() : ICommand<Response<bool>>
    {
        public Guid TaskAssignmentId { get; set; }
        public Guid ElectionId { get; set; }
        public Guid ParticipantId { get; set; }
        public Guid TaskTypeId { get; set; }
    }

    public sealed class AssignParticipantToTaskCommandValidator : AbstractValidator<AssignParticipantToTaskCommand>
    {
        public AssignParticipantToTaskCommandValidator(ITaskValidationService taskValidationService, IElectionWorkLocationTasksQueryRepository electionWorkLocationTasksQueryRepository)
        {
            RuleFor(x => x.TaskAssignmentId)
                .NotEmpty();

            RuleFor(x => x.ElectionId)
                .NotEmpty();

            RuleFor(x => x.ParticipantId)
                .NotEmpty();

            RuleFor(x => x.TaskTypeId)
                .NotEmpty();

            RuleFor(x => x)
               .Must((command) => !electionWorkLocationTasksQueryRepository.CheckIfTaskHasConflictsAsync(command.ElectionId, command.TaskAssignmentId, command.ParticipantId, default).Result)
               .WithMessage("tasks.error.task_conflict");

            RuleFor(x => taskValidationService.ExecuteAsync(x.TaskTypeId, x.ElectionId, x.ParticipantId, default).Result).Custom((result, context) =>
            {
                if (!result.IsAlive())
                {
                    context.AddFailure("tasks.error.validation_rules_alive");
                }
                if (!result.IsAge18OrOver())
                {
                    context.AddFailure("tasks.error.validation_rules_age");
                }
                if (!result.IsResidencyMunicipality())
                {
                    context.AddFailure("tasks.error.validation_rules_municipality");
                }
                if (!result.IsDisenfranchised())
                {
                    context.AddFailure("tasks.error.validation_rules_legal_adult");
                }
                if (!result.HasDanishCitizenship())
                {
                    context.AddFailure("tasks.error.validation_rules_citizenship");
                }                
            });
        }
    }

    internal class AssignParticipantToTaskCommandHandler : ICommandHandler<AssignParticipantToTaskCommand, Response<bool>>
    {
        private readonly ICommunicationService communicationService;
        private readonly IElectionWorkLocationTasksCommandRepository electionWorkLocationTasksCommandRepository;

        public AssignParticipantToTaskCommandHandler(
            ICommunicationService communicationService,
            IElectionWorkLocationTasksCommandRepository electionWorkLocationTasksCommandRepository)
        {
            this.communicationService = communicationService;
            this.electionWorkLocationTasksCommandRepository = electionWorkLocationTasksCommandRepository;
        }

        public async Task<Response<bool>> Handle(AssignParticipantToTaskCommand command, CancellationToken cancellationToken)
        {
            var result = await electionWorkLocationTasksCommandRepository.AssignParticipantToTaskAsync(command, cancellationToken);

            await communicationService.SendTaskInvitationAsync(command.ParticipantId, command.TaskAssignmentId, cancellationToken);

            return Response.Ok(result);
        }
    }
}
