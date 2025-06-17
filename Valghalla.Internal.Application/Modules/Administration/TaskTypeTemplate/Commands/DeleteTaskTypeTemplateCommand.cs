using FluentValidation;
using Valghalla.Application.Abstractions.Messaging;
using Valghalla.Application.Confirmations;
using Valghalla.Internal.Application.Modules.Administration.TaskTypeTemplate.Interfaces;

namespace Valghalla.Internal.Application.Modules.Administration.TaskTypeTemplate.Commands
{
    public sealed record DeleteTaskTypeTemplateCommand(Guid Id) : ConfirmationCommand<Response>;

    public sealed class DeleteTaskTypeTemplateCommandValidator : AbstractValidator<DeleteTaskTypeTemplateCommand>
    {
        public DeleteTaskTypeTemplateCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty();
        }
    }

    public sealed class DeleteTaskTypeTemplateCommandConfirmator : Confirmator<DeleteTaskTypeTemplateCommand>
    {
        public override string Title => "administration.task_type.delete_task_type_confirmation.title";
        public override string Message => "administration.task_type.delete_task_type_confirmation.content";

        public DeleteTaskTypeTemplateCommandConfirmator(ITaskTypeTemplateQueryRepository repository)
        {
            WhenAsync(repository.CheckIfTaskTypeTemplateHasTasksInActiveElectionAsync)
                .WithMessage("administration.task_type.delete_task_type_confirmation.has_tasks_connected_to_work_location");
        }
    }

    internal class DeleteTaskTypeTemplateCommandHandler : ICommandHandler<DeleteTaskTypeTemplateCommand, Response>
    {
        private readonly ITaskTypeTemplateCommandRepository TaskTypeTemplateCommandRepository;
        public DeleteTaskTypeTemplateCommandHandler(ITaskTypeTemplateCommandRepository TaskTypeTemplateCommandRepository)
        {
            this.TaskTypeTemplateCommandRepository = TaskTypeTemplateCommandRepository;
        }

        public async Task<Response> Handle(DeleteTaskTypeTemplateCommand command, CancellationToken cancellationToken)
        {
            await TaskTypeTemplateCommandRepository.DeleteTaskTypeTemplateAsync(command, cancellationToken);

            return Response.Ok();
        }
    }
}
