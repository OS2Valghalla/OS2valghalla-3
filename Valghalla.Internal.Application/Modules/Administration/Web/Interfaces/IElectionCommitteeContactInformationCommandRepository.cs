using Valghalla.Internal.Application.Modules.Administration.Web.Commands;

namespace Valghalla.Internal.Application.Modules.Administration.Web.Interfaces
{
    public interface IElectionCommitteeContactInformationCommandRepository
    {
        Task<bool> UpdateWebPageAsync(string pageName, UpdateElectionCommitteeContactInformationPageCommand command, CancellationToken cancellationToken);
    }
}