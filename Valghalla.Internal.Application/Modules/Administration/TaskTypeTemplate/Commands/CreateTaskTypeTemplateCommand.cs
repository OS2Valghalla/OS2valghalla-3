using FluentValidation;
using Valghalla.Application.Abstractions.Messaging;
using Valghalla.Internal.Application.Modules.Administration.TaskTypeTemplate.Interfaces;

namespace Valghalla.Internal.Application.Modules.Administration.TaskTypeTemplate.Commands
{
    public sealed record CreateTaskTypeTemplateCommand() : ICommand<Response<Guid>>
    {
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

    public sealed class CreateTaskTypeTemplateCommandValidator : AbstractValidator<CreateTaskTypeTemplateCommand>
    {
        public CreateTaskTypeTemplateCommandValidator(ITaskTypeTemplateQueryRepository TaskTypeTemplateQueryRepository)
        {
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
                .Must((command) => !TaskTypeTemplateQueryRepository.CheckIfTaskTypeTemplateExistsAsync(command, default).Result)
                .WithMessage("administration.task_type.error.task_type_exist");
        }
    }

    internal class CreateTaskTypeTemplateCommandHandler : ICommandHandler<CreateTaskTypeTemplateCommand, Response<Guid>>
    {
        private readonly ITaskTypeTemplateCommandRepository TaskTypeTemplateCommandRepository;
        public CreateTaskTypeTemplateCommandHandler(ITaskTypeTemplateCommandRepository TaskTypeTemplateCommandRepository)
        {
            this.TaskTypeTemplateCommandRepository = TaskTypeTemplateCommandRepository;
        }

        public async Task<Response<Guid>> Handle(CreateTaskTypeTemplateCommand command, CancellationToken cancellationToken)
        {
            var id = await TaskTypeTemplateCommandRepository.CreateTaskTypeTemplateAsync(command, cancellationToken);

            return Response.Ok(id);
        }
    }
}
