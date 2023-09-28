using FluentValidation;
using Valghalla.Application.Abstractions.Messaging;
using Valghalla.Application.User;
using Valghalla.External.Application.Modules.Team.Interfaces;

namespace Valghalla.External.Application.Modules.Team.Queries
{
    public sealed record GetMyTeamsQuery() : IQuery<Response>;

    public sealed class GetMyTeamsQueryValidator : AbstractValidator<GetMyTeamsQuery>
    {
        public GetMyTeamsQueryValidator()
        {
        }
    }

    internal class GetMyTeamsQueryHandler : IQueryHandler<GetMyTeamsQuery>
    {
        private readonly IUserContextProvider userContextProvider;
        private readonly ITeamQueryRepository teamQueryRepository;

        public GetMyTeamsQueryHandler(IUserContextProvider userContextProvider, ITeamQueryRepository teamQueryRepository)
        {
            this.userContextProvider = userContextProvider;
            this.teamQueryRepository = teamQueryRepository;
        }

        public async Task<Response> Handle(GetMyTeamsQuery query, CancellationToken cancellationToken)
        {
            var participantId = userContextProvider.CurrentUser.ParticipantId!.Value;
            var result = await teamQueryRepository.GetMyTeamsAsync(participantId, cancellationToken);

            return Response.Ok(result);
        }
    }
}
