using FluentValidation;
using Valghalla.Application.Abstractions.Messaging;
using Valghalla.Internal.Application.Modules.Administration.Team.Interfaces;

namespace Valghalla.Internal.Application.Modules.Administration.Team.Queries
{
    public sealed record GetAllTeamsQuery() : IQuery<Response>;

    public sealed class GetAllTeamsQueryValidator : AbstractValidator<GetAllTeamsQuery>
    {
        public GetAllTeamsQueryValidator()
        {
        }
    }

    internal sealed class GetAllTeamsQueryHandler : IQueryHandler<GetAllTeamsQuery>
    {
        private readonly ITeamQueryRepository teamQueryRepository;

        public GetAllTeamsQueryHandler(ITeamQueryRepository teamQueryRepository)
        {
            this.teamQueryRepository = teamQueryRepository;
        }

        public async Task<Response> Handle(GetAllTeamsQuery query, CancellationToken cancellationToken)
        {
            var result = await teamQueryRepository.GetAllTeamsAsync(cancellationToken);
            return Response.Ok(result);
        }
    }
}
