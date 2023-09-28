using Valghalla.Application.Abstractions.Messaging;
using Valghalla.Internal.Application.Modules.Shared.ElectionType.Interfaces;

namespace Valghalla.Internal.Application.Modules.Shared.ElectionType.Queries
{
    public sealed record GetElectionTypesSharedQuery(): IQuery<Response>;

    internal class GetElectionTypesSharedQueryHandler : IQueryHandler<GetElectionTypesSharedQuery>
    {
        private readonly IElectionTypeSharedQueryRepository electionTypeSharedQueryRepository;

        public GetElectionTypesSharedQueryHandler(IElectionTypeSharedQueryRepository electionTypeSharedQueryRepository)
        {
            this.electionTypeSharedQueryRepository = electionTypeSharedQueryRepository;
        }

        public async Task<Response> Handle(GetElectionTypesSharedQuery query, CancellationToken cancellationToken)
        {
            var result = await electionTypeSharedQueryRepository.GetElectionTypesAsync(query, cancellationToken);
            return Response.Ok(result);
        }
    }
}
