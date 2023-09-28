using FluentValidation;
using Valghalla.Application.Abstractions.Messaging;
using Valghalla.Internal.Application.Modules.Communication.CommunicationLog.Interfaces;

namespace Valghalla.Internal.Application.Modules.Communication.CommunicationLog.Queries
{
    public sealed record GetCommunicationLogDetailsQuery(Guid Id) : IQuery<Response>;

    public sealed class GetCommunicationLogDetailsQueryValidator : AbstractValidator<GetCommunicationLogDetailsQuery>
    {
        public GetCommunicationLogDetailsQueryValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty();
        }
    }

    internal class GetCommunicationLogDetailsQueryHandler : IQueryHandler<GetCommunicationLogDetailsQuery>
    {
        private readonly ICommunicationLogQueryRepository communicationLogQueryRepository;

        public GetCommunicationLogDetailsQueryHandler(ICommunicationLogQueryRepository communicationLogQueryRepository)
        {
            this.communicationLogQueryRepository = communicationLogQueryRepository;
        }

        public async Task<Response> Handle(GetCommunicationLogDetailsQuery query, CancellationToken cancellationToken)
        {
            var result = await communicationLogQueryRepository.GetCommunicationLogDetailsAsync(query, cancellationToken);
            return Response.Ok(result);
        }
    }
}
