using Valghalla.External.Application.Modules.Shared.SpecialDiet.Queries;
using Valghalla.External.Application.Modules.Shared.SpecialDiet.Responses;

namespace Valghalla.External.Application.Modules.Shared.SpecialDiet.Interfaces
{
    public interface ISpecialDietSharedQueryRepository
    {
        Task<IEnumerable<SpecialDietSharedResponse>> GetSpecialDietsAsync(GetSpecialDietsSharedQuery query, CancellationToken cancellationToken);
    }
}
