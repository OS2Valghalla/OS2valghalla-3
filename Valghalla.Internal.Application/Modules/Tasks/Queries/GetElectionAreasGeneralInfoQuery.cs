using FluentValidation;
using Valghalla.Application.Abstractions.Messaging;
using Valghalla.Internal.Application.Modules.Tasks.Interfaces;

namespace Valghalla.Internal.Application.Modules.Tasks.Queries
{
    public sealed record GetElectionAreasGeneralInfoQuery(Guid ElectionId) : IQuery<Response>;

    public sealed class GetElectionAreasGeneralInfoQueryValidator : AbstractValidator<GetElectionAreasGeneralInfoQuery>
    {
        public GetElectionAreasGeneralInfoQueryValidator()
        {
            RuleFor(x => x.ElectionId)
                .NotEmpty();
        }
    }

    internal sealed class GetElectionAreasGeneralInfoQueryHandler : IQueryHandler<GetElectionAreasGeneralInfoQuery>
    {
        private readonly IElectionAreaTasksQueryRepository electionAreaTasksQueryRepository;

        public GetElectionAreasGeneralInfoQueryHandler(IElectionAreaTasksQueryRepository electionAreaTasksQueryRepository)
        {
            this.electionAreaTasksQueryRepository = electionAreaTasksQueryRepository;
        }

        public async Task<Response> Handle(GetElectionAreasGeneralInfoQuery query, CancellationToken cancellationToken)
        {
            var result = await electionAreaTasksQueryRepository.GetElectionAreasGeneralInfoAsync(query, cancellationToken);
            return Response.Ok(result);
        }
    }
}
