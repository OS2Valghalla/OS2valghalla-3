using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Valghalla.Database;
using Valghalla.Database.Entities.Tables;
using Valghalla.Worker.Infrastructure.Modules.Tasks.Responses;

namespace Valghalla.Worker.Infrastructure.Modules.Tasks.Repositories
{
    public interface ITaskAssignmentQueryRepository
    {
        Task<IEnumerable<TaskAssignmentResponse>> GetUnsentInvitationTaskAssignmentsAsync(Guid electionId, CancellationToken cancellationToken);
        Task<IEnumerable<TaskAssignmentResponse>> GetUnsentRegistrationTaskAssignmentsAsync(Guid electionId, CancellationToken cancellationToken);
        Task<IEnumerable<TaskAssignmentResponse>> GetTaskAssignmentsAsync(IEnumerable<Guid> participantIds, IEnumerable<Guid> electionIds, CancellationToken cancellationToken);
    }

    internal class TaskAssignmentQueryRepository : ITaskAssignmentQueryRepository
    {
        private readonly IMapper mapper;
        private readonly IQueryable<TaskAssignmentEntity> taskAssignments;

        public TaskAssignmentQueryRepository(DataContext dataContext, IMapper mapper)
        {
            this.mapper = mapper;
            taskAssignments = dataContext.Set<TaskAssignmentEntity>().AsNoTracking();
        }

        public async Task<IEnumerable<TaskAssignmentResponse>> GetUnsentInvitationTaskAssignmentsAsync(Guid electionId, CancellationToken cancellationToken)
        {
            var entities = await taskAssignments
                .Where(i =>
                    i.ParticipantId != null &&
                    i.InvitationCode != null &&
                    i.InvitationSent == false &&
                    i.Responsed == false)
                .ToArrayAsync(cancellationToken);

            return entities.Select(mapper.Map<TaskAssignmentResponse>);
        }

        public async Task<IEnumerable<TaskAssignmentResponse>> GetUnsentRegistrationTaskAssignmentsAsync(Guid electionId, CancellationToken cancellationToken)
        {
            var entities = await taskAssignments
                .Where(i =>
                    i.ParticipantId != null &&
                    i.RegistrationSent == false &&
                    i.Accepted &&
                    i.Responsed)
                .ToArrayAsync(cancellationToken);

            return entities.Select(mapper.Map<TaskAssignmentResponse>);
        }

        public async Task<IEnumerable<TaskAssignmentResponse>> GetTaskAssignmentsAsync(IEnumerable<Guid> participantIds, IEnumerable<Guid> electionIds, CancellationToken cancellationToken)
        {
            var entities = await taskAssignments
                .Where(i =>
                    i.ParticipantId != null &&
                    participantIds.Contains(i.ParticipantId.Value) &&
                    electionIds.Contains(i.ElectionId))
                .ToArrayAsync(cancellationToken);

            return entities.Select(mapper.Map<TaskAssignmentResponse>);
        }
    }
}
