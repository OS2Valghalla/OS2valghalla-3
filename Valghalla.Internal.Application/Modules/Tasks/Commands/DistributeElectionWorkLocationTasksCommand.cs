using FluentValidation;
using Valghalla.Application.Abstractions.Messaging;
using Valghalla.Internal.Application.Modules.Tasks.Interfaces;
using Valghalla.Internal.Application.Modules.Tasks.Requests;

namespace Valghalla.Internal.Application.Modules.Tasks.Commands
{
    public sealed record DistributeElectionWorkLocationTasksCommand() : ICommand<Response>
    {
        public Guid WorkLocationId { get; set; }
        public Guid ElectionId { get; set; }
        public IList<TasksDistributingRequest> DistributingTasks { get; set; } = new List<TasksDistributingRequest>();
    }

    public sealed class DistributeElectionWorkLocationTasksCommandValidator : AbstractValidator<DistributeElectionWorkLocationTasksCommand>
    {
        public DistributeElectionWorkLocationTasksCommandValidator(IElectionWorkLocationTasksQueryRepository electionWorkLocationTasksQueryRepository)
        {
            RuleFor(x => x.WorkLocationId)
                .NotEmpty();

            RuleFor(x => x.ElectionId)
                .NotEmpty();

            RuleFor(x => x)
               .Must((command) => electionWorkLocationTasksQueryRepository.CheckIfWorkLocationInElectionAsync(command.WorkLocationId, command.ElectionId, default).Result)
               .WithMessage("tasks.error.invalid_work_location");
        }
    }

    internal class DistributeElectionWorkLocationTasksCommandHandler : ICommandHandler<DistributeElectionWorkLocationTasksCommand, Response>
    {
        private readonly IElectionWorkLocationTasksCommandRepository electionWorkLocationTasksCommandRepository;

        public DistributeElectionWorkLocationTasksCommandHandler(IElectionWorkLocationTasksCommandRepository electionWorkLocationTasksCommandRepository)
        {
            this.electionWorkLocationTasksCommandRepository = electionWorkLocationTasksCommandRepository;
        }

        public async Task<Response> Handle(DistributeElectionWorkLocationTasksCommand command, CancellationToken cancellationToken)
        {
            await electionWorkLocationTasksCommandRepository.DistributeElectionWorkLocationTasksAsync(command, cancellationToken);
            return Response.Ok();
        }
    }
}
