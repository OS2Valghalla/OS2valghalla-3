using Valghalla.Internal.Application.Modules.Administration.Area.Commands;

namespace Valghalla.Internal.Application.Modules.Administration.Area.Interfaces
{
    public interface IAreaCommandRepository
    {
        Task<Guid> CreateAreaAsync(CreateAreaCommand command, CancellationToken cancellationToken);
        Task UpdateAreaAsync(UpdateAreaCommand command, CancellationToken cancellationToken);
        Task DeleteAreaAsync(DeleteAreaCommand command, CancellationToken cancellationToken);
    }
}
