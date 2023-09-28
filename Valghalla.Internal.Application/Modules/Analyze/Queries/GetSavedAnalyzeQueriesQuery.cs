using FluentValidation;
using Valghalla.Application.Abstractions.Messaging;
using Valghalla.Internal.Application.Modules.Analyze.Interfaces;

namespace Valghalla.Internal.Application.Modules.Analyze.Queries
{
    public sealed record GetSavedAnalyzeQueriesQuery(Guid UserId) : IQuery<Response>;

    public sealed class GetSavedAnalyzeQueriesQueryValidator : AbstractValidator<GetSavedAnalyzeQueriesQuery>
    {
        public GetSavedAnalyzeQueriesQueryValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty();
        }
    }

    internal sealed class GetSavedAnalyzeQueriesQueryHandler : IQueryHandler<GetSavedAnalyzeQueriesQuery>
    {
        private readonly IAnalyzeQueryRepository analyzeQueryRepository;

        public GetSavedAnalyzeQueriesQueryHandler(IAnalyzeQueryRepository analyzeQueryRepository)
        {
            this.analyzeQueryRepository = analyzeQueryRepository;
        }

        public async Task<Response> Handle(GetSavedAnalyzeQueriesQuery query, CancellationToken cancellationToken)
        {
            var response = analyzeQueryRepository.GetSavedQueriesByUserId(query.UserId);

            return Response.Ok(response);
        }
    }
}
