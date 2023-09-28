using Valghalla.External.Application.Modules.Shared.WorkLocation.Responses;
using Valghalla.External.Application.Modules.WorkLocation.Responses;

namespace Valghalla.External.Application.Modules.WorkLocation.Interfaces
{
    public interface IWorkLocationQueryRepository
    {
        Task<IList<WorkLocationSharedResponse>> GetMyWorkLocationsAsync(Guid participantId, CancellationToken cancellationToken);
        Task<IList<DateTime>> GetWorkLocationDatesAsync(Guid workLocationId, Guid participantId, CancellationToken cancellationToken);
        Task<WorkLocationDetailsResponse> GetWorkLocationDetailsAsync(Guid workLocationId, DateTime taskDate, Guid participantId, CancellationToken cancellationToken);
    }
}
