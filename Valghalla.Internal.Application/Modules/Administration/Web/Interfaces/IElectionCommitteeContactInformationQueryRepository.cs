using Valghalla.Application.Web;

namespace Valghalla.Internal.Application.Modules.Administration.Web.Interfaces
{
    public interface IElectionCommitteeContactInformationQueryRepository
    {
        Task<ElectionCommitteeContactInformationPage?> GetWebPageAsync(string pageName, CancellationToken cancellationToken);
    }
}
