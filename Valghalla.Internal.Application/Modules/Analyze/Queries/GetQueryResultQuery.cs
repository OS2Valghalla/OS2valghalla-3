using FluentValidation;
using System.Text.Json;
using Valghalla.Application.Abstractions.Messaging;
using Valghalla.Internal.Application.Modules.Analyze.Interfaces;

namespace Valghalla.Internal.Application.Modules.Analyze.Queries
{
    public sealed record GetQueryResultQuery(Guid ElectionId, int QueryId, int Skip, int Take = 10) : IQuery<Response>;

    public sealed class GetQueryResultQueryValidator : AbstractValidator<GetQueryResultQuery>
    {
        public GetQueryResultQueryValidator()
        {
            RuleFor(x => x.ElectionId)
                .NotEmpty();

            RuleFor(x => x.QueryId)
                .GreaterThan(0);

            RuleFor(x => x.Skip)
                .GreaterThan(-1);

            RuleFor(x => x.Take)
                .GreaterThan(0);
        }
    }

    internal sealed class GetQueryResultQueryHandler : IQueryHandler<GetQueryResultQuery>
    {
        private readonly IAnalyzeQueryRepository analyzeQueryRepository;

        public GetQueryResultQueryHandler(IAnalyzeQueryRepository analyzeQueryRepository)
        {
            this.analyzeQueryRepository = analyzeQueryRepository;
        }

        public async Task<Response> Handle(GetQueryResultQuery query, CancellationToken cancellationToken)
        {
            var queryResult = await analyzeQueryRepository.GetQueryResult(query.ElectionId, query.QueryId, query.Skip, query.Take);
            var jsonResult = JsonSerializer.Serialize(queryResult);

            return Response.Ok(jsonResult);
        }
    }
}
