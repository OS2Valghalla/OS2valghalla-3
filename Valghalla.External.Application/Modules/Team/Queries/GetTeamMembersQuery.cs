using FluentValidation;
using Valghalla.Application.Abstractions.Messaging;
using Valghalla.Application.User;
using Valghalla.External.Application.Modules.Team.Interfaces;

namespace Valghalla.External.Application.Modules.Team.Queries
{
    public sealed record GetTeamMembersQuery(Guid TeamId) : IQuery<Response>;

    public sealed class GetTeamMembersQueryValidator : AbstractValidator<GetTeamMembersQuery>
    {
        public GetTeamMembersQueryValidator()
        {
            RuleFor(x => x.TeamId)
                .NotEmpty();
        }
    }

    internal class GetTeamMembersQueryHandler : IQueryHandler<GetTeamMembersQuery>
    {
        private readonly IUserContextProvider userContextProvider;
        private readonly ITeamQueryRepository teamQueryRepository;

        public GetTeamMembersQueryHandler(IUserContextProvider userContextProvider, ITeamQueryRepository teamQueryRepository)
        {
            this.userContextProvider = userContextProvider;
            this.teamQueryRepository = teamQueryRepository;
        }

        public async Task<Response> Handle(GetTeamMembersQuery query, CancellationToken cancellationToken)
        {
            var participantId = userContextProvider.CurrentUser.ParticipantId!.Value;
            var result = await teamQueryRepository.GetTeamMembersAsync(query.TeamId, participantId, cancellationToken);

            return Response.Ok(result);
        }
    }
}
