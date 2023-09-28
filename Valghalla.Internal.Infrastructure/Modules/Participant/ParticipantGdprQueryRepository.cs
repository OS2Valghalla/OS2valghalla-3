using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Valghalla.Database;
using Valghalla.Database.Entities.Tables;
using Valghalla.Internal.Application.Modules.Participant.Commands;
using Valghalla.Internal.Application.Modules.Participant.Interfaces;

namespace Valghalla.Internal.Infrastructure.Modules.Participant
{
    internal class ParticipantGdprQueryRepository : IParticipantGdprQueryRepository
    {
        private readonly IQueryable<TeamResponsibleEntity> teamResponsibles;
        private readonly IQueryable<WorkLocationResponsibleEntity> workLocationResponsibles;
        private readonly IQueryable<TaskAssignmentEntity> taskAssignments;

        public ParticipantGdprQueryRepository(DataContext dataContext)
        {
            taskAssignments = dataContext.Set<TaskAssignmentEntity>().AsNoTracking();
            teamResponsibles = dataContext.Set<TeamResponsibleEntity>().AsNoTracking();
            workLocationResponsibles = dataContext.Set<WorkLocationResponsibleEntity>().AsNoTracking();
        }

        public async Task<bool> CheckIfParticipantHasAssignedTasksInActiveElectionAsync(DeleteParticipantCommand command, CancellationToken cancellationToken)
        {
            return await taskAssignments
                .Where(i =>
                    i.ParticipantId == command.Id &&
                    i.Election.Active &&
                    i.Responsed &&
                    i.Accepted)
                .AnyAsync(cancellationToken);
        }

        public async Task<bool> CheckIfParticipantIsTeamResponsibleAsync(DeleteParticipantCommand command, CancellationToken cancellationToken)
        {
            return await teamResponsibles
                .Where(i => i.ParticipantId == command.Id)
                .AnyAsync(cancellationToken);
        }

        public async Task<bool> CheckIfParticipantIsWorkLocationResponsibleAsync(DeleteParticipantCommand command, CancellationToken cancellationToken)
        {
            return await workLocationResponsibles
                .Where(i => i.ParticipantId == command.Id)
                .AnyAsync(cancellationToken);
        }

        public async Task<bool> CheckIfParticipantsContainAssignedTasksInActiveElectionAsync(BulkDeleteParticipantsCommand command, CancellationToken cancellationToken)
        {
            return await taskAssignments
                .Where(i =>
                    i.ParticipantId != null &&
                    command.ParticipantIds.Contains(i.ParticipantId.Value) &&
                    i.Election.Active &&
                    i.Responsed &&
                    i.Accepted)
                .AnyAsync(cancellationToken);
        }

        public async Task<bool> CheckIfParticipantsAreTeamResponsiblesAsync(BulkDeleteParticipantsCommand command, CancellationToken cancellationToken)
        {
            return await teamResponsibles
                .Where(i => command.ParticipantIds.Contains(i.ParticipantId))
                .AnyAsync(cancellationToken);
        }

        public async Task<bool> CheckIfParticipantsAreWorkLocationResponsiblesAsync(BulkDeleteParticipantsCommand command, CancellationToken cancellationToken)
        {
            return await workLocationResponsibles
                .Where(i => command.ParticipantIds.Contains(i.ParticipantId))
                .AnyAsync(cancellationToken);
        }
    }
}
