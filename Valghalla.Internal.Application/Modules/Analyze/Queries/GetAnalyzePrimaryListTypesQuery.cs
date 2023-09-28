using Valghalla.Application.Abstractions.Messaging;
using Valghalla.Internal.Application.Modules.Analyze.Interfaces;

namespace Valghalla.Internal.Application.Modules.Analyze.Queries
{
    public sealed record GetAnalyzePrimaryListTypesQuery() : IQuery<Response>;

    internal sealed class GetAnalyzePrimaryListTypesQueryHandler : IQueryHandler<GetAnalyzePrimaryListTypesQuery>
    {
        private readonly IAnalyzeQueryRepository analyzeQueryRepository;

        public GetAnalyzePrimaryListTypesQueryHandler(IAnalyzeQueryRepository analyzeQueryRepository)
        {
            this.analyzeQueryRepository = analyzeQueryRepository;
        }

        public async Task<Response> Handle(GetAnalyzePrimaryListTypesQuery query, CancellationToken cancellationToken)
        {
            var response = analyzeQueryRepository.GetAnalyzePrimaryListTypes();

            return Response.Ok(response);
        }
    }
}
