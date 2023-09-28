using FluentValidation;
using Valghalla.Application.Abstractions.Messaging;
using Valghalla.Internal.Application.Modules.Analyze.Interfaces;

namespace Valghalla.Internal.Application.Modules.Analyze.Queries
{
    public sealed record GetSavedAnalyzeQueryDetailQuery(int QueryId) : IQuery<Response>;

    public sealed class GetSavedAnalyzeQueryDetailQueryValidator : AbstractValidator<GetSavedAnalyzeQueryDetailQuery>
    {
        public GetSavedAnalyzeQueryDetailQueryValidator()
        {
            RuleFor(x => x.QueryId)
                .GreaterThan(0);
        }
    }

    internal sealed class GetSavedAnalyzeQueryDetailQueryHandler : IQueryHandler<GetSavedAnalyzeQueryDetailQuery>
    {
        private readonly IAnalyzeQueryRepository analyzeQueryRepository;

        public GetSavedAnalyzeQueryDetailQueryHandler(IAnalyzeQueryRepository analyzeQueryRepository)
        {
            this.analyzeQueryRepository = analyzeQueryRepository;
        }

        public async Task<Response> Handle(GetSavedAnalyzeQueryDetailQuery query, CancellationToken cancellationToken)
        {
            var response = await analyzeQueryRepository.GetSavedQueryDetail(query.QueryId, cancellationToken);

            if (response == null) { return Response.FailWithItemNotFoundError(); }

            return Response.Ok(response);
        }
    }
}
