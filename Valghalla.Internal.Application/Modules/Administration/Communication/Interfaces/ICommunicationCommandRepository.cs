using Valghalla.Internal.Application.Modules.Administration.Communication.Commands;

namespace Valghalla.Internal.Application.Modules.Administration.Communication.Interfaces
{
    public interface ICommunicationCommandRepository
    {
        Task<Guid> CreateCommunicationTemplateAsync(CreateCommunicationTemplateCommand command, CancellationToken cancellationToken);
        Task UpdateCommunicationTemplateAsync(UpdateCommunicationTemplateCommand command, CancellationToken cancellationToken);
        Task DeleteCommunicationTemplateAsync(DeleteCommunicationTemplateCommand command, CancellationToken cancellationToken);
    }
}
