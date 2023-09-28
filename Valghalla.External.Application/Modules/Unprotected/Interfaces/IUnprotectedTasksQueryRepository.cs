using Valghalla.External.Application.Modules.Unprotected.Queries;
using Valghalla.External.Application.Modules.Unprotected.Responses;

namespace Valghalla.External.Application.Modules.Unprotected.Interfaces
{
    public interface IUnprotectedTasksQueryRepository
    {
        Task<TasksFiltersOptionsResponse> GetTasksFiltersOptionsAsync(GetTasksFiltersOptionsQuery query, CancellationToken cancellationToken);
        Task<IList<UnprotectedAvailableTasksDetailsResponse>> GetAvailableTasksByFiltersAsync(GetUnprotectedAvailableTasksByFiltersQuery query, CancellationToken cancellationToken);
    }
}
