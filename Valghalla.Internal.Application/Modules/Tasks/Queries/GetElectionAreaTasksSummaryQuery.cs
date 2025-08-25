using FluentValidation;

using Valghalla.Application.Abstractions.Messaging;
using Valghalla.Internal.Application.Modules.Tasks.Interfaces;

namespace Valghalla.Internal.Application.Modules.Tasks.Queries
{
    public sealed record GetElectionAreaTasksSummaryQuery(
        Guid ElectionId,
        IList<DateTime>? SelectedDates,
        IList<Guid>? SelectedTeamIds) : IQuery<Response>;

    public sealed class GetElectionAreaTasksSummaryQueryValidator : AbstractValidator<GetElectionAreaTasksSummaryQuery>
    {
        public GetElectionAreaTasksSummaryQueryValidator()
        {
            RuleFor(x => x.ElectionId)
                .NotEmpty();
        }
    }

    internal sealed class GetElectionAreaTasksSummaryQueryHandler : IQueryHandler<GetElectionAreaTasksSummaryQuery>
    {
        private readonly IElectionAreaTasksQueryRepository electionAreaTasksQueryRepository;

        public GetElectionAreaTasksSummaryQueryHandler(IElectionAreaTasksQueryRepository electionAreaTasksQueryRepository)
        {
            this.electionAreaTasksQueryRepository = electionAreaTasksQueryRepository;
        }

        public async Task<Response> Handle(GetElectionAreaTasksSummaryQuery query, CancellationToken cancellationToken)
        {
            var result = await electionAreaTasksQueryRepository.GetElectionAreaTasksSummaryAsync(query, cancellationToken);
            return Response.Ok(result);
        }
    }
}
