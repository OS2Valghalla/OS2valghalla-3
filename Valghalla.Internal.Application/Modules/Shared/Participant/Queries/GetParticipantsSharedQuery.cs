using FluentValidation;
using Valghalla.Application.Abstractions.Messaging;
using Valghalla.Internal.Application.Modules.Shared.Participant.Interfaces;

namespace Valghalla.Internal.Application.Modules.Shared.Participant.Queries
{
    public sealed record GetParticipantsSharedQuery : IQuery<Response>
    {
        public IEnumerable<Guid> Values { get; init; } = Enumerable.Empty<Guid>();
    }

    public sealed class GetParticipantsSharedQueryValidator : AbstractValidator<GetParticipantsSharedQuery>
    {
        public GetParticipantsSharedQueryValidator()
        {
            RuleFor(x => x.Values)
                .NotEmpty();
        }
    }

    internal class GetParticipantsSharedQueryHandler : IQueryHandler<GetParticipantsSharedQuery>
    {
        private readonly IParticipantSharedQueryRepository participantSharedQueryRepository;

        public GetParticipantsSharedQueryHandler(IParticipantSharedQueryRepository participantSharedQueryRepository)
        {
            this.participantSharedQueryRepository = participantSharedQueryRepository;
        }

        public async Task<Response> Handle(GetParticipantsSharedQuery query, CancellationToken cancellationToken)
        {
            var result = await participantSharedQueryRepository.GetPariticipantsAsync(query, cancellationToken);
            return Response.Ok(result);
        }
    }
}
