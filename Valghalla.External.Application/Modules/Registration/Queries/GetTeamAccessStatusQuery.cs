using FluentValidation;
using Valghalla.Application.Abstractions.Messaging;
using Valghalla.Application.User;
using Valghalla.External.Application.Modules.Registration.Interfaces;
using Valghalla.External.Application.Modules.Registration.Responses;

namespace Valghalla.External.Application.Modules.Registration.Queries
{
    public sealed record GetTeamAccessStatusQuery(string HashValue) : IQuery<Response>;

    public sealed class GetTeamAccessStatusQueryValidator : AbstractValidator<GetTeamAccessStatusQuery>
    {
        public GetTeamAccessStatusQueryValidator(IRegistrationQueryRepository registrationQueryRepository)
        {
            RuleFor(x => x.HashValue)
                .NotEmpty();

            RuleFor(x => x)
                .Must(query => registrationQueryRepository.CheckIfTeamExistsFromLink(query.HashValue, default).Result)
                .WithMessage("registration.error.team_not_exist");
        }
    }

    internal class GetTeamAccessStatusQueryHandler : IQueryHandler<GetTeamAccessStatusQuery>
    {
        private readonly IUserContextProvider userContextProvider;
        private readonly IRegistrationQueryRepository registrationQueryRepository;

        public GetTeamAccessStatusQueryHandler(
            IUserContextProvider userContextProvider,
            IRegistrationQueryRepository registrationQueryRepository)
        {
            this.userContextProvider = userContextProvider;
            this.registrationQueryRepository = registrationQueryRepository;
        }

        public async Task<Response> Handle(GetTeamAccessStatusQuery query, CancellationToken cancellationToken)
        {
            var participantId = userContextProvider.CurrentUser.ParticipantId!.Value;
            var joined = await registrationQueryRepository.CheckIfCurrentUserJoinedTeam(query, participantId, cancellationToken);

            return Response.Ok(new TeamAccessStatusResponse()
            {
                Joined = joined,
            });
        }
    }
}
