using Valghalla.Application.Abstractions.Messaging;
using Valghalla.Internal.Application.Modules.Shared.SpecialDiet.Interfaces;

namespace Valghalla.Internal.Application.Modules.Shared.SpecialDiet.Queries
{
    public sealed record GetSpecialDietsSharedQuery() : IQuery<Response>;

    internal class GetSpecialDietsSharedQueryHandler : IQueryHandler<GetSpecialDietsSharedQuery>
    {
        private readonly ISpecialDietSharedQueryRepository specialDietSharedQueryRepository;

        public GetSpecialDietsSharedQueryHandler(ISpecialDietSharedQueryRepository specialDietSharedQueryRepository)
        {
            this.specialDietSharedQueryRepository = specialDietSharedQueryRepository;
        }

        public async Task<Response> Handle(GetSpecialDietsSharedQuery query, CancellationToken cancellationToken)
        {
            var result = await specialDietSharedQueryRepository.GetSpecialDietsAsync(query, cancellationToken);
            return Response.Ok(result);
        }
    }
}
