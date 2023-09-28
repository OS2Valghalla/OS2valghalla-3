using Valghalla.Internal.Application.Modules.Shared.SpecialDiet.Queries;
using Valghalla.Internal.Application.Modules.Shared.SpecialDiet.Responses;

namespace Valghalla.Internal.Application.Modules.Shared.SpecialDiet.Interfaces
{
    public interface ISpecialDietSharedQueryRepository
    {
        Task<IEnumerable<SpecialDietSharedResponse>> GetSpecialDietsAsync(GetSpecialDietsSharedQuery query, CancellationToken cancellationToken);
    }
}
