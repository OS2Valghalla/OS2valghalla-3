using Valghalla.Application.Abstractions.Messaging;
using Valghalla.Internal.Application.Modules.Shared.Election.Interfaces;

namespace Valghalla.Internal.Application.Modules.Shared.Election.Queries
{
    public sealed record GetElectionsSharedQuery() : IQuery<Response>;

    internal class GetElectionsSharedQueryHandler : IQueryHandler<GetElectionsSharedQuery>
    {
        private readonly IElectionSharedQueryRepository electionQueryRepository;

        public GetElectionsSharedQueryHandler(IElectionSharedQueryRepository electionQueryRepository)
        {
            this.electionQueryRepository = electionQueryRepository;
        }

        public async Task<Response> Handle(GetElectionsSharedQuery query, CancellationToken cancellationToken)
        {
            var result = await electionQueryRepository.GetElectionsAsync(query, cancellationToken);
            return Response.Ok(result);
        }
    }
}
