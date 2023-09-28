using Valghalla.Application.Communication;
using Valghalla.Internal.Application.Modules.Administration.Communication.Commands;
using Valghalla.Internal.Application.Modules.Administration.Communication.Queries;
using Valghalla.Internal.Application.Modules.Administration.Communication.Responses;

namespace Valghalla.Internal.Application.Modules.Administration.Communication.Interfaces
{
    public interface ICommunicationQueryRepository
    {
        Task<bool> CheckIfCommunicationTemplateExistsAsync(CreateCommunicationTemplateCommand command, CancellationToken cancellationToken);
        Task<bool> CheckIfCommunicationTemplateExistsAsync(UpdateCommunicationTemplateCommand command, CancellationToken cancellationToken);
        Task<bool> CheckIsDefaultCommunicationTemplateAsync(Guid id, CancellationToken cancellationToken);
        Task<bool> CheckIfCommunicationTemplateUsedInElectionAsync(DeleteCommunicationTemplateCommand command, CancellationToken cancellationToken);
        Task<CommunicationTemplateDetailsResponse?> GetCommunicationTemplateAsync(GetCommunicationTemplateQuery query, CancellationToken cancellationToken);
        Task<IList<ParticipantForSendingGroupMessageResponse>> GetParticipantsForSendingGroupMessageAsync(GetParticipantsForSendingGroupMessageQuery query, CancellationToken cancellationToken);
        Task<IList<CommunicationTaskParticipantInfo>> GetTasksForSendingGroupMessageAsync(Guid electionId, IEnumerable<Guid> workLocationIds, IEnumerable<Guid> teamIds, IEnumerable<Guid> taskTypeIds,
            IEnumerable<Valghalla.Application.Enums.TaskStatus> taskStatuses, IEnumerable<DateTime> taskDates, CancellationToken cancellationToken);
    }
}
