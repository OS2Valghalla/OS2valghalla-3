using FluentValidation;
using Valghalla.Application.Abstractions.Messaging;
using Valghalla.Internal.Application.Modules.Tasks.Interfaces;

namespace Valghalla.Internal.Application.Modules.Tasks.Queries
{
    public sealed record GetTaskAssignmentQuery(Guid TaskAssignmentId, Guid ElectionId) : IQuery<Response>;

    public sealed class GetTaskAssignmentQueryValidator : AbstractValidator<GetTaskAssignmentQuery>
    {
        public GetTaskAssignmentQueryValidator()
        {
            RuleFor(x => x.TaskAssignmentId)
                .NotEmpty();

            RuleFor(x => x.ElectionId)
                .NotEmpty();
        }
    }

    internal sealed class GetTaskAssignmentQueryHandler : IQueryHandler<GetTaskAssignmentQuery>
    {
        private readonly IElectionWorkLocationTasksQueryRepository electionWorkLocationTasksQueryRepository;

        public GetTaskAssignmentQueryHandler(IElectionWorkLocationTasksQueryRepository electionWorkLocationTasksQueryRepository)
        {
            this.electionWorkLocationTasksQueryRepository = electionWorkLocationTasksQueryRepository;
        }

        public async Task<Response> Handle(GetTaskAssignmentQuery query, CancellationToken cancellationToken)
        {
            var result = await electionWorkLocationTasksQueryRepository.GetTaskAssignmentAsync(query, cancellationToken);
            return Response.Ok(result);
        }
    }
}
