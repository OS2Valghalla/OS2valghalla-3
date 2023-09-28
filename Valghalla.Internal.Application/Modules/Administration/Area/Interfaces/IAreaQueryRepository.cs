using Valghalla.Internal.Application.Modules.Administration.Area.Commands;
using Valghalla.Internal.Application.Modules.Administration.Area.Queries;
using Valghalla.Internal.Application.Modules.Administration.Area.Responses;

namespace Valghalla.Internal.Application.Modules.Administration.Area.Interfaces
{
    public interface IAreaQueryRepository
    {
        Task<bool> CheckIfAreaExistsAsync(CreateAreaCommand command, CancellationToken cancellationToken);
        Task<bool> CheckIfAreaExistsAsync(UpdateAreaCommand command, CancellationToken cancellationToken);
        Task<bool> CheckIfAreaHasWorkLocationsAsync(DeleteAreaCommand command, CancellationToken cancellationToken);
        Task<bool> CheckIfAreaIsLastOneAsync(DeleteAreaCommand command, CancellationToken cancellationToken);
        Task<AreaDetailsResponse?> GetAreaAsync(GetAreaDetailsQuery query, CancellationToken cancellationToken);
        Task<IList<AreaListingItemResponse>> GetAllAreasAsync(GetAllAreasQuery query, CancellationToken cancellationToken);
    }
}
