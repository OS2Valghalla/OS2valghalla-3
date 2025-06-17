using Valghalla.Internal.Application.Modules.Shared.WorkLocation.Queries;
using Valghalla.Internal.Application.Modules.Shared.WorkLocationTemplate.Queries;
using Valghalla.Internal.Application.Modules.Shared.WorkLocationTemplate.Responses;

namespace Valghalla.Internal.Application.Modules.Shared.WorkLocationTemplate.Interfaces
{
    public interface IWorkLocationTemplateSharedQueryRepository
    {
        Task<IEnumerable<WorkLocationTemplateSharedResponse>> GetWorkLocationTemplatesAsync(GetWorkLocationTemplatesSharedQuery query, CancellationToken cancellationToken);
        Task<WorkLocationTemplateSharedResponse?> GetWorkLocationTemplateAsync(GetWorkLocationTemplateSharedQuery query, CancellationToken cancellationToken);
    }
}
