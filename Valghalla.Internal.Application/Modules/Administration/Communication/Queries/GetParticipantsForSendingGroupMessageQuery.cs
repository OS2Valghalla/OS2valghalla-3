using FluentValidation;
using Valghalla.Application.Abstractions.Messaging;
using Valghalla.Internal.Application.Modules.Administration.Communication.Interfaces;

namespace Valghalla.Internal.Application.Modules.Administration.Communication.Queries
{
    public sealed record GetParticipantsForSendingGroupMessageQuery(Guid ElectionId, List<Guid>? WorkLocationIds, List<Guid>? TeamIds, List<Guid>? TaskTypeIds, List<Valghalla.Application.Enums.TaskStatus>? TaskStatuses, List<DateTime>? TaskDates) : IQuery<Response>;

    public sealed class GetParticipantsForSendingGroupMessageQueryValidator : AbstractValidator<GetParticipantsForSendingGroupMessageQuery>
    {
        public GetParticipantsForSendingGroupMessageQueryValidator()
        {
            RuleFor(x => x.ElectionId)
                .NotEmpty();
        }
    }

    internal sealed class GetParticipantsForSendingGroupMessageQueryHandler : IQueryHandler<GetParticipantsForSendingGroupMessageQuery>
    {
        private readonly ICommunicationQueryRepository communicationQueryRepository;

        public GetParticipantsForSendingGroupMessageQueryHandler(ICommunicationQueryRepository communicationQueryRepository)
        {
            this.communicationQueryRepository = communicationQueryRepository;
        }

        public async Task<Response> Handle(GetParticipantsForSendingGroupMessageQuery query, CancellationToken cancellationToken)
        {
            var result = await communicationQueryRepository.GetParticipantsForSendingGroupMessageAsync(query, cancellationToken);
            return Response.Ok(result);
        }
    }
}
