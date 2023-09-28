using Valghalla.Internal.Application.Modules.Administration.SpecialDiet.Commands;
using Valghalla.Internal.Application.Modules.Administration.SpecialDiet.Queries;
using Valghalla.Internal.Application.Modules.Administration.SpecialDiet.Responses;

namespace Valghalla.Internal.Application.Modules.Administration.SpecialDiet.Interfaces
{
    public interface ISpecialDietQueryRepository
    {
        Task<bool> CheckIfSpecialDietExistsAsync(CreateSpecialDietCommand command, CancellationToken cancellationToken);
        Task<bool> CheckIfSpecialDietExistsAsync(UpdateSpecialDietCommand command, CancellationToken cancellationToken);
        Task<bool> CheckHasActiveElectionAsync(CancellationToken cancellationToken);
        Task<SpecialDietResponse?> GetSpecialDietAsync(GetSpecialDietQuery query, CancellationToken cancellationToken);
    }
}
