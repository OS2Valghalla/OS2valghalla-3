
using Valghalla.Internal.Application.Modules.Administration.SpecialDiet.Commands;

namespace Valghalla.Internal.Application.Modules.Administration.SpecialDiet.Interfaces
{
    public interface ISpecialDietCommandRepository
    {
        Task<Guid> CreateSpecialDietAsync(CreateSpecialDietCommand command, CancellationToken cancellationToken);
        Task UpdateSpecialDietAsync(UpdateSpecialDietCommand command, CancellationToken cancellationToken);
        Task DeleteSpecialDietAsync(DeleteSpecialDietCommand command, CancellationToken cancellationToken);
    }
}
