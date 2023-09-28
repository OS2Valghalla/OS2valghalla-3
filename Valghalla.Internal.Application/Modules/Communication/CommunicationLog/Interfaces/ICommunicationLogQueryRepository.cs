using Valghalla.Application.Communication;
using Valghalla.Internal.Application.Modules.Communication.CommunicationLog.Queries;

namespace Valghalla.Internal.Application.Modules.Communication.CommunicationLog.Interfaces
{
    public interface ICommunicationLogQueryRepository
    {
        Task<CommunicationLogDetails?> GetCommunicationLogDetailsAsync(GetCommunicationLogDetailsQuery query, CancellationToken cancellationToken);
    }
}
