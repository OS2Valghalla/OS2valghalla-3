using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Valghalla.Application.TaskValidation;
using Valghalla.Database;
using Valghalla.Database.Entities.Tables;

namespace Valghalla.Infrastructure.TaskValidation
{
    internal class TaskValidationRepository : ITaskValidationRepository
    {
        private readonly IMapper mapper;
        private readonly IQueryable<ParticipantEntity> participants;
        private readonly IQueryable<ElectionEntity> elections;
        private readonly IQueryable<TaskTypeEntity> taskTypes;
        private readonly IQueryable<TaskAssignmentEntity> tasks;

        public TaskValidationRepository(DataContext dataContext, IMapper mapper)
        {
            this.mapper = mapper;
            participants = dataContext.Set<ParticipantEntity>().AsNoTracking();
            elections = dataContext.Set<ElectionEntity>().AsNoTracking();
            taskTypes = dataContext.Set<TaskTypeEntity>().AsNoTracking();
            tasks = dataContext.Set<TaskAssignmentEntity>().AsNoTracking();
        }

        public async Task<EvaluatedParticipant> GetEvaluatedParticipant(Guid participantId, CancellationToken cancellationToken)
        {
            var entity = await participants
                .Where(i => i.Id == participantId)
                .SingleAsync(cancellationToken);

            return mapper.Map<EvaluatedParticipant>(entity);
        }

        public async Task<IEnumerable<TaskValidationRule>> GetValidationRules(Guid electionId, CancellationToken cancellationToken)
        {
            var entities = await elections
                .Include(i => i.ElectionType)
                    .ThenInclude(i => i.ValidationRules)
                .Where(i => i.Id == electionId)
                .Select(i => i.ElectionType.ValidationRules)
                .SingleAsync(cancellationToken);

            return entities.Select(i => new TaskValidationRule(i.ValidationRuleId)).ToArray();
        }

        public async Task<EvaluatedTask> GetEvaluatedTask(Guid taskAssignmentId, CancellationToken cancellationToken)
        {
            var task = await tasks
                .Where(i => i.Id == taskAssignmentId)
                .SingleAsync(cancellationToken);

            var entity = await taskTypes
                .Where(i => i.Id == task.TaskTypeId)
                .SingleAsync(cancellationToken);

            return new EvaluatedTask()
            {
                TaskAssignmentId = taskAssignmentId,
                TaskTypeId = entity.Id,
                TaskDate = task.TaskDate,
                ValidationNotRequired = entity.ValidationNotRequired
            };
        }
    }
}
