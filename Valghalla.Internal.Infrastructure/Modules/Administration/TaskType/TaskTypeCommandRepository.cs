using Microsoft.EntityFrameworkCore;

using Valghalla.Database;
using Valghalla.Database.Entities.Tables;
using Valghalla.Internal.Application.Modules.Administration.TaskType.Commands;
using Valghalla.Internal.Application.Modules.Administration.TaskType.Interfaces;

namespace Valghalla.Internal.Infrastructure.Modules.Administration.TaskType
{
    internal class TaskTypeCommandRepository : ITaskTypeCommandRepository
    {
        private readonly DataContext dataContext;
        private readonly DbSet<TaskTypeEntity> taskTypes;
        private readonly DbSet<TaskTypeFileEntity> taskTypeFiles;
        private readonly DbSet<TaskAssignmentEntity> taskAssignments;
        private readonly DbSet<WorkLocationTaskTypeEntity> workLocationTaskTypes;
        private readonly DbSet<ElectionTaskTypeCommunicationTemplateEntity> electionTaskTypeCommunicationTemplates;


        public TaskTypeCommandRepository(DataContext dataContext)
        {
            this.dataContext = dataContext;
            taskTypes = dataContext.Set<TaskTypeEntity>();
            taskTypeFiles = dataContext.Set<TaskTypeFileEntity>();
            taskAssignments = dataContext.Set<TaskAssignmentEntity>();
            workLocationTaskTypes = dataContext.Set<WorkLocationTaskTypeEntity>();
            electionTaskTypeCommunicationTemplates = dataContext.Set<ElectionTaskTypeCommunicationTemplateEntity>();
        }

        public async Task<Guid> CreateTaskTypeAsync(CreateTaskTypeCommand command, CancellationToken cancellationToken)
        {
            var entity = new TaskTypeEntity()
            {
                Id = Guid.NewGuid(),
                Title = command.Title,
                ShortName = command.ShortName,
                Description = command.Description,
                StartTime = command.StartTime,
                EndTime = command.EndTime,
                Payment = command.Payment,
                ValidationNotRequired = command.ValidationNotRequired,
                Trusted = command.Trusted,
                SendingReminderEnabled = command.SendingReminderEnabled,
            };

            var taskTypeFileEntities = command.FileReferenceIds.Select(fileRefId => new TaskTypeFileEntity()
            {
                TaskTypeId = entity.Id,
                FileReferenceId = fileRefId,
            });

            await taskTypes.AddAsync(entity, cancellationToken);
            await taskTypeFiles.AddRangeAsync(taskTypeFileEntities, cancellationToken);
            await dataContext.SaveChangesAsync(cancellationToken);

            return entity.Id;
        }

        public async Task DeleteTaskTypeAsync(DeleteTaskTypeCommand command, CancellationToken cancellationToken)
        {
            var entity = await taskTypes
                .Include(i => i.WorkLocationTaskTypes)
                .Include(i => i.TaskTypeFiles)
                .Include(i => i.TaskAssignments)
                .Include(i => i.ElectionTaskTypeCommunicationTemplates)
                .Where(i => i.Id == command.Id)
                .SingleAsync(cancellationToken);

            taskTypeFiles.RemoveRange(entity.TaskTypeFiles);
            taskAssignments.RemoveRange(entity.TaskAssignments);
            electionTaskTypeCommunicationTemplates.RemoveRange(entity.ElectionTaskTypeCommunicationTemplates);
            workLocationTaskTypes.RemoveRange(entity.WorkLocationTaskTypes);
            taskTypes.Remove(entity);

            await dataContext.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateTaskTypeAsync(UpdateTaskTypeCommand command, CancellationToken cancellationToken)
        {
            var entity = await taskTypes
                .Where(i => i.Id == command.Id)
                .SingleAsync(cancellationToken);

            var taskTypeFileEntities = await taskTypeFiles
                .Where(i => i.TaskTypeId == command.Id)
                .ToArrayAsync(cancellationToken);

            entity.Title = command.Title;
            entity.ShortName = command.ShortName;
            entity.Description = command.Description;
            entity.StartTime = command.StartTime;
            entity.EndTime = command.EndTime;
            entity.Payment = command.Payment;
            entity.ValidationNotRequired = command.ValidationNotRequired;
            entity.Trusted = command.Trusted;
            entity.SendingReminderEnabled = command.SendingReminderEnabled;

            var taskTypeFileEntitiesToAdd = command.FileReferenceIds
                .Where(fileRefId => !taskTypeFileEntities.Any(i => i.FileReferenceId == fileRefId))
                .Select(fileRefId => new TaskTypeFileEntity()
                {
                    TaskTypeId = entity.Id,
                    FileReferenceId = fileRefId,
                });

            var taskTypeFileEntitiesToRemove = taskTypeFileEntities
                .Where(i => !command.FileReferenceIds.Any(fileRefId => fileRefId == i.FileReferenceId));

            taskTypes.Update(entity);

            if (taskTypeFileEntitiesToRemove.Any())
            {
                taskTypeFiles.RemoveRange(taskTypeFileEntitiesToRemove);
            }

            if (taskTypeFileEntitiesToAdd.Any())
            {
                await taskTypeFiles.AddRangeAsync(taskTypeFileEntitiesToAdd, cancellationToken);
            }

            await dataContext.SaveChangesAsync(cancellationToken);
        }

    }
}
