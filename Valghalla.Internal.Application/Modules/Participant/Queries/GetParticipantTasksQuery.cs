using FluentValidation;
using Valghalla.Application.Abstractions.Messaging;
using Valghalla.Internal.Application.Modules.Participant.Interfaces;

namespace Valghalla.Internal.Application.Modules.Participant.Queries
{
    public sealed record GetParticipantTasksQuery(Guid Id) : IQuery<Response>;

    public sealed class GetParticipantTasksQueryValidator : AbstractValidator<GetParticipantTasksQuery>
    {
        public GetParticipantTasksQueryValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty();
        }
    }

    internal class GetParticipantTasksQueryHandler : IQueryHandler<GetParticipantTasksQuery>
    {
        private readonly IParticipantQueryRepository participantQueryRepository;

        public GetParticipantTasksQueryHandler(IParticipantQueryRepository participantQueryRepository)
        {
            this.participantQueryRepository = participantQueryRepository;
        }

        public async Task<Response> Handle(GetParticipantTasksQuery query, CancellationToken cancellationToken)
        {
            var result = await participantQueryRepository.GetParticipantTasksAsync(query, cancellationToken);
            return Response.Ok(result);
        }
    }
}
