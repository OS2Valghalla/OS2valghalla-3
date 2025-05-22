using Microsoft.EntityFrameworkCore;
using Valghalla.Application.CPR;
using Valghalla.Application.User;
using Valghalla.Database;
using Valghalla.Database.Entities.Tables;
using Valghalla.Internal.Application.Modules.Participant.Commands;
using Valghalla.Internal.Application.Modules.Participant.Interfaces;

namespace Valghalla.Internal.Infrastructure.Modules.Participant
{
    internal class ParticipantCommandRepository : IParticipantCommandRepository
    {
        private readonly DataContext dataContext;

        private readonly DbSet<ParticipantEntity> participants;
        private readonly DbSet<UserEntity> users;
        private readonly DbSet<TeamMemberEntity> teamMembers;
        private readonly DbSet<SpecialDietParticipantEntity> specialDietParticipants;

        public ParticipantCommandRepository(DataContext dataContext)
        {
            this.dataContext = dataContext;

            participants = dataContext.Set<ParticipantEntity>();
            users = dataContext.Set<UserEntity>();
            teamMembers = dataContext.Set<TeamMemberEntity>();
            specialDietParticipants = dataContext.Set<SpecialDietParticipantEntity>();
        }

        public async Task<Guid> CreateParticipantAsync(CreateParticipantCommand command, ParticipantPersonalRecord record, CancellationToken cancellationToken)
        {
            var userEntity = new UserEntity()
            {
                Id = Guid.NewGuid(),
                RoleId = SystemRole.Participant.Id,
                Name = string.Join(" ", record.FirstName, record.LastName),
                Cpr = command.Cpr.Trim(),
            };

            var participantEntity = new ParticipantEntity
            {
                Id = Guid.NewGuid(),
                UserId = userEntity.Id,
                Cpr = command.Cpr,
                MobileNumber = command.MobileNumber,
                Email = command.Email,

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
                ProtectedAddress = record.ProtectedAddress,
                LastValidationDate = DateTime.UtcNow,
            };

            var teamMemberEntities = command.TeamIds.Select(teamId => new TeamMemberEntity()
            {
                TeamId = teamId,
                ParticipantId = participantEntity.Id,
            });

            var specialDietParticipantEntities = command.SpecialDietIds.Select(specialDietId => new SpecialDietParticipantEntity()
            {
                SpecialDietId = specialDietId,
                ParticipantId = participantEntity.Id,
            });

            await users.AddAsync(userEntity, cancellationToken);
            await participants.AddAsync(participantEntity, cancellationToken);
            await teamMembers.AddRangeAsync(teamMemberEntities, cancellationToken);
            await specialDietParticipants.AddRangeAsync(specialDietParticipantEntities, cancellationToken);
            await dataContext.SaveChangesAsync(cancellationToken);

            return participantEntity.Id;
        }

        public async Task UpdateParticipantAsync(UpdateParticipantCommand command, CancellationToken cancellationToken)
        {
            var participantEntity = await participants.SingleAsync(i => i.Id == command.Id, cancellationToken);

            var teamMemberEntities = await teamMembers
                .Where(i => i.ParticipantId == command.Id)
                .ToArrayAsync(cancellationToken);

            var specialDietParticipantEntities = await specialDietParticipants
                .Where(i => i.ParticipantId == command.Id)
                .ToArrayAsync(cancellationToken);

            participantEntity.MobileNumber = command.MobileNumber;
            participantEntity.Email = command.Email;

            var teamMemberEntitiesToAdd = command.TeamIds
                .Where(teamId => !teamMemberEntities.Any(i => i.TeamId == teamId))
                .Select(teamId => new TeamMemberEntity()
                {
                    ParticipantId = command.Id,
                    TeamId = teamId,
                });

            var teamMemberEntitiesToRemove = teamMemberEntities
                .Where(i => !command.TeamIds.Any(teamId => teamId == i.TeamId));

            var specialDietParticipantEntitiesToAdd = command.SpecialDietIds
                .Where(specialDietId => !specialDietParticipantEntities.Any(i => i.SpecialDietId == specialDietId))
                .Select(specialDietId => new SpecialDietParticipantEntity()
                {
                    ParticipantId = command.Id,
                    SpecialDietId = specialDietId,
                });

            var specialDietParticipantEntitiesToRemove = specialDietParticipantEntities
                .Where(i => !command.SpecialDietIds.Any(specialDietId => specialDietId == i.SpecialDietId));

            participants.Update(participantEntity);

            if (teamMemberEntitiesToRemove.Any())
            {
                teamMembers.RemoveRange(teamMemberEntitiesToRemove);
            }

            if (teamMemberEntitiesToAdd.Any())
            {
                await teamMembers.AddRangeAsync(teamMemberEntitiesToAdd, cancellationToken);
            }

            if (specialDietParticipantEntitiesToRemove.Any())
            {
                specialDietParticipants.RemoveRange(specialDietParticipantEntitiesToRemove);
            }

            if (specialDietParticipantEntitiesToAdd.Any())
            {
                await specialDietParticipants.AddRangeAsync(specialDietParticipantEntitiesToAdd, cancellationToken);
            }

            await dataContext.SaveChangesAsync(cancellationToken);
        }
    }
}
