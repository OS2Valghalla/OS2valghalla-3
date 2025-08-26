using FluentValidation;

using Valghalla.Application.Abstractions.Messaging;
using Valghalla.Application.Confirmations;
using Valghalla.Internal.Application.Modules.Administration.TaskType.Interfaces;

namespace Valghalla.Internal.Application.Modules.Administration.TaskType.Commands
{
    public sealed record UpdateTaskTypeCommand() : ConfirmationCommand<Response>
    {
        public Guid Id { get; init; }
        public string Title { get; init; } = null!;
        public string ShortName { get; init; } = null!;
        public string Description { get; init; } = null!;
        public TimeSpan StartTime { get; init; }
        public TimeSpan EndTime { get; init; }
        public int? Payment { get; init; }
        public bool ValidationNotRequired { get; init; }
        public bool Trusted { get; init; }
        public bool SendingReminderEnabled { get; init; } = true;
        public IEnumerable<Guid> FileReferenceIds { get; init; } = Enumerable.Empty<Guid>();
    }

    public sealed class UpdateTaskTypeCommandValidator : AbstractValidator<UpdateTaskTypeCommand>
    {
        public UpdateTaskTypeCommandValidator(ITaskTypeQueryRepository repository)
        {
            RuleFor(x => x.Id)
                .NotEmpty();

            RuleFor(x => x.Title)
                .MaximumLength(Valghalla.Application.Constants.Validation.MaximumGeneralStringLength)
                .NotEmpty();

            RuleFor(x => x.ShortName)
                .MaximumLength(50)
                .NotEmpty();

            RuleFor(x => x.Description)
                .NotEmpty();

            RuleFor(x => x.StartTime)
                .LessThan(TimeSpan.FromHours(24))
                .NotEmpty();

            RuleFor(x => x.EndTime)
                .LessThan(TimeSpan.FromHours(24))
                .NotEmpty();

            When(command => command.Payment.HasValue, () =>
            {
                RuleFor(x => x.Payment).GreaterThanOrEqualTo(0);
            });

            RuleFor(x => x)
                .Must((command) => !repository.CheckIfTaskTypeExistsAsync(command, default).Result)
                .WithMessage("administration.task_type.error.task_type_exist");
        }
    }

    public sealed class UpdateTaskTypeCommandConfirmator : Confirmator<UpdateTaskTypeCommand>
    {
        public override string Title => "administration.task_type.update_task_type_confirmation.title";
        public override string Message => string.Empty;

        public UpdateTaskTypeCommandConfirmator(ITaskTypeQueryRepository repository)
        {
            BypassIfNoRuleTriggers = true;

            WhenAsync(repository.CheckIfTaskTypeHasTasksInActiveElectionAsync)
                .WithMessage(async (command, cancellationToken) =>
                {
                    var messages = new List<string>();
                    var taskType = (await repository.GetTaskTypeAsync(command.Id, cancellationToken))!;

                    if (taskType.ValidationNotRequired != command.ValidationNotRequired)
                    {
                        if (command.ValidationNotRequired)
                        {
                            messages.Add("administration.task_type.update_task_type_confirmation.validation_not_required_turn_on");
                        }
                        else
                        {
                            messages.Add("administration.task_type.update_task_type_confirmation.validation_not_required_turn_off");
                        }
                    }

                    if (taskType.Trusted != command.Trusted)
                    {
                        if (command.Trusted)
                        {
                            messages.Add("administration.task_type.update_task_type_confirmation.trusted_turn_on");
                        }
                        else
                        {
                            messages.Add("administration.task_type.update_task_type_confirmation.trusted_turn_off");
                        }
                    }

                    return messages.ToArray();
                });
        }
    }

    internal class UpdateTaskTypeCommandHandler : ICommandHandler<UpdateTaskTypeCommand, Response>
    {
        private readonly ITaskTypeCommandRepository taskTypeCommandRepository;
        public UpdateTaskTypeCommandHandler(ITaskTypeCommandRepository taskTypeCommandRepository)
        {
            this.taskTypeCommandRepository = taskTypeCommandRepository;
        }

        public async Task<Response> Handle(UpdateTaskTypeCommand command, CancellationToken cancellationToken)
        {
            await taskTypeCommandRepository.UpdateTaskTypeAsync(command, cancellationToken);
            return Response.Ok();
        }
    }
}
