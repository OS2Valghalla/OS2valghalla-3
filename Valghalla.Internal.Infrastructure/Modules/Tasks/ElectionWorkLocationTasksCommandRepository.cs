using System.Security.Cryptography;
using System.Text;

using Microsoft.EntityFrameworkCore;

using Valghalla.Database;
using Valghalla.Database.Entities.Tables;
using Valghalla.Internal.Application.Modules.Tasks.Commands;
using Valghalla.Internal.Application.Modules.Tasks.Interfaces;
using Valghalla.Internal.Application.Modules.Tasks.Requests;

namespace Valghalla.Internal.Infrastructure.Modules.Tasks
{
    internal class ElectionWorkLocationTasksCommandRepository : IElectionWorkLocationTasksCommandRepository
    {
        private readonly DataContext dataContext;
        private readonly DbSet<TaskAssignmentEntity> taskAssignments;
        private readonly DbSet<RejectedTaskAssignmentEntity> rejectedTaskAssignments;
        private readonly DbSet<ParticipantEntity> participants;
        private readonly DbSet<TeamMemberEntity> teamMembers;
        private readonly DbSet<SpecialDietParticipantEntity> specialDietParticipants;

        public ElectionWorkLocationTasksCommandRepository(DataContext dataContext)
        {
            this.dataContext = dataContext;
            taskAssignments = dataContext.Set<TaskAssignmentEntity>();
            rejectedTaskAssignments = dataContext.Set<RejectedTaskAssignmentEntity>();
            participants = dataContext.Set<ParticipantEntity>();
            teamMembers = dataContext.Set<TeamMemberEntity>();
            specialDietParticipants = dataContext.Set<SpecialDietParticipantEntity>();
        }

        public async Task DistributeElectionWorkLocationTasksAsync(DistributeElectionWorkLocationTasksCommand command, CancellationToken cancellationToken)
        {
            var tasks = await taskAssignments
                .Where(i => i.WorkLocationId == command.WorkLocationId && i.ElectionId == command.ElectionId).OrderBy(x => x.TaskDate).ToListAsync(cancellationToken);

            foreach (var distributingTask in command.DistributingTasks)
            {
                var assignedTasksCount = tasks.Count(t => t.TaskDate == distributingTask.TasksDate && t.TeamId == distributingTask.TeamId && t.TaskTypeId == distributingTask.TaskTypeId
                    && t.Accepted);

                var unassignedTasks = tasks.Where(t => t.TaskDate == distributingTask.TasksDate && t.TeamId == distributingTask.TeamId && t.TaskTypeId == distributingTask.TaskTypeId
                    && !t.Accepted).ToList();

                var allTasksCount = tasks.Count(t => t.TaskDate == distributingTask.TasksDate && t.TeamId == distributingTask.TeamId && t.TaskTypeId == distributingTask.TaskTypeId);

                if (distributingTask.TasksCount >= assignedTasksCount)
                {
                    if (distributingTask.TasksCount > allTasksCount)
                    {
                        var hashValue = await CreateHashValueAsync(command, distributingTask);

                        for (int i = allTasksCount; i < distributingTask.TasksCount; i++)
                        {
                            var newTask = new TaskAssignmentEntity()
                            {
                                Id = Guid.NewGuid(),
                                ElectionId = command.ElectionId,
                                WorkLocationId = command.WorkLocationId,
                                TeamId = distributingTask.TeamId,
                                TaskTypeId = distributingTask.TaskTypeId,
                                TaskDate = distributingTask.TasksDate.ToUniversalTime(),
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
                    }
                    else
                    {
                        for (int i = 0; i < allTasksCount - distributingTask.TasksCount; i++)
                        {
                            if (unassignedTasks.Count > i)
                            {
                                var deletingTask = await taskAssignments.FirstOrDefaultAsync(t => t.Id == unassignedTasks[i].Id);
                                if (deletingTask != null)
                                {
                                    taskAssignments.Remove(deletingTask);
                                }
                            }
                        }
                    }
                }
            }

            await dataContext.SaveChangesAsync(cancellationToken);
        }

        public async Task<bool> AssignParticipantToTaskAsync(AssignParticipantToTaskCommand command, CancellationToken cancellationToken)
        {
            var task = await taskAssignments.FirstAsync(x => x.Id == command.TaskAssignmentId && x.ElectionId == command.ElectionId);
            var existingTeamMember = await teamMembers.AnyAsync(x => x.TeamId == task.TeamId && x.ParticipantId == command.ParticipantId);

            task.ParticipantId = command.ParticipantId;
            task.Accepted = false;
            task.Responsed = false;
            task.InvitationCode = Guid.NewGuid();
            task.InvitationSent = false;
            task.RegistrationSent = false;
            task.InvitationDate = null;
            task.InvitationReminderDate = null;
            task.TaskReminderDate = null;

            taskAssignments.Update(task);

            if (!existingTeamMember)
            {
                await teamMembers.AddAsync(new TeamMemberEntity()
                {
                    ParticipantId = command.ParticipantId,
                    TeamId = task.TeamId,
                });
            }

            var updated = await dataContext.SaveChangesAsync(cancellationToken);
            return updated > 0;
        }

        public async Task<bool> AssignCreatingParticipantToTaskAsync(AssignParticipantToTaskCommand command, IEnumerable<Guid> teamIds, CancellationToken cancellationToken)
        {
            var task = await taskAssignments.FirstAsync(x => x.Id == command.TaskAssignmentId && x.ElectionId == command.ElectionId);

            task.ParticipantId = command.ParticipantId;
            task.Accepted = false;
            task.Responsed = false;
            task.InvitationCode = Guid.NewGuid();
            task.InvitationSent = false;
            task.RegistrationSent = false;
            task.InvitationDate = null;
            task.InvitationReminderDate = null;
            task.TaskReminderDate = null;

            taskAssignments.Update(task);

            if (!teamIds.Any(x => x == task.TeamId))
            {
                await teamMembers.AddAsync(new TeamMemberEntity()
                {
                    ParticipantId = command.ParticipantId,
                    TeamId = task.TeamId,
                });
            }

            var updated = await dataContext.SaveChangesAsync(cancellationToken);
            return updated > 0;
        }

        public async Task RemoveParticipantFromTaskAsync(RemoveParticipantFromTaskCommand command, CancellationToken cancellationToken)
        {
            var task = await taskAssignments
                .FirstAsync(i => i.Id == command.TaskAssignmentId && i.ElectionId == command.ElectionId, cancellationToken);

            task.ParticipantId = null;
            task.Accepted = false;
            task.Responsed = false;
            task.InvitationCode = null;
            task.InvitationSent = false;
            task.RegistrationSent = false;
            task.InvitationDate = null;
            task.InvitationReminderDate = null;
            task.TaskReminderDate = null;

            taskAssignments.Update(task);

            await dataContext.SaveChangesAsync(cancellationToken);
        }

        public async Task<Guid?> ReplyForParticipantAsync(ReplyForParticipantCommand command, CancellationToken cancellationToken)
        {
            var task = await taskAssignments
                .FirstAsync(i => i.Id == command.TaskAssignmentId && i.ElectionId == command.ElectionId, cancellationToken);

            task.Accepted = command.Accepted;

            if (!command.Accepted)
            {
                var rejectedTask = new RejectedTaskAssignmentEntity()
                {
                    Id = Guid.NewGuid(),
                    ElectionId = task.ElectionId,
                    WorkLocationId = task.WorkLocationId,
                    TeamId = task.TeamId,
                    TaskTypeId = task.TaskTypeId,
                    TaskDate = task.TaskDate,
                    ParticipantId = task.ParticipantId.GetValueOrDefault()
                };

                await rejectedTaskAssignments.AddAsync(rejectedTask, cancellationToken);

                task.Responsed = false;
                task.ParticipantId = null;
                task.InvitationCode = null;
                task.InvitationSent = false;
                task.RegistrationSent = false;
                task.InvitationDate = null;
                task.InvitationReminderDate = null;
                task.TaskReminderDate = null;
            }
            else
            {
                task.Responsed = true;
                task.InvitationCode = null;
                task.InvitationSent = true;
                task.RegistrationSent = false;

                var participant = await participants.FirstAsync(i => i.Id == task.ParticipantId, cancellationToken);
                participant.MobileNumber = command.MobileNumber;
                participant.Email = command.Email;

                var specialDietParticipantEntities = await specialDietParticipants
                    .Where(i => i.ParticipantId == task.ParticipantId)
                    .ToArrayAsync(cancellationToken);

                var specialDietParticipantEntitiesToAdd = command.SpecialDietIds
                    .Where(specialDietId => !specialDietParticipantEntities.Any(i => i.SpecialDietId == specialDietId))
                    .Select(specialDietId => new SpecialDietParticipantEntity()
                    {
                        ParticipantId = task.ParticipantId.GetValueOrDefault(),
                        SpecialDietId = specialDietId,
                    });

                var specialDietParticipantEntitiesToRemove = specialDietParticipantEntities
                    .Where(i => !command.SpecialDietIds.Any(specialDietId => specialDietId == i.SpecialDietId));

                participants.Update(participant);

                if (specialDietParticipantEntitiesToRemove.Any())
                {
                    specialDietParticipants.RemoveRange(specialDietParticipantEntitiesToRemove);
                }

                if (specialDietParticipantEntitiesToAdd.Any())
                {
                    await specialDietParticipants.AddRangeAsync(specialDietParticipantEntitiesToAdd, cancellationToken);
                }
            }

            taskAssignments.Update(task);

            await dataContext.SaveChangesAsync(cancellationToken);

            return task.ParticipantId;
        }

        private static async Task<string> CreateHashValueAsync(DistributeElectionWorkLocationTasksCommand command, TasksDistributingRequest request)
        {
            var uniqueValue =
                command.ElectionId.ToString() +
                command.WorkLocationId.ToString() +
                request.TeamId.ToString() +
                request.TaskTypeId.ToString() +
                request.TasksDate.ToUniversalTime().ToString();

            using var hashProcess = SHA256.Create();
            var byteArrayResultOfRawData = new MemoryStream(Encoding.UTF8.GetBytes(uniqueValue));
            var byteArrayResult = await hashProcess.ComputeHashAsync(byteArrayResultOfRawData);

            return string.Concat(Array.ConvertAll(byteArrayResult, h => h.ToString("X2")));
        }

        public async Task<bool> MoveElectionWorkLocationTasksAsync(MoveTasksCommand command, CancellationToken cancellationToken)
        {
            if (command == null ||
                command.TaskIds == null || command.TaskIds.Length == 0 ||
                command.TargetTeamId is not { } targetTeamId || targetTeamId == Guid.Empty ||
                command.SourceTeamId is not { } sourceTeamId || sourceTeamId == Guid.Empty)
            {
                return false;
            }

            var taskIdSet = new HashSet<Guid>(command.TaskIds.Select(Guid.Parse));

            var tasks = await taskAssignments
                .Where(ta => taskIdSet.Contains(ta.Id) && ta.TeamId == sourceTeamId && ta.ParticipantId == null)
                .ToListAsync(cancellationToken);

            if (tasks.Count == 0) return false;

            foreach (var task in tasks)
            {
                task.TeamId = targetTeamId;
            }

            var updated = await dataContext.SaveChangesAsync(cancellationToken);

            return updated > 0;
        }
    }
}
