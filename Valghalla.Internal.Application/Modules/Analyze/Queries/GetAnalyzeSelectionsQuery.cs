using FluentValidation;
using Valghalla.Application.Abstractions.Messaging;
using Valghalla.Internal.Application.Modules.Analyze.Interfaces;

namespace Valghalla.Internal.Application.Modules.Analyze.Queries
{
    public sealed record GetAnalyzeSelectionsQuery(int ListTypeId) : IQuery<Response>;

    public sealed class GetAnalyzeSelectionsQueryValidator : AbstractValidator<GetAnalyzeSelectionsQuery>
    {
        public GetAnalyzeSelectionsQueryValidator()
        {
            RuleFor(x => x.ListTypeId)
                .GreaterThan(0);
        }
    }

    internal sealed class GetAnalyzeSelectionsQueryHandler : IQueryHandler<GetAnalyzeSelectionsQuery>
    {
        private readonly IAnalyzeQueryRepository analyzeQueryRepository;

        public GetAnalyzeSelectionsQueryHandler(IAnalyzeQueryRepository analyzeQueryRepository)
        {
            this.analyzeQueryRepository = analyzeQueryRepository;
        }

        public async Task<Response> Handle(GetAnalyzeSelectionsQuery query, CancellationToken cancellationToken)
        {
            var response = await analyzeQueryRepository.GetAnalyzeSelections(query.ListTypeId, cancellationToken);

            if (response == null) return Response.FailWithItemNotFoundError();

            return Response.Ok(response);
        }
    }
}
