using Valghalla.External.Application.Modules.Team.Commands;
using Valghalla.External.Application.Modules.Team.Responses;

namespace Valghalla.External.Application.Modules.Team.Interfaces
{
    public interface ITeamCommandRepository
    {
        Task<TeamMemberRemovalResponse?> RemoveTeamMemberAsync(Guid teamId, Guid participantId, Guid teamResponsibleId, CancellationToken cancellationToken);
        Task<string> CreateTeamLinkAsync(CreateTeamLinkCommand command, CancellationToken cancellationToken);
    }
}
