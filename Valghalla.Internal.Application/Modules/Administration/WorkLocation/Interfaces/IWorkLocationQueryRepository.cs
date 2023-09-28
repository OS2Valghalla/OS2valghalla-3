using Valghalla.Internal.Application.Modules.Administration.WorkLocation.Commands;
using Valghalla.Internal.Application.Modules.Administration.WorkLocation.Queries;
using Valghalla.Internal.Application.Modules.Administration.WorkLocation.Responses;

namespace Valghalla.Internal.Application.Modules.Administration.WorkLocation.Interfaces
{
    public interface IWorkLocationQueryRepository
    {
        Task<bool> CheckIfWorkLocationExistsAsync(CreateWorkLocationCommand command, CancellationToken cancellationToken);
        Task<bool> CheckIfWorkLocationExistsAsync(UpdateWorkLocationCommand command, CancellationToken cancellationToken);
        Task<bool> CheckIfWorkLocationUsedInActiveElectionAsync(Guid id, CancellationToken cancellationToken);
        Task<bool> CheckIfWorkLocationHasTasksAsync(DeleteWorkLocationCommand command, CancellationToken cancellationToken);
        Task<WorkLocationDetailResponse?> GetWorkLocationAsync(GetWorkLocationQuery query, CancellationToken cancellationToken);
        Task<List<WorkLocationResponsibleResponse>> GetWorkLocationResponsiblesAsync(GetWorkLocationResponsibleParticipantsQuery query, CancellationToken cancellationToken);
    }
}
