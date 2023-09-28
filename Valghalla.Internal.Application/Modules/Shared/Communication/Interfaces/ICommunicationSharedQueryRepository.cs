using Valghalla.Internal.Application.Modules.Shared.Communication.Queries;
using Valghalla.Internal.Application.Modules.Shared.Communication.Responses;

namespace Valghalla.Internal.Application.Modules.Shared.Communication.Interfaces
{
    public interface ICommunicationSharedQueryRepository
    {
        Task<IEnumerable<CommunicationTemplateSharedResponse>> GetCommunicationTemplatesAsync(GetCommunicationTemplatesSharedQuery query, CancellationToken cancellationToken);
    }
}
