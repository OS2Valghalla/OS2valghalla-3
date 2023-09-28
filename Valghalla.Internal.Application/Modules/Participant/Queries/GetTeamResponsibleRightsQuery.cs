using FluentValidation;
using Valghalla.Application.Abstractions.Messaging;
using Valghalla.Internal.Application.Modules.Participant.Interfaces;

namespace Valghalla.Internal.Application.Modules.Participant.Queries
{
    public sealed record GetTeamResponsibleRightsQuery(Guid Id) : IQuery<Response>;

    public sealed class GetTeamResponsibleRightsQueryValidator : AbstractValidator<GetTeamResponsibleRightsQuery>
    {
        public GetTeamResponsibleRightsQueryValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty();
        }
    }

    internal class GetTeamResponsibleRightsQueryHandler : IQueryHandler<GetTeamResponsibleRightsQuery>
    {
        private readonly IParticipantQueryRepository participantQueryRepository;

        public GetTeamResponsibleRightsQueryHandler(IParticipantQueryRepository participantQueryRepository)
        {
            this.participantQueryRepository = participantQueryRepository;
        }

        public async Task<Response> Handle(GetTeamResponsibleRightsQuery query, CancellationToken cancellationToken)
        {
            var result = await participantQueryRepository.GetTeamResponsibleRightsAsync(query, cancellationToken);
            return Response.Ok(result);
        }
    }
}
