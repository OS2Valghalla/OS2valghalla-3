using FluentValidation;
using Valghalla.Application.Abstractions.Messaging;
using Valghalla.Internal.Application.Modules.Administration.Team.Interfaces;

namespace Valghalla.Internal.Application.Modules.Administration.Team.Queries
{
    public sealed record GetTeamQuery(Guid TeamId) : IQuery<Response>;

    public sealed class GetTeamQueryValidator : AbstractValidator<GetTeamQuery>
    {
        public GetTeamQueryValidator()
        {
            RuleFor(x => x.TeamId)
                .NotEmpty();

        }
    }

    internal sealed class GetTeamQueryHandler : IQueryHandler<GetTeamQuery>
    {
        private readonly ITeamQueryRepository teamQueryRepository;

        public GetTeamQueryHandler(ITeamQueryRepository teamQueryRepository)
        {
            this.teamQueryRepository = teamQueryRepository;
        }

        public async Task<Response> Handle(GetTeamQuery query, CancellationToken cancellationToken)
        {
            var result = await teamQueryRepository.GetTeamAsync(query, cancellationToken);
            return Response.Ok(result);
        }
    }
}
