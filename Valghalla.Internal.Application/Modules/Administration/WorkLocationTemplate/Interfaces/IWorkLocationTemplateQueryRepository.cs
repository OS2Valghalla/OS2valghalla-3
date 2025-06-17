using Valghalla.Internal.Application.Modules.Administration.WorkLocation.Commands;
using Valghalla.Internal.Application.Modules.Administration.WorkLocation.Queries;
using Valghalla.Internal.Application.Modules.Administration.WorkLocation.Responses;
using Valghalla.Internal.Application.Modules.Administration.WorkLocationTemplate.Commands;
using Valghalla.Internal.Application.Modules.Administration.WorkLocationTemplate.Queries;
using Valghalla.Internal.Application.Modules.Administration.WorkLocationTemplate.Responses;

namespace Valghalla.Internal.Application.Modules.Administration.WorkLocationTemplate.Interfaces
{
    public interface IWorkLocationTemplateQueryRepository
    {
        Task<bool> CheckIfWorkLocationTemplateExistsAsync(CreateWorkLocationTemplateCommand command, CancellationToken cancellationToken);
        Task<bool> CheckIfWorkLocationTemplateExistsAsync(UpdateWorkLocationTemplateCommand command, CancellationToken cancellationToken);
        Task<WorkLocationTemplateDetailResponse?> GetWorkLocationTemplateAsync(GetWorkLocationTemplateQuery query, CancellationToken cancellationToken);
        Task<List<WorkLocationTemplateDetailResponse>?> GetWorkLocationTemplatesAsync(GetWorkLocationTemplatesQuery query, CancellationToken cancellationToken);
    }
}
