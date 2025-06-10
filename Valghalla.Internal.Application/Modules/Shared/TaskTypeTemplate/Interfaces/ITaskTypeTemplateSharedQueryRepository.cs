using Valghalla.Internal.Application.Modules.Shared.TaskTypeTemplate.Queries;
using Valghalla.Internal.Application.Modules.Shared.TaskTypeTemplate.Responses;

namespace Valghalla.Internal.Application.Modules.Shared.TaskTypeTemplate.Interfaces
{
    public interface ITaskTypeTemplateSharedQueryRepository
    {
        Task<IEnumerable<TaskTypeTemplateSharedResponse>> GetTaskTypeTemplatesAsync(GetTaskTypeTemplatesSharedQuery query, CancellationToken cancellationToken);
    }
}
