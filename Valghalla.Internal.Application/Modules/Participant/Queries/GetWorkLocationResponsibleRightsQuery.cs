using FluentValidation;
using Valghalla.Application.Abstractions.Messaging;
using Valghalla.Internal.Application.Modules.Participant.Interfaces;

namespace Valghalla.Internal.Application.Modules.Participant.Queries
{
    public sealed record GetWorkLocationResponsibleRightsQuery(Guid Id) : IQuery<Response>;

    public sealed class GetWorkLocationResponsibleRightsQueryValidator : AbstractValidator<GetWorkLocationResponsibleRightsQuery>
    {
        public GetWorkLocationResponsibleRightsQueryValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty();
        }
    }

    internal class GetWorkLocationResponsibleRightsQueryHandler : IQueryHandler<GetWorkLocationResponsibleRightsQuery>
    {
        private readonly IParticipantQueryRepository participantQueryRepository;

        public GetWorkLocationResponsibleRightsQueryHandler(IParticipantQueryRepository participantQueryRepository)
        {
            this.participantQueryRepository = participantQueryRepository;
        }

        public async Task<Response> Handle(GetWorkLocationResponsibleRightsQuery query, CancellationToken cancellationToken)
        {
            var result = await participantQueryRepository.GetWorkLocationResponsibleRightsAsync(query, cancellationToken);
            return Response.Ok(result);
        }
    }
}
