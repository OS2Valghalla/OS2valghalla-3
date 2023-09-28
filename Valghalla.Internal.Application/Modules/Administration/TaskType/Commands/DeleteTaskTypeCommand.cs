using FluentValidation;
using Valghalla.Application.Abstractions.Messaging;
using Valghalla.Application.Confirmations;
using Valghalla.Internal.Application.Modules.Administration.TaskType.Interfaces;

namespace Valghalla.Internal.Application.Modules.Administration.TaskType.Commands
{
    public sealed record DeleteTaskTypeCommand(Guid Id) : ConfirmationCommand<Response>;

    public sealed class DeleteTaskTypeCommandValidator : AbstractValidator<DeleteTaskTypeCommand>
    {
        public DeleteTaskTypeCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty();
        }
    }

    public sealed class DeleteTaskTypeCommandConfirmator : Confirmator<DeleteTaskTypeCommand>
    {
        public override string Title => "administration.task_type.delete_task_type_confirmation.title";
        public override string Message => "administration.task_type.delete_task_type_confirmation.content";

        public DeleteTaskTypeCommandConfirmator(ITaskTypeQueryRepository repository)
        {
            WhenAsync(repository.CheckIfTaskTypeHasTasksInActiveElectionAsync)
                .WithMessage("administration.task_type.delete_task_type_confirmation.has_tasks_connected_to_work_location");
        }
    }

    internal class DeleteTaskTypeCommandHandler : ICommandHandler<DeleteTaskTypeCommand, Response>
    {
        private readonly ITaskTypeCommandRepository taskTypeCommandRepository;
        public DeleteTaskTypeCommandHandler(ITaskTypeCommandRepository taskTypeCommandRepository)
        {
            this.taskTypeCommandRepository = taskTypeCommandRepository;
        }

        public async Task<Response> Handle(DeleteTaskTypeCommand command, CancellationToken cancellationToken)
        {
            await taskTypeCommandRepository.DeleteTaskTypeAsync(command, cancellationToken);

            return Response.Ok();
        }
    }
}
