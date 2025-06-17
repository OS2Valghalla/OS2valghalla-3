using Microsoft.EntityFrameworkCore;

using Valghalla.Database;
using Valghalla.Database.Entities.Tables;
using Valghalla.Internal.Application.Modules.Administration.TaskTypeTemplate.Commands;
using Valghalla.Internal.Application.Modules.Administration.TaskTypeTemplate.Interfaces;

namespace Valghalla.Internal.Infrastructure.Modules.Administration.TaskTypeTemplate
{
    internal class TaskTypeTemplateCommandRepository : ITaskTypeTemplateCommandRepository
    {
        private readonly DataContext dataContext;
        private readonly DbSet<TaskTypeTemplateEntity> TaskTypeTemplates;
        private readonly DbSet<TaskTypeTemplateFileEntity> TaskTypeTemplateFiles;
        private readonly DbSet<TaskAssignmentEntity> taskAssignments;
        private readonly DbSet<WorkLocationTaskTypeEntity> workLocationTaskTypeTemplates;
        private readonly DbSet<ElectionTaskTypeCommunicationTemplateEntity> electionTaskTypeTemplateCommunicationTemplates;


        public TaskTypeTemplateCommandRepository(DataContext dataContext)
        {
            this.dataContext = dataContext;
            TaskTypeTemplates = dataContext.Set<TaskTypeTemplateEntity>();
            TaskTypeTemplateFiles = dataContext.Set<TaskTypeTemplateFileEntity>();
            taskAssignments = dataContext.Set<TaskAssignmentEntity>();
            workLocationTaskTypeTemplates = dataContext.Set<WorkLocationTaskTypeEntity>();
            electionTaskTypeTemplateCommunicationTemplates = dataContext.Set<ElectionTaskTypeCommunicationTemplateEntity>();
        }

        public async Task<Guid> CreateTaskTypeTemplateAsync(CreateTaskTypeTemplateCommand command, CancellationToken cancellationToken)
        {
            var entity = new TaskTypeTemplateEntity()
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

            var TaskTypeTemplateFileEntities = command.FileReferenceIds.Select(fileRefId => new TaskTypeTemplateFileEntity()
            {
                TaskTypeTemplateId = entity.Id,
                FileReferenceId = fileRefId,
            }).ToArray();

            await TaskTypeTemplates.AddAsync(entity, cancellationToken);
            await TaskTypeTemplateFiles.AddRangeAsync(TaskTypeTemplateFileEntities, cancellationToken);
            await dataContext.SaveChangesAsync(cancellationToken);

            return entity.Id;
        }

        public async Task DeleteTaskTypeTemplateAsync(DeleteTaskTypeTemplateCommand command, CancellationToken cancellationToken)
        {
            var entity = await TaskTypeTemplates
                .Include(i => i.WorkLocationTaskTypes)
                .Include(i => i.TaskTypeFileTemplates)
                .Include(i => i.TaskAssignments)
                .Include(i => i.ElectionTaskTypeCommunicationTemplates)
                .Where(i => i.Id == command.Id)
                .SingleAsync(cancellationToken);

            TaskTypeTemplateFiles.RemoveRange(entity.TaskTypeFileTemplates);
            taskAssignments.RemoveRange(entity.TaskAssignments);
            electionTaskTypeTemplateCommunicationTemplates.RemoveRange(entity.ElectionTaskTypeCommunicationTemplates);
            workLocationTaskTypeTemplates.RemoveRange(entity.WorkLocationTaskTypes);
            TaskTypeTemplates.Remove(entity);

            await dataContext.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateTaskTypeTemplateAsync(UpdateTaskTypeTemplateCommand command, CancellationToken cancellationToken)
        {

            var entity = await TaskTypeTemplates
                .Where(i => i.Id == command.Id)
                .SingleAsync(cancellationToken);

            var TaskTypeTemplateFileEntities = await TaskTypeTemplateFiles
                .Where(i => i.TaskTypeTemplateId == command.Id)
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

            var TaskTypeTemplateFileEntitiesToAdd = command.FileReferenceIds
                .Where(fileRefId => !TaskTypeTemplateFileEntities.Any(i => i.FileReferenceId == fileRefId))
                .Select(fileRefId => new TaskTypeTemplateFileEntity()
                {
                    TaskTypeTemplateId = entity.Id,
                    FileReferenceId = fileRefId,
                });

            var TaskTypeTemplateFileEntitiesToRemove = TaskTypeTemplateFileEntities
                .Where(i => !command.FileReferenceIds.Any(fileRefId => fileRefId == i.FileReferenceId));

            TaskTypeTemplates.Update(entity);

            if (TaskTypeTemplateFileEntitiesToRemove.Any())
            {
                TaskTypeTemplateFiles.RemoveRange(TaskTypeTemplateFileEntitiesToRemove);
            }

            if (TaskTypeTemplateFileEntitiesToAdd.Any())
            {
                await TaskTypeTemplateFiles.AddRangeAsync(TaskTypeTemplateFileEntitiesToAdd, cancellationToken);
            }

            await dataContext.SaveChangesAsync(cancellationToken);
        }

    }
}
