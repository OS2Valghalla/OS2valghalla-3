using FluentValidation;
using Valghalla.Application.Abstractions.Messaging;
using Valghalla.Internal.Application.Modules.Administration.TaskType.Interfaces;

namespace Valghalla.Internal.Application.Modules.Administration.TaskType.Commands
{
    public sealed record CreateTaskTypeCommand() : ICommand<Response<Guid>>
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

    public sealed class CreateTaskTypeCommandValidator : AbstractValidator<CreateTaskTypeCommand>
    {
        public CreateTaskTypeCommandValidator(ITaskTypeQueryRepository taskTypeQueryRepository)
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
                .Must((command) => !taskTypeQueryRepository.CheckIfTaskTypeExistsAsync(command, default).Result)
                .WithMessage("administration.task_type.error.task_type_exist");
        }
    }

    internal class CreateTaskTypeCommandHandler : ICommandHandler<CreateTaskTypeCommand, Response<Guid>>
    {
        private readonly ITaskTypeCommandRepository taskTypeCommandRepository;
        public CreateTaskTypeCommandHandler(ITaskTypeCommandRepository taskTypeCommandRepository)
        {
            this.taskTypeCommandRepository = taskTypeCommandRepository;
        }

        public async Task<Response<Guid>> Handle(CreateTaskTypeCommand command, CancellationToken cancellationToken)
        {
            var id = await taskTypeCommandRepository.CreateTaskTypeAsync(command, cancellationToken);

            return Response.Ok(id);
        }
    }
}
