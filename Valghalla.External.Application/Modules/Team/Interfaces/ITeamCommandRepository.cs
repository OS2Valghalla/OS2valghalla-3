using Valghalla.External.Application.Modules.Team.Commands;

namespace Valghalla.External.Application.Modules.Team.Interfaces
{
    public interface ITeamCommandRepository
    {
        Task RemoveTeamMemberAsync(Guid teamId, Guid participantId, Guid teamResponsibleId, CancellationToken cancellationToken);
        Task<string> CreateTeamLinkAsync(CreateTeamLinkCommand command, CancellationToken cancellationToken);
    }
}
