using FluentValidation;
using Valghalla.Application.Abstractions.Messaging;
using Valghalla.Application.Confirmations;
using Valghalla.Internal.Application.Modules.Administration.TaskTypeTemplate.Interfaces;

namespace Valghalla.Internal.Application.Modules.Administration.TaskTypeTemplate.Commands
{
    public sealed record UpdateTaskTypeTemplateCommand() : ConfirmationCommand<Response>
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

    public sealed class UpdateTaskTypeTemplateCommandValidator : AbstractValidator<UpdateTaskTypeTemplateCommand>
    {
        public UpdateTaskTypeTemplateCommandValidator(ITaskTypeTemplateQueryRepository repository)
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
                RuleFor(x => x.Payment).GreaterThan(0);
            });

            RuleFor(x => x)
                .Must((command) => !repository.CheckIfTaskTypeTemplateExistsAsync(command, default).Result)
                .WithMessage("administration.task_type.error.task_type_exist");
        }
    }

    public sealed class UpdateTaskTypeTemplateCommandConfirmator : Confirmator<UpdateTaskTypeTemplateCommand>
    {
        public override string Title => "administration.task_type.update_task_type_confirmation.title";
        public override string Message => string.Empty;

        public UpdateTaskTypeTemplateCommandConfirmator(ITaskTypeTemplateQueryRepository repository)
        {
            BypassIfNoRuleTriggers = true;

            WhenAsync(repository.CheckIfTaskTypeTemplateHasTasksInActiveElectionAsync)
                .WithMessage(async (command, cancellationToken) =>
                {
                    var messages = new List<string>();
                    var TaskTypeTemplate = (await repository.GetTaskTypeTemplateAsync(command.Id, cancellationToken))!;

                    if (TaskTypeTemplate.ValidationNotRequired != command.ValidationNotRequired)
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

                    if (TaskTypeTemplate.Trusted != command.Trusted)
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

    internal class UpdateTaskTypeTemplateCommandHandler : ICommandHandler<UpdateTaskTypeTemplateCommand, Response>
    {
        private readonly ITaskTypeTemplateCommandRepository TaskTypeTemplateCommandRepository;
        public UpdateTaskTypeTemplateCommandHandler(ITaskTypeTemplateCommandRepository TaskTypeTemplateCommandRepository)
        {
            this.TaskTypeTemplateCommandRepository = TaskTypeTemplateCommandRepository;
        }

        public async Task<Response> Handle(UpdateTaskTypeTemplateCommand command, CancellationToken cancellationToken)
        {
            await TaskTypeTemplateCommandRepository.UpdateTaskTypeTemplateAsync(command, cancellationToken);
            return Response.Ok();
        }
    }
}
