using Valghalla.Internal.Application.Modules.Administration.Link.Queries;
using Valghalla.Internal.Application.Modules.Administration.Link.Responses;

namespace Valghalla.Internal.Application.Modules.Administration.Link.Interfaces
{
    public interface ILinkQueryRepository
    {
        //Task<bool> DoesTeamLinkExist(CreateTeamLinkCommand command, CancellationToken cancellationToken);
        Task<LinkResponse?> GetTaskLinkAsync(GetTaskLinkQuery queryTaskLink, CancellationToken cancellationToken);
        Task<LinkResponse?> GetTasksFilteredLinkAsync(GetTasksFilteredLinkQuery queryTasksFiltered, CancellationToken cancellationToken);
    }
}
