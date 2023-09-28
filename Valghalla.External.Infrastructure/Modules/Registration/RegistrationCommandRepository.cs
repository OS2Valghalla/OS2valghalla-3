using Microsoft.EntityFrameworkCore;
using Valghalla.Application.CPR;
using Valghalla.Application.User;
using Valghalla.Database;
using Valghalla.Database.Entities.Tables;
using Valghalla.External.Application.Modules.Registration.Interfaces;

namespace Valghalla.External.Infrastructure.Modules.Registration
{
    internal class RegistrationCommandRepository : IRegistrationCommandRepository
    {
        private readonly DataContext dataContext;

        private readonly DbSet<ParticipantEntity> participants;
        private readonly DbSet<UserEntity> users;
        private readonly DbSet<TeamMemberEntity> teamMembers;
        private readonly DbSet<SpecialDietParticipantEntity> specialDietParticipants;

        public RegistrationCommandRepository(DataContext dataContext)
        {
            this.dataContext = dataContext;

            participants = dataContext.Set<ParticipantEntity>();
            users = dataContext.Set<UserEntity>();
            teamMembers = dataContext.Set<TeamMemberEntity>();
            specialDietParticipants = dataContext.Set<SpecialDietParticipantEntity>();
        }

        public async Task<Guid> CreateParticipantAsync(string cpr, string? mobileNumber, string? email, IEnumerable<Guid> specialDietIds, Guid teamId, ParticipantPersonalRecord record, CancellationToken cancellationToken)
        {
            var userEntity = new UserEntity()
            {
                Id = Guid.NewGuid(),
                RoleId = SystemRole.Participant.Id,
                Name = string.Join(" ", record.FirstName, record.LastName),
                Cpr = cpr.Trim(),
            };

            var participantEntity = new ParticipantEntity
            {
                Id = Guid.NewGuid(),
                UserId = userEntity.Id,
                Cpr = cpr,
                MobileNumber = mobileNumber,
                Email = email,

                FirstName = record.FirstName,
                LastName = record.LastName,
                StreetAddress = record.StreetAddress,
                PostalCode = record.PostalCode,
                City = record.City,
                MunicipalityCode = record.MunicipalityCode,
                MunicipalityName = record.MunicipalityName,
                CountryCode = record.CountryCode,
                CountryName = record.CountryName,
                Age = record.Age,
                Birthdate = record.Birthdate,
                Deceased = record.Deceased,
                Disenfranchised = record.Disenfranchised,
                ExemptDigitalPost = record.ExemptDigitalPost,
                LastValidationDate = DateTime.UtcNow,
            };

            var teamMemberEntity = new TeamMemberEntity()
            {
                TeamId = teamId,
                ParticipantId = participantEntity.Id,
            };

            var specialDietParticipantEntities = specialDietIds.Select(specialDietId => new SpecialDietParticipantEntity()
            {
                SpecialDietId = specialDietId,
                ParticipantId = participantEntity.Id,
            });

            await users.AddAsync(userEntity, cancellationToken);
            await participants.AddAsync(participantEntity, cancellationToken);
            await teamMembers.AddAsync(teamMemberEntity, cancellationToken);
            await specialDietParticipants.AddRangeAsync(specialDietParticipantEntities, cancellationToken);
            await dataContext.SaveChangesAsync(cancellationToken);

            return participantEntity.Id;
        }

        public async Task JoinTeamAsync(Guid participantId, Guid teamId, CancellationToken cancellationToken)
        {
            var teamMemberEntity = new TeamMemberEntity()
            {
                TeamId = teamId,
                ParticipantId = participantId,
            };

            await teamMembers.AddAsync(teamMemberEntity, cancellationToken);
            await dataContext.SaveChangesAsync(cancellationToken);
        }
    }
}
