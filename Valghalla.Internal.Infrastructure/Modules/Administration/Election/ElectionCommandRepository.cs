﻿using System.Security.Cryptography;
using System.Text;

using Microsoft.EntityFrameworkCore;

using Valghalla.Database;
using Valghalla.Database.Entities.Tables;
using Valghalla.Internal.Application.Modules.Administration.Election.Commands;
using Valghalla.Internal.Application.Modules.Administration.Election.Interfaces;

namespace Valghalla.Internal.Infrastructure.Modules.Administration.Election
{
    internal class ElectionCommandRepository : IElectionCommandRepository
    {
        private readonly DataContext dataContext;
        private readonly DbSet<ElectionEntity> elections;
        private readonly DbSet<ElectionWorkLocationEntity> electionWorkLocations;
        private readonly DbSet<WorkLocationTemplateEntity> workLocationTemplates;
        private readonly DbSet<WorkLocationEntity> workLocations;
        private readonly DbSet<ElectionTaskTypeCommunicationTemplateEntity> electionTaskTypeCommunicationTemplates;
        private readonly DbSet<TaskAssignmentEntity> taskAssignments;

        public ElectionCommandRepository(DataContext dataContext)
        {
            this.dataContext = dataContext;
            elections = dataContext.Set<ElectionEntity>();
            electionWorkLocations = dataContext.Set<ElectionWorkLocationEntity>();
            workLocationTemplates = dataContext.Set<WorkLocationTemplateEntity>();
            workLocations = dataContext.Set<WorkLocationEntity>();
            electionTaskTypeCommunicationTemplates = dataContext.Set<ElectionTaskTypeCommunicationTemplateEntity>();
            taskAssignments = dataContext.Set<TaskAssignmentEntity>();
        }

        public async Task<Guid> CreateElectionAsync(CreateElectionCommand command, CancellationToken cancellationToken)
        {
            var entity = new ElectionEntity()
            {
                Id = Guid.NewGuid(),
                Active = false,
                Title = command.Title,
                ElectionTypeId = command.ElectionTypeId,
                LockPeriod = command.LockPeriod,
                ElectionStartDate = command.ElectionStartDate,
                ElectionEndDate = command.ElectionEndDate,
                ElectionDate = command.ElectionDate,
                ConfirmationOfRegistrationCommunicationTemplateId = command.ConfirmationOfRegistrationCommunicationTemplateId,
                ConfirmationOfCancellationCommunicationTemplateId = command.ConfirmationOfCancellationCommunicationTemplateId,
                InvitationCommunicationTemplateId = command.InvitationCommunicationTemplateId,
                InvitationReminderCommunicationTemplateId = command.InvitationReminderCommunicationTemplateId,
                TaskReminderCommunicationTemplateId = command.TaskReminderCommunicationTemplateId,
                RetractedInvitationCommunicationTemplateId = command.RetractedInvitationCommunicationTemplateId,
                RemovedFromTaskCommunicationTemplateId = command.RemovedFromTaskCommunicationTemplateId,
                RemovedByValidationCommunicationTemplateId = command.RemovedByValidationCommunicationTemplateId
            };

            var workLocationTemplatesForElection = await workLocationTemplates.Where(i => command.WorkLocationIds.Contains(i.Id)).ToListAsync(cancellationToken);
            var workLocationsForElection = workLocationTemplatesForElection
                .Select(i => new WorkLocationEntity()
                {
                    Id = Guid.NewGuid(),
                    Address = i.Address,
                    PostalCode = i.PostalCode,
                    City = i.City,
                    AreaId = i.AreaId,
                    Title = i.Title,
                    VoteLocation = i.VoteLocation,
                    ChangedBy = i.ChangedBy,
                    ChangedAt = i.ChangedAt,
                    CreatedAt = i.CreatedAt,
                    CreatedBy = i.CreatedBy,
                    ChangedByUser = i.ChangedByUser,
                    CreatedByUser = i.CreatedByUser,
                    WorkLocationTemplateId = i.Id
                })
                .ToList();
            await workLocations.AddRangeAsync(workLocationsForElection, cancellationToken);
            await dataContext.SaveChangesAsync(cancellationToken);

            var electionWorkLocationEntities = workLocationsForElection.Select(workLocation => new ElectionWorkLocationEntity()
            {
                ElectionId = entity.Id,
                WorkLocationId = workLocation.Id,
            });

            var electionTaskTypeCommunicationTemplateEntities = command.ElectionTaskTypeCommunicationTemplates.Select(electionTaskTypeCommunicationTemplate => new ElectionTaskTypeCommunicationTemplateEntity()
            {
                ElectionId = entity.Id,
                TaskTypeId = electionTaskTypeCommunicationTemplate.TaskTypeId,
                ConfirmationOfRegistrationCommunicationTemplateId = electionTaskTypeCommunicationTemplate.ConfirmationOfRegistrationCommunicationTemplateId,
                ConfirmationOfCancellationCommunicationTemplateId = electionTaskTypeCommunicationTemplate.ConfirmationOfCancellationCommunicationTemplateId,
                InvitationCommunicationTemplateId = electionTaskTypeCommunicationTemplate.InvitationCommunicationTemplateId,
                InvitationReminderCommunicationTemplateId = electionTaskTypeCommunicationTemplate.InvitationReminderCommunicationTemplateId,
                TaskReminderCommunicationTemplateId = electionTaskTypeCommunicationTemplate.TaskReminderCommunicationTemplateId,
                RetractedInvitationCommunicationTemplateId = electionTaskTypeCommunicationTemplate.RetractedInvitationCommunicationTemplateId,
                RemovedFromTaskCommunicationTemplateId = electionTaskTypeCommunicationTemplate.RemovedFromTaskCommunicationTemplateId,
                RemovedByValidationCommunicationTemplateId = electionTaskTypeCommunicationTemplate.RemovedByValidationCommunicationTemplateId
            });

            await elections.AddAsync(entity, cancellationToken);
            await electionWorkLocations.AddRangeAsync(electionWorkLocationEntities, cancellationToken);
            await electionTaskTypeCommunicationTemplates.AddRangeAsync(electionTaskTypeCommunicationTemplateEntities, cancellationToken);
            await dataContext.SaveChangesAsync(cancellationToken);

            return entity.Id;
        }

        public async Task UpdateElectionAsync(UpdateElectionCommand command, CancellationToken cancellationToken)
        {
            var entity = await elections.SingleAsync(i => i.Id == command.Id, cancellationToken);

            entity.Title = command.Title;
            entity.LockPeriod = command.LockPeriod;

            elections.Update(entity);

            if (command.WorkLocationIds.Any())
            {
                var workLocationIds = command.WorkLocationIds;
                var existingElectionWorkLocations = await electionWorkLocations.Where(item => item.ElectionId == entity.Id).ToListAsync(cancellationToken);

                workLocationIds = workLocationIds.Where(workLocationId => !existingElectionWorkLocations.Any(item => item.WorkLocationId == workLocationId));

                var electionWorkLocationEntities = workLocationIds.Select(workLocationId => new ElectionWorkLocationEntity()
                {
                    ElectionId = entity.Id,
                    WorkLocationId = workLocationId,
                });

                if (electionWorkLocationEntities.Any())
                    await electionWorkLocations.AddRangeAsync(electionWorkLocationEntities, cancellationToken);
            }

            await dataContext.SaveChangesAsync(cancellationToken);
        }

        public async Task<Guid> DuplicateElectionAsync(DuplicateElectionCommand command, CancellationToken cancellationToken)
        {
            var sourceElection = await elections.SingleAsync(i => i.Id == command.SourceElectionId, cancellationToken);

            var sourceElectionTasks = await taskAssignments
                .Where(i => i.ElectionId == command.SourceElectionId && command.WorkLocationIds.Contains(i.WorkLocationId)
                && i.TaskDate >= sourceElection.ElectionDate.AddDays(-1 * command.DaysBeforeElectionDate)
                && i.TaskDate <= sourceElection.ElectionDate.AddDays(command.DaysAfterElectionDate))
                .OrderBy(x => x.TaskDate).ThenBy(x => x.Id).ToListAsync(cancellationToken);

            var newElectionId = Guid.NewGuid();

            var entity = new ElectionEntity()
            {
                Id = newElectionId,
                Active = false,
                Title = command.Title,
                ElectionTypeId = command.ElectionTypeId,
                LockPeriod = command.LockPeriod,
                ElectionDate = command.ElectionDate,
                ElectionStartDate = command.ElectionDate.AddDays(-1 * command.DaysBeforeElectionDate),
                ElectionEndDate = command.ElectionDate.AddDays(command.DaysAfterElectionDate),
                ConfirmationOfRegistrationCommunicationTemplateId = command.ConfirmationOfRegistrationCommunicationTemplateId,
                ConfirmationOfCancellationCommunicationTemplateId = command.ConfirmationOfCancellationCommunicationTemplateId,
                InvitationCommunicationTemplateId = command.InvitationCommunicationTemplateId,
                InvitationReminderCommunicationTemplateId = command.InvitationReminderCommunicationTemplateId,
                TaskReminderCommunicationTemplateId = command.TaskReminderCommunicationTemplateId,
                RetractedInvitationCommunicationTemplateId = command.RetractedInvitationCommunicationTemplateId,
                RemovedFromTaskCommunicationTemplateId = command.RemovedFromTaskCommunicationTemplateId,
                RemovedByValidationCommunicationTemplateId = command.RemovedByValidationCommunicationTemplateId,
            };

            var electionWorkLocationEntities = command.WorkLocationIds.Select(workLocationId => new ElectionWorkLocationEntity()
            {
                ElectionId = entity.Id,
                WorkLocationId = workLocationId,
            });

            var electionTaskTypeCommunicationTemplateEntities = command.ElectionTaskTypeCommunicationTemplates.Select(electionTaskTypeCommunicationTemplate => new ElectionTaskTypeCommunicationTemplateEntity()
            {
                ElectionId = entity.Id,
                TaskTypeId = electionTaskTypeCommunicationTemplate.TaskTypeId,
                ConfirmationOfRegistrationCommunicationTemplateId = electionTaskTypeCommunicationTemplate.ConfirmationOfRegistrationCommunicationTemplateId,
                ConfirmationOfCancellationCommunicationTemplateId = electionTaskTypeCommunicationTemplate.ConfirmationOfCancellationCommunicationTemplateId,
                InvitationCommunicationTemplateId = electionTaskTypeCommunicationTemplate.InvitationCommunicationTemplateId,
                InvitationReminderCommunicationTemplateId = electionTaskTypeCommunicationTemplate.InvitationReminderCommunicationTemplateId,
                TaskReminderCommunicationTemplateId = electionTaskTypeCommunicationTemplate.TaskReminderCommunicationTemplateId,
                RetractedInvitationCommunicationTemplateId = electionTaskTypeCommunicationTemplate.RetractedInvitationCommunicationTemplateId,
                RemovedFromTaskCommunicationTemplateId = electionTaskTypeCommunicationTemplate.RemovedFromTaskCommunicationTemplateId,
                RemovedByValidationCommunicationTemplateId = electionTaskTypeCommunicationTemplate.RemovedByValidationCommunicationTemplateId,
            });

            foreach (var sourceElectionTask in sourceElectionTasks)
            {
                var taskDate = command.ElectionDate.AddDays(sourceElectionTask.TaskDate.Subtract(sourceElection.ElectionDate).Days);
                var hashValue = await CreateTaskAssignmentHashValueAsync(newElectionId, sourceElectionTask.WorkLocationId, sourceElectionTask.TeamId, sourceElectionTask.TaskTypeId, taskDate);

                var newTask = new TaskAssignmentEntity()
                {
                    Id = Guid.NewGuid(),
                    ElectionId = newElectionId,
                    WorkLocationId = sourceElectionTask.WorkLocationId,
                    TeamId = sourceElectionTask.TeamId,
                    TaskTypeId = sourceElectionTask.TaskTypeId,
                    TaskDate = taskDate.ToUniversalTime(),
                    Responsed = false,
                    Accepted = false,
                    InvitationSent = false,
                    RegistrationSent = false,
                    HashValue = hashValue,
                    InvitationDate = null,
                    InvitationReminderDate = null,
                    TaskReminderDate = null
                };
                await taskAssignments.AddAsync(newTask, cancellationToken);
            }

            await elections.AddAsync(entity, cancellationToken);
            await electionWorkLocations.AddRangeAsync(electionWorkLocationEntities, cancellationToken);
            await electionTaskTypeCommunicationTemplates.AddRangeAsync(electionTaskTypeCommunicationTemplateEntities, cancellationToken);
            await dataContext.SaveChangesAsync(cancellationToken);

            return entity.Id;
        }

        public async Task UpdateElectionCommunicationConfigurationsAsync(UpdateElectionCommunicationConfigurationsCommand command, CancellationToken cancellationToken)
        {
            var entity = await elections.Include(i => i.ElectionTaskTypeCommunicationTemplates).SingleAsync(i => i.Id == command.Id, cancellationToken);

            entity.ConfirmationOfRegistrationCommunicationTemplateId = command.ConfirmationOfRegistrationCommunicationTemplateId;
            entity.ConfirmationOfCancellationCommunicationTemplateId = command.ConfirmationOfCancellationCommunicationTemplateId;
            entity.InvitationCommunicationTemplateId = command.InvitationCommunicationTemplateId;
            entity.InvitationReminderCommunicationTemplateId = command.InvitationReminderCommunicationTemplateId;
            entity.TaskReminderCommunicationTemplateId = command.TaskReminderCommunicationTemplateId;
            entity.RetractedInvitationCommunicationTemplateId = command.RetractedInvitationCommunicationTemplateId;
            entity.RemovedFromTaskCommunicationTemplateId = command.RemovedFromTaskCommunicationTemplateId;
            entity.RemovedByValidationCommunicationTemplateId = command.RemovedByValidationCommunicationTemplateId;

            var existingElectionTaskTypeCommunicationTemplates = await electionTaskTypeCommunicationTemplates.Where(i => i.ElectionId == command.Id).ToArrayAsync(cancellationToken);

            var electionTaskTypeCommunicationTemplateEntitiesToRemove = existingElectionTaskTypeCommunicationTemplates.Where(i => !command.ElectionTaskTypeCommunicationTemplates.Any(t => t.TaskTypeId == i.TaskTypeId));

            var electionTaskTypeCommunicationTemplateEntitiesToUpdate = existingElectionTaskTypeCommunicationTemplates.Where(i => command.ElectionTaskTypeCommunicationTemplates.Any(t => t.TaskTypeId == i.TaskTypeId)).ToList();

            var electionTaskTypeCommunicationTemplateEntitiesToAdd = command.ElectionTaskTypeCommunicationTemplates
                .Where(i => !existingElectionTaskTypeCommunicationTemplates.Any(t => t.TaskTypeId == i.TaskTypeId))
                 .Select(i => new ElectionTaskTypeCommunicationTemplateEntity()
                 {
                     ElectionId = command.Id,
                     TaskTypeId = i.TaskTypeId,
                     ConfirmationOfRegistrationCommunicationTemplateId = i.ConfirmationOfRegistrationCommunicationTemplateId,
                     ConfirmationOfCancellationCommunicationTemplateId = i.ConfirmationOfCancellationCommunicationTemplateId,
                     InvitationCommunicationTemplateId = i.InvitationCommunicationTemplateId,
                     InvitationReminderCommunicationTemplateId = i.InvitationReminderCommunicationTemplateId,
                     TaskReminderCommunicationTemplateId = i.TaskReminderCommunicationTemplateId,
                     RetractedInvitationCommunicationTemplateId = i.RetractedInvitationCommunicationTemplateId,
                     RemovedFromTaskCommunicationTemplateId = i.RemovedFromTaskCommunicationTemplateId,
                     RemovedByValidationCommunicationTemplateId = i.RemovedByValidationCommunicationTemplateId,
                 });

            elections.Update(entity);

            if (electionTaskTypeCommunicationTemplateEntitiesToRemove.Any())
            {
                electionTaskTypeCommunicationTemplates.RemoveRange(electionTaskTypeCommunicationTemplateEntitiesToRemove);
            }

            if (electionTaskTypeCommunicationTemplateEntitiesToAdd.Any())
            {
                await electionTaskTypeCommunicationTemplates.AddRangeAsync(electionTaskTypeCommunicationTemplateEntitiesToAdd, cancellationToken);
            }

            foreach (var electionTaskTypeCommunicationTemplateEntityToUpdate in electionTaskTypeCommunicationTemplateEntitiesToUpdate)
            {
                var commandElectionTaskTypeCommunicationTemplate = command.ElectionTaskTypeCommunicationTemplates.First(i => i.TaskTypeId == electionTaskTypeCommunicationTemplateEntityToUpdate.TaskTypeId);
                electionTaskTypeCommunicationTemplateEntityToUpdate.ConfirmationOfRegistrationCommunicationTemplateId = commandElectionTaskTypeCommunicationTemplate.ConfirmationOfRegistrationCommunicationTemplateId;
                electionTaskTypeCommunicationTemplateEntityToUpdate.ConfirmationOfCancellationCommunicationTemplateId = commandElectionTaskTypeCommunicationTemplate.ConfirmationOfCancellationCommunicationTemplateId;
                electionTaskTypeCommunicationTemplateEntityToUpdate.InvitationCommunicationTemplateId = commandElectionTaskTypeCommunicationTemplate.InvitationCommunicationTemplateId;
                electionTaskTypeCommunicationTemplateEntityToUpdate.InvitationReminderCommunicationTemplateId = commandElectionTaskTypeCommunicationTemplate.InvitationReminderCommunicationTemplateId;
                electionTaskTypeCommunicationTemplateEntityToUpdate.TaskReminderCommunicationTemplateId = commandElectionTaskTypeCommunicationTemplate.TaskReminderCommunicationTemplateId;
                electionTaskTypeCommunicationTemplateEntityToUpdate.RetractedInvitationCommunicationTemplateId = commandElectionTaskTypeCommunicationTemplate.RetractedInvitationCommunicationTemplateId;
                electionTaskTypeCommunicationTemplateEntityToUpdate.RemovedFromTaskCommunicationTemplateId = commandElectionTaskTypeCommunicationTemplate.RemovedFromTaskCommunicationTemplateId;
                electionTaskTypeCommunicationTemplateEntityToUpdate.RemovedByValidationCommunicationTemplateId = commandElectionTaskTypeCommunicationTemplate.RemovedByValidationCommunicationTemplateId;
                electionTaskTypeCommunicationTemplates.Update(electionTaskTypeCommunicationTemplateEntityToUpdate);
            }

            await dataContext.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteElectionAsync(DeleteElectionCommand command, CancellationToken cancellationToken)
        {
            var entity = await elections.SingleAsync(i => i.Id == command.Id, cancellationToken);

            elections.Remove(entity);

            await dataContext.SaveChangesAsync(cancellationToken);
        }

        public async Task ActivateElectionAsync(ActivateElectionCommand command, CancellationToken cancellationToken)
        {
            var entity = await elections.SingleAsync(i => i.Id == command.Id, cancellationToken);
            entity.Active = true;

            elections.Update(entity);

            await dataContext.SaveChangesAsync(cancellationToken);
        }

        public async Task DeactivateElectionAsync(DeactivateElectionCommand command, CancellationToken cancellationToken)
        {
            var entity = await elections.SingleAsync(i => i.Id == command.Id, cancellationToken);
            entity.Active = false;

            elections.Update(entity);

            await dataContext.SaveChangesAsync(cancellationToken);
        }

        private static async Task<string> CreateTaskAssignmentHashValueAsync(Guid electionId, Guid workLocationId, Guid teamId, Guid taskTypeId, DateTime taskDate)
        {
            var uniqueValue =
                electionId.ToString() +
                workLocationId.ToString() +
                teamId.ToString() +
                taskTypeId.ToString() +
                taskDate.ToUniversalTime().ToString();

            using var hashProcess = SHA256.Create();
            var byteArrayResultOfRawData = new MemoryStream(Encoding.UTF8.GetBytes(uniqueValue));
            var byteArrayResult = await hashProcess.ComputeHashAsync(byteArrayResultOfRawData);

            return string.Concat(Array.ConvertAll(byteArrayResult, h => h.ToString("X2")));
        }
    }
}
