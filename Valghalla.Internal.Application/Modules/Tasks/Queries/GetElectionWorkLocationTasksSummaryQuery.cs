using FluentValidation;
using Valghalla.Application.Abstractions.Messaging;
using Valghalla.Internal.Application.Modules.Tasks.Interfaces;

namespace Valghalla.Internal.Application.Modules.Tasks.Queries
{
    public sealed record GetElectionWorkLocationTasksSummaryQuery(Guid WorkLocationId, Guid ElectionId) : IQuery<Response>;

    public sealed class GetElectionWorkLocationTasksSummaryQueryValidator : AbstractValidator<GetElectionWorkLocationTasksSummaryQuery>
    {
        public GetElectionWorkLocationTasksSummaryQueryValidator()
        {
            RuleFor(x => x.WorkLocationId)
                .NotEmpty();

            RuleFor(x => x.ElectionId)
                .NotEmpty();
        }
    }

    internal sealed class GetElectionWorkLocationTasksSummaryQueryHandler : IQueryHandler<GetElectionWorkLocationTasksSummaryQuery>
    {
        private readonly IElectionWorkLocationTasksQueryRepository electionWorkLocationTasksQueryRepository;

        public GetElectionWorkLocationTasksSummaryQueryHandler(IElectionWorkLocationTasksQueryRepository electionWorkLocationTasksQueryRepository)
        {
            this.electionWorkLocationTasksQueryRepository = electionWorkLocationTasksQueryRepository;
        }

        public async Task<Response> Handle(GetElectionWorkLocationTasksSummaryQuery query, CancellationToken cancellationToken)
        {
            var result = await electionWorkLocationTasksQueryRepository.GetElectionWorkLocationTasksSummaryAsync(query, cancellationToken);
            return Response.Ok(result);
        }
    }
}
