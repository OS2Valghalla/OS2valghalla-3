using FluentValidation;
using Valghalla.Application.Abstractions.Messaging;
using Valghalla.External.Application.Modules.Unprotected.Interfaces;

namespace Valghalla.External.Application.Modules.Unprotected.Queries
{
    public sealed record GetTeamByTeamLinkQuery(string HashValue) : IQuery<Response>;

    public sealed class GetTeamByTeamLinkQueryValidator : AbstractValidator<GetTeamByTeamLinkQuery>
    {
        public GetTeamByTeamLinkQueryValidator()
        {
            RuleFor(x => x.HashValue)
                .NotEmpty();
        }
    }

    internal sealed class GetTeamByTeamLinkQueryHandler : IQueryHandler<GetTeamByTeamLinkQuery>
    {
        private readonly IUnprotectedTeamQueryRepository unprotectedTeamQueryRepository;
        public GetTeamByTeamLinkQueryHandler(IUnprotectedTeamQueryRepository unprotectedTeamQueryRepository)
        {
            this.unprotectedTeamQueryRepository = unprotectedTeamQueryRepository;
        }

        public async Task<Response> Handle(GetTeamByTeamLinkQuery query, CancellationToken cancellationToken)
        {
            var result = await unprotectedTeamQueryRepository.GetTeamByTeamLinkAsync(query, cancellationToken);
            return Response.Ok(result);
        }
    }
}
