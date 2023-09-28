using Valghalla.Application.Abstractions.Messaging;
using Valghalla.Internal.Application.Modules.Shared.Communication.Interfaces;

namespace Valghalla.Internal.Application.Modules.Shared.Communication.Queries
{
    public sealed record GetCommunicationTemplatesSharedQuery() : IQuery<Response>;

    internal class GetCommunicationTemplatesSharedQueryHandler : IQueryHandler<GetCommunicationTemplatesSharedQuery>
    {
        private readonly ICommunicationSharedQueryRepository communicationSharedQueryRepository;

        public GetCommunicationTemplatesSharedQueryHandler(ICommunicationSharedQueryRepository communicationSharedQueryRepository)
        {
            this.communicationSharedQueryRepository = communicationSharedQueryRepository;
        }

        public async Task<Response> Handle(GetCommunicationTemplatesSharedQuery query, CancellationToken cancellationToken)
        {
            var result = await communicationSharedQueryRepository.GetCommunicationTemplatesAsync(query, cancellationToken);
            return Response.Ok(result);
        }
    }
}
