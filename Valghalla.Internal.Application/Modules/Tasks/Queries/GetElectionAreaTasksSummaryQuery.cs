using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Valghalla.Application.Abstractions.Messaging;
using Valghalla.Internal.Application.Modules.Tasks.Interfaces;

namespace Valghalla.Internal.Application.Modules.Tasks.Queries
{
    public sealed record GetElectionAreaTasksSummaryQuery(Guid ElectionId, DateTime? SelectedDate, Guid? SelectedTeamId) : IQuery<Response>;

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
