using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Valghalla.Database;
using Valghalla.Database.Entities.Tables;
using Valghalla.External.Application.Modules.Registration.Interfaces;
using Valghalla.External.Application.Modules.Registration.Queries;
using Valghalla.External.Application.Modules.Registration.Responses;

namespace Valghalla.External.Infrastructure.Modules.Registration
{
    internal class RegistrationQueryRepository : IRegistrationQueryRepository
    {
        private readonly IMapper mapper;
        private readonly IQueryable<ParticipantEntity> participants;
        private readonly IQueryable<UserEntity> users;
        private readonly IQueryable<TeamLinkEntity> teamLinks;
        private readonly IQueryable<TaskAssignmentEntity> taskAssignments;
        private readonly IQueryable<TeamMemberEntity> teamMembers;
        private readonly IQueryable<TeamResponsibleEntity> teamResponsibles;

        public RegistrationQueryRepository(DataContext dataContext, IMapper mapper)
        {
            this.mapper = mapper;
            participants = dataContext.Set<ParticipantEntity>().AsNoTracking();
            users = dataContext.Set<UserEntity>().AsNoTracking();
            teamLinks = dataContext.Set<TeamLinkEntity>().AsNoTracking();
            taskAssignments = dataContext.Set<TaskAssignmentEntity>().AsNoTracking();
            teamMembers = dataContext.Set<TeamMemberEntity>().AsNoTracking();
            teamResponsibles = dataContext.Set<TeamResponsibleEntity>().AsNoTracking();
        }

        public async Task<MyProfileRegistrationResponse> GetMyProfileRegistrationAsync(Guid participantId, CancellationToken cancellationToken)
        {
            var entity = await participants
                .Include(i => i.TeamForMembers)
                .Include(i => i.SpecialDietParticipants)
                .Where(i => i.Id == participantId)
                .SingleAsync(cancellationToken);

            return mapper.Map<MyProfileRegistrationResponse>(entity);
        }

        public async Task<Guid> GetTeamIdFromLink(string hashValue, CancellationToken cancellationToken)
        {
            var entity = await teamLinks
                .Where(i => i.HashValue == hashValue)
                .SingleAsync(cancellationToken);

            return new Guid(entity.Value!);
        }

        public async Task<Guid?> GetTeamIdFromTask(string hashValue, Guid? invitationCode, CancellationToken cancellationToken)
        {
            var queryable = taskAssignments.Where(i => i.HashValue == hashValue);

            if (invitationCode.HasValue)
            {
                queryable = queryable.Where(i => i.InvitationCode == invitationCode);
            }

            var entity = await queryable.FirstOrDefaultAsync(cancellationToken);

            return entity?.TeamId;
        }
        public async Task<bool> CheckIfTeamExistsFromLink(string hashValue, CancellationToken cancellationToken)
        {
            return await teamLinks
                .Where(i => i.HashValue == hashValue)
                .AnyAsync(cancellationToken);
        }

        public async Task<bool> CheckIfTeamExistsFromTask(string hashValue, CancellationToken cancellationToken)
        {
            return await taskAssignments
                .Where(i => i.HashValue == hashValue)
                .AnyAsync(cancellationToken);
        }

        public async Task<bool> CheckIfCurrentUserJoinedTeam(GetTeamAccessStatusQuery query, Guid participantId, CancellationToken cancellationToken)
        {
            var teamIdString = await teamLinks
                .Where(i => i.HashValue == query.HashValue)
                .Select(i => i.Value!)
                .SingleAsync(cancellationToken);

            var teamId = new Guid(teamIdString);

            var joinedAsMember = await teamMembers
                .Where(i => i.ParticipantId == participantId && i.TeamId == teamId)
                .AnyAsync(cancellationToken);

            if (joinedAsMember) return true;

            var joinedAsTeamResponsible = await teamResponsibles
                .Where(i => i.ParticipantId == participantId && i.TeamId == teamId)
                .AnyAsync(cancellationToken);

            return joinedAsTeamResponsible;
        }

        public async Task<bool> CheckIfParticipantExistsAsync(string cprNumber, CancellationToken cancellationToken)
        {
            var cpr = cprNumber.Trim().ToLower();

            var participantExists = await participants
                .AnyAsync(i => i.Cpr.ToLower() == cpr, cancellationToken);

            var userExists = await users
                .AnyAsync(i => i.Cpr != null && i.Cpr.ToLower() == cpr, cancellationToken);

            return participantExists || userExists;
        }
    }
}
