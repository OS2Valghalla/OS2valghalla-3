using Valghalla.Internal.Application.Modules.Shared.TaskType.Queries;
using Valghalla.Internal.Application.Modules.Shared.TaskType.Responses;

namespace Valghalla.Internal.Application.Modules.Shared.TaskType.Interfaces
{
    public interface ITaskTypeSharedQueryRepository
    {
        Task<IEnumerable<TaskTypeSharedResponse>> GetTaskTypesAsync(GetTaskTypesSharedQuery query, CancellationToken cancellationToken);
    }
}
