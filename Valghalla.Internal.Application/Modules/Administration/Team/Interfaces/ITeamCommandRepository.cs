using Valghalla.Internal.Application.Modules.Administration.Team.Commands;
using Valghalla.Internal.Application.Modules.Team.Commands;

namespace Valghalla.Internal.Application.Modules.Administration.Team.Interfaces
{
    public interface ITeamCommandRepository
    {
        Task<string> CreateTeamLinkAsync(CreateTeamLinkCommand command, CancellationToken cancellationToken);
        Task<Guid> CreateTeamAsync(CreateTeamCommand command, CancellationToken cancellationToken);
        Task<(IEnumerable<Guid>, string)> DeleteTeamAsync(DeleteTeamCommand command, CancellationToken cancellationToken);
        Task<(IEnumerable<Guid>, IEnumerable<Guid>)> UpdateTeamAsync(UpdateTeamCommand command, CancellationToken cancellationToken);
    }
}
