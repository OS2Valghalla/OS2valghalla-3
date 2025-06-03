using Microsoft.EntityFrameworkCore;
using Valghalla.Database;
using Valghalla.Database.Entities.Tables;
using Valghalla.Internal.Application.Modules.Administration.WorkLocationTemplate.Commands;
using Valghalla.Internal.Application.Modules.Administration.WorkLocationTemplate.Interfaces;

namespace Valghalla.Internal.Infrastructure.Modules.Administration.WorkLocationTemplate
{
    public class WorkLocationTemplateCommandRepository : IWorkLocationTemplateCommandRepository
    {
        private readonly DataContext dataContext;
        private readonly DbSet<TaskAssignmentEntity> tasks;
        private readonly DbSet<WorkLocationTemplateEntity> WorkLocationTemplates;
        //private readonly DbSet<WorkLocationTemplateTaskTypeEntity> WorkLocationTemplateTaskTypes;
        //private readonly DbSet<WorkLocationTemplateResponsibleEntity> WorkLocationTemplateResponsibles;
        //private readonly DbSet<WorkLocationTemplateTeamEntity> WorkLocationTemplateTeams;

        public WorkLocationTemplateCommandRepository(DataContext dataContext)
        {
            this.dataContext = dataContext;
            tasks = dataContext.Set<TaskAssignmentEntity>();
            WorkLocationTemplates = dataContext.Set<WorkLocationTemplateEntity>();
            //WorkLocationTemplateTaskTypes = dataContext.Set<WorkLocationTemplateTaskTypeEntity>();
            //WorkLocationTemplateResponsibles = dataContext.Set<WorkLocationTemplateResponsibleEntity>();
            //WorkLocationTemplateTeams = dataContext.Set<WorkLocationTemplateTeamEntity>();
        }

        public async Task<Guid> CreateWorkLocationTemplateAsync(CreateWorkLocationTemplateCommand command, CancellationToken cancellationToken)
        {
            var WorkLocationTemplateId = Guid.NewGuid();
            var entity = new WorkLocationTemplateEntity()
            {
                Id = WorkLocationTemplateId,
                Title = command.Title,
                AreaId = command.AreaId,
                Address = command.Address,
                PostalCode = command.PostalCode,
                City = command.City,
                VoteLocation = command.VoteLocation
            };

            await WorkLocationTemplates.AddAsync(entity, cancellationToken);
            //foreach(var taskTypeId in command.TaskTypeIds)
            //{
            //    var childEntity = new WorkLocationTemplateTaskTypeEntity()
            //    {
            //        WorkLocationTemplateId = WorkLocationTemplateId,
            //        TaskTypeId = taskTypeId
            //    };
            //    await WorkLocationTemplateTaskTypes.AddAsync(childEntity, cancellationToken);
            //}
            //foreach (var teamId in command.TeamIds)
            //{
            //    var childEntity = new WorkLocationTemplateTeamEntity()
            //    {
            //        WorkLocationTemplateId = WorkLocationTemplateId,
            //        TeamId = teamId
            //    };
            //    await WorkLocationTemplateTeams.AddAsync(childEntity, cancellationToken);
            //}
            //foreach (var responsibleId in command.ResponsibleIds)
            //{
            //    var childEntity = new WorkLocationTemplateResponsibleEntity()
            //    {
            //        WorkLocationTemplateId = WorkLocationTemplateId,
            //        ParticipantId = responsibleId
            //    };
            //    await WorkLocationTemplateResponsibles.AddAsync(childEntity, cancellationToken);
            //}

            await dataContext.SaveChangesAsync(cancellationToken);

            return entity.Id;
        }

        public async Task<int> UpdateWorkLocationTemplateAsync(UpdateWorkLocationTemplateCommand command, CancellationToken cancellationToken)
        {
            var entity = await WorkLocationTemplates.SingleAsync(x => x.Id == command.Id, cancellationToken);
            //var existingWorkLocationTemplateTaskTypes = await WorkLocationTemplateTaskTypes.Where(x => x.WorkLocationTemplateId == command.Id).ToListAsync(cancellationToken);
            //var existingWorkLocationTemplateTeams = await WorkLocationTemplateTeams.Where(x => x.WorkLocationTemplateId == command.Id).ToListAsync(cancellationToken);
            //var existingWorkLocationTemplateResponsibles = await WorkLocationTemplateResponsibles.Where(x => x.WorkLocationTemplateId == command.Id).ToListAsync(cancellationToken);

            entity.Title = command.Title;
            entity.AreaId = command.AreaId;
            entity.Address = command.Address;
            entity.PostalCode = command.PostalCode;
            entity.City = command.City;
            entity.VoteLocation = command.VoteLocation;

            WorkLocationTemplates.Update(entity);

            //var deletedWorkLocationTemplateTaskTypes = existingWorkLocationTemplateTaskTypes.Where(r => !command.TaskTypeIds.Contains(r.TaskTypeId)).ToList();
            //var deletedWorkLocationTemplateTaskTypeIds = deletedWorkLocationTemplateTaskTypes.Select(t => t.TaskTypeId).ToList();
            //var newWorkLocationTemplateTaskTypeIds = command.TaskTypeIds.Where(r => !existingWorkLocationTemplateTaskTypes.Any(i => i.TaskTypeId == r)).ToList();

            //var deletedWorkLocationTemplateTeams = existingWorkLocationTemplateTeams.Where(r => !command.TeamIds.Contains(r.TeamId)).ToList();
            //var deletedWorkLocationTemplateTeamsIds = deletedWorkLocationTemplateTeams.Select(t => t.TeamId).ToList();
            //var newWorkLocationTemplateTeamIds = command.TeamIds.Where(r => !existingWorkLocationTemplateTeams.Any(i => i.TeamId == r)).ToList();

            //var deletedWorkLocationTemplateResponsibles = existingWorkLocationTemplateResponsibles.Where(r => !command.ResponsibleIds.Contains(r.ParticipantId)).ToList();
            //var deletedWorkLocationTemplateResponsiblesIds = deletedWorkLocationTemplateResponsibles.Select(t => t.ParticipantId).ToList();
            //var newWorkLocationTemplateResponsibleIds = command.ResponsibleIds.Where(r => !existingWorkLocationTemplateResponsibles.Any(i => i.ParticipantId == r)).ToList();

            //var deletedAssociatedTasks = await tasks.Where(t => t.WorkLocationTemplateId == command.Id && (deletedWorkLocationTemplateTaskTypeIds.Contains(t.TaskTypeId) || deletedWorkLocationTemplateTeamsIds.Contains(t.TeamId))).ToListAsync(cancellationToken);

            //foreach (var deletedWorkLocationTemplateTaskType in deletedWorkLocationTemplateTaskTypes)
            //{
            //    WorkLocationTemplateTaskTypes.Remove(deletedWorkLocationTemplateTaskType);
            //}
            //foreach (var newWorkLocationTemplateTaskTypeId in newWorkLocationTemplateTaskTypeIds)
            //{
            //    await WorkLocationTemplateTaskTypes.AddAsync(new WorkLocationTemplateTaskTypeEntity
            //    {
            //        WorkLocationTemplateId = command.Id,
            //        TaskTypeId = newWorkLocationTemplateTaskTypeId
            //    }, cancellationToken);
            //}

            //foreach (var deletedWorkLocationTemplateTeam in deletedWorkLocationTemplateTeams)
            //{
            //    WorkLocationTemplateTeams.Remove(deletedWorkLocationTemplateTeam);
            //}
            //foreach (var newWorkLocationTemplateTeamId in newWorkLocationTemplateTeamIds)
            //{
            //    await WorkLocationTemplateTeams.AddAsync(new WorkLocationTemplateTeamEntity
            //    {
            //        WorkLocationTemplateId = command.Id,
            //        TeamId = newWorkLocationTemplateTeamId
            //    }, cancellationToken);
            //}

            //foreach (var deletedWorkLocationTemplateResponsible in deletedWorkLocationTemplateResponsibles)
            //{
            //    WorkLocationTemplateResponsibles.Remove(deletedWorkLocationTemplateResponsible);
            //}
            //foreach (var newWorkLocationTemplateResponsibleId in newWorkLocationTemplateResponsibleIds)
            //{
            //    await WorkLocationTemplateResponsibles.AddAsync(new WorkLocationTemplateResponsibleEntity
            //    {
            //        WorkLocationTemplateId = command.Id,
            //        ParticipantId = newWorkLocationTemplateResponsibleId
            //    }, cancellationToken);
            //}

            //foreach (var deletedAssociatedTask in deletedAssociatedTasks)
            //{
            //    tasks.Remove(deletedAssociatedTask);
            //}

            var result = await dataContext.SaveChangesAsync(cancellationToken);

            return result;
        }

        public async Task<int> DeleteWorkLocationTemplateAsync(DeleteWorkLocationTemplateCommand command, CancellationToken cancellationToken)
        {
            var entity = await WorkLocationTemplates
                //.Include(i => i.WorkLocationTemplateResponsibles)
                //.Include(i => i.WorkLocationTemplateTaskTypes)
                //.Include(i => i.WorkLocationTemplateTeams)
                .SingleAsync(x => x.Id == command.Id, cancellationToken);

            var WorkLocationTemplateTitle = entity.Title;

            //var WorkLocationTemplateResponsibleIds = entity.WorkLocationTemplateResponsibles
            //    .Select(i => i.ParticipantId)
            //    .ToArray();

            //WorkLocationTemplateResponsibles.RemoveRange(entity.WorkLocationTemplateResponsibles);
            //WorkLocationTemplateTaskTypes.RemoveRange(entity.WorkLocationTemplateTaskTypes);
            //WorkLocationTemplateTeams.RemoveRange(entity.WorkLocationTemplateTeams);
            WorkLocationTemplates.Remove(entity);

            var reuslt = await dataContext.SaveChangesAsync(cancellationToken);

            return reuslt;
        }
    }
}
