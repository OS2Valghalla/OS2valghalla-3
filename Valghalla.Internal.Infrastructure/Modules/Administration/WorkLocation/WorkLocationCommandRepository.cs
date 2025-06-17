using AutoMapper;

using Microsoft.EntityFrameworkCore;

using Valghalla.Database;
using Valghalla.Database.Entities.Tables;
using Valghalla.Internal.Application.Modules.Administration.WorkLocation.Commands;
using Valghalla.Internal.Application.Modules.Administration.WorkLocation.Interfaces;

namespace Valghalla.Internal.Infrastructure.Modules.Administration.WorkLocation
{
    public class WorkLocationCommandRepository : IWorkLocationCommandRepository
    {
        private readonly DataContext dataContext;
        private readonly DbSet<TaskAssignmentEntity> tasks;
        private readonly DbSet<WorkLocationEntity> workLocations;
        private readonly DbSet<WorkLocationTaskTypeEntity> workLocationTaskTypes;
        private readonly DbSet<WorkLocationResponsibleEntity> workLocationResponsibles;
        private readonly DbSet<WorkLocationTeamEntity> workLocationTeams;
        private readonly IQueryable<TaskTypeTemplateEntity> TaskTypeTemplates;
        private readonly DbSet<TaskTypeEntity> taskTypes;
        private readonly DbSet<TaskTypeFileEntity> taskTypeFiles;
        private readonly DbSet<ElectionWorkLocationEntity> electionWorkLocationEntities;
        private readonly IMapper mapper;

        public WorkLocationCommandRepository(DataContext dataContext, IMapper mapper)
        {
            this.dataContext = dataContext;
            tasks = dataContext.Set<TaskAssignmentEntity>();
            workLocations = dataContext.Set<WorkLocationEntity>();
            workLocationTaskTypes = dataContext.Set<WorkLocationTaskTypeEntity>();
            workLocationResponsibles = dataContext.Set<WorkLocationResponsibleEntity>();
            workLocationTeams = dataContext.Set<WorkLocationTeamEntity>();
            TaskTypeTemplates = dataContext.Set<TaskTypeTemplateEntity>().AsNoTracking();
            taskTypes = dataContext.Set<TaskTypeEntity>();
            taskTypeFiles = dataContext.Set<TaskTypeFileEntity>();
            electionWorkLocationEntities = dataContext.Set<ElectionWorkLocationEntity>();
            this.mapper = mapper;
        }

        public async Task<Guid> CreateWorkLocationAsync(CreateWorkLocationCommand command, CancellationToken cancellationToken)
        {
            var workLocationId = Guid.NewGuid();

            var templates = await GetWorkLocationTaskTypeTemplates(command, cancellationToken);

            var taskTypeEntities = new List<TaskTypeEntity>();
            var fileEntities = new List<TaskTypeFileEntity>();

            if (templates != null)
            {
                foreach (var template in templates)
                {
                    var taskTypeId = Guid.NewGuid();

                    var taskTypeEntity = mapper.Map<TaskTypeEntity>(template);
                    taskTypeEntity.Id = taskTypeId;

                    taskTypeEntities.Add(taskTypeEntity);

                    if (template.FileReferences != null)
                    {
                        foreach (var fileRef in template.FileReferences)
                        {
                            fileEntities.Add(new TaskTypeFileEntity
                            {
                                TaskTypeId = taskTypeId,
                                FileReferenceId = fileRef.Id
                            });
                        }
                    }
                }
            }
            var taskTypeIds = await CreateWorkLocationTaskTypes(taskTypeEntities, fileEntities, workLocationId, cancellationToken);

            command.TaskTypeIds.AddRange(taskTypeIds);

            var entity = new WorkLocationEntity()
            {
                Id = workLocationId,
                Title = command.Title,
                AreaId = command.AreaId,
                Address = command.Address,
                PostalCode = command.PostalCode,
                City = command.City,
                VoteLocation = command.VoteLocation
            };

            await workLocations.AddAsync(entity, cancellationToken);
            foreach (var taskTypeId in command.TaskTypeIds)
            {
                var childEntity = new WorkLocationTaskTypeEntity()
                {
                    WorkLocationId = workLocationId,
                    TaskTypeId = taskTypeId
                };
                await workLocationTaskTypes.AddAsync(childEntity, cancellationToken);
            }
            foreach (var teamId in command.TeamIds)
            {
                var childEntity = new WorkLocationTeamEntity()
                {
                    WorkLocationId = workLocationId,
                    TeamId = teamId
                };
                await workLocationTeams.AddAsync(childEntity, cancellationToken);
            }
            foreach (var responsibleId in command.ResponsibleIds)
            {
                var childEntity = new WorkLocationResponsibleEntity()
                {
                    WorkLocationId = workLocationId,
                    ParticipantId = responsibleId
                };
                await workLocationResponsibles.AddAsync(childEntity, cancellationToken);
            }

            var electionWorkLocation = new ElectionWorkLocationEntity()
            {
                ElectionId = command.ElectionId,
                WorkLocationId = workLocationId
            };
            await electionWorkLocationEntities.AddAsync(electionWorkLocation);

            await dataContext.SaveChangesAsync(cancellationToken);

            return entity.Id;
        }

        private async Task<List<TaskTypeTemplateEntity>?> GetWorkLocationTaskTypeTemplates(CreateWorkLocationCommand command, CancellationToken cancellationToken)
        {
            return await TaskTypeTemplates
             .Include(i => i.FileReferences)
             .Where(i => command.TaskTypeTemplateIds.Contains(i.Id))
             .ToListAsync(cancellationToken);
        }

        private async Task<List<Guid>> CreateWorkLocationTaskTypes(List<TaskTypeEntity> taskTypeEntities, List<TaskTypeFileEntity> taskTypeFileEntities, Guid workLocationId, CancellationToken cancellationToken)
        {
            await taskTypes.AddRangeAsync(taskTypeEntities, cancellationToken);

            await taskTypeFiles.AddRangeAsync(taskTypeFileEntities, cancellationToken);

            var workLocationTaskTypeEntities = taskTypeEntities
                .Select(t => new WorkLocationTaskTypeEntity
                {
                    WorkLocationId = workLocationId,
                    TaskTypeId = t.Id
                })
                .ToList();

            await dataContext.SaveChangesAsync(cancellationToken);

            return taskTypeEntities.Select(t => t.Id).ToList();

        }

        public async Task<(IEnumerable<Guid>, IEnumerable<Guid>)> UpdateWorkLocationAsync(UpdateWorkLocationCommand command, CancellationToken cancellationToken)
        {
            var entity = await workLocations.SingleAsync(x => x.Id == command.Id, cancellationToken);
            var existingWorkLocationTaskTypes = await workLocationTaskTypes.Where(x => x.WorkLocationId == command.Id).ToListAsync(cancellationToken);
            var existingWorkLocationTeams = await workLocationTeams.Where(x => x.WorkLocationId == command.Id).ToListAsync(cancellationToken);
            var existingWorkLocationResponsibles = await workLocationResponsibles.Where(x => x.WorkLocationId == command.Id).ToListAsync(cancellationToken);

            entity.Title = command.Title;
            entity.AreaId = command.AreaId;
            entity.Address = command.Address;
            entity.PostalCode = command.PostalCode;
            entity.City = command.City;
            entity.VoteLocation = command.VoteLocation;

            workLocations.Update(entity);

            var deletedWorkLocationTaskTypes = existingWorkLocationTaskTypes.Where(r => !command.TaskTypeIds.Contains(r.TaskTypeId)).ToList();
            var deletedWorkLocationTaskTypeIds = deletedWorkLocationTaskTypes.Select(t => t.TaskTypeId).ToList();
            var newWorkLocationTaskTypeIds = command.TaskTypeIds.Where(r => !existingWorkLocationTaskTypes.Any(i => i.TaskTypeId == r)).ToList();

            var deletedWorkLocationTeams = existingWorkLocationTeams.Where(r => !command.TeamIds.Contains(r.TeamId)).ToList();
            var deletedWorkLocationTeamsIds = deletedWorkLocationTeams.Select(t => t.TeamId).ToList();
            var newWorkLocationTeamIds = command.TeamIds.Where(r => !existingWorkLocationTeams.Any(i => i.TeamId == r)).ToList();

            var deletedWorkLocationResponsibles = existingWorkLocationResponsibles.Where(r => !command.ResponsibleIds.Contains(r.ParticipantId)).ToList();
            var deletedWorkLocationResponsiblesIds = deletedWorkLocationResponsibles.Select(t => t.ParticipantId).ToList();
            var newWorkLocationResponsibleIds = command.ResponsibleIds.Where(r => !existingWorkLocationResponsibles.Any(i => i.ParticipantId == r)).ToList();

            var deletedAssociatedTasks = await tasks.Where(t => t.WorkLocationId == command.Id && (deletedWorkLocationTaskTypeIds.Contains(t.TaskTypeId) || deletedWorkLocationTeamsIds.Contains(t.TeamId))).ToListAsync(cancellationToken);

            foreach (var deletedWorkLocationTaskType in deletedWorkLocationTaskTypes)
            {
                workLocationTaskTypes.Remove(deletedWorkLocationTaskType);
            }
            foreach (var newWorkLocationTaskTypeId in newWorkLocationTaskTypeIds)
            {
                await workLocationTaskTypes.AddAsync(new WorkLocationTaskTypeEntity
                {
                    WorkLocationId = command.Id,
                    TaskTypeId = newWorkLocationTaskTypeId
                }, cancellationToken);
            }

            foreach (var deletedWorkLocationTeam in deletedWorkLocationTeams)
            {
                workLocationTeams.Remove(deletedWorkLocationTeam);
            }
            foreach (var newWorkLocationTeamId in newWorkLocationTeamIds)
            {
                await workLocationTeams.AddAsync(new WorkLocationTeamEntity
                {
                    WorkLocationId = command.Id,
                    TeamId = newWorkLocationTeamId
                }, cancellationToken);
            }

            foreach (var deletedWorkLocationResponsible in deletedWorkLocationResponsibles)
            {
                workLocationResponsibles.Remove(deletedWorkLocationResponsible);
            }
            foreach (var newWorkLocationResponsibleId in newWorkLocationResponsibleIds)
            {
                await workLocationResponsibles.AddAsync(new WorkLocationResponsibleEntity
                {
                    WorkLocationId = command.Id,
                    ParticipantId = newWorkLocationResponsibleId
                }, cancellationToken);
            }

            foreach (var deletedAssociatedTask in deletedAssociatedTasks)
            {
                tasks.Remove(deletedAssociatedTask);
            }

            await dataContext.SaveChangesAsync(cancellationToken);

            return (newWorkLocationResponsibleIds, deletedWorkLocationResponsiblesIds);
        }

        public async Task<(IEnumerable<Guid>, string)> DeleteWorkLocationAsync(DeleteWorkLocationCommand command, CancellationToken cancellationToken)
        {
            var entity = await workLocations
                .Include(i => i.WorkLocationResponsibles)
                .Include(i => i.WorkLocationTaskTypes)
                .Include(i => i.WorkLocationTeams)
                .SingleAsync(x => x.Id == command.Id, cancellationToken);

            var workLocationTitle = entity.Title;

            var workLocationResponsibleIds = entity.WorkLocationResponsibles
                .Select(i => i.ParticipantId)
                .ToArray();

            workLocationResponsibles.RemoveRange(entity.WorkLocationResponsibles);
            workLocationTaskTypes.RemoveRange(entity.WorkLocationTaskTypes);
            workLocationTeams.RemoveRange(entity.WorkLocationTeams);
            workLocations.Remove(entity);

            await dataContext.SaveChangesAsync(cancellationToken);

            return (workLocationResponsibleIds, workLocationTitle);
        }
    }
}
