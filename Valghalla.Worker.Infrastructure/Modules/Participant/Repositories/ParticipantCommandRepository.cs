using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Valghalla.Application.TaskValidation;
using Valghalla.Database;
using Valghalla.Database.Entities.Tables;
using Valghalla.Worker.Infrastructure.Modules.Participant.Requests;

namespace Valghalla.Worker.Infrastructure.Modules.Participant.Repositories
{
    public interface IParticipantCommandRepository
    {
        Task<IEnumerable<EvaluatedParticipant>> UpdateRecordsAsync(IEnumerable<ParticipantSyncJobItem> items, CancellationToken cancellationToken);
    }

    internal class ParticipantCommandRepository : IParticipantCommandRepository
    {
        private readonly IMapper mapper;
        private readonly DataContext dataContext;
        private readonly DbSet<ParticipantEntity> participants;

        public ParticipantCommandRepository(DataContext dataContext, IMapper mapper)
        {
            this.mapper = mapper;
            this.dataContext = dataContext;
            participants = dataContext.Set<ParticipantEntity>();
        }

        public async Task<IEnumerable<EvaluatedParticipant>> UpdateRecordsAsync(IEnumerable<ParticipantSyncJobItem> items, CancellationToken cancellationToken)
        {
            var participantIds = items.Select(i => i.ParticipantId).ToArray();
            var entities = await participants
                .Where(i => participantIds.Contains(i.Id))
                .ToArrayAsync(cancellationToken);

            foreach (var entity in entities)
            {
                var record = items.Single(i => i.ParticipantId == entity.Id).Record;

                entity.FirstName = record.FirstName;
                entity.LastName = record.LastName;
                entity.StreetAddress = record.StreetAddress;
                entity.PostalCode = record.PostalCode;
                entity.MunicipalityCode = record.MunicipalityCode;
                entity.MunicipalityName = record.MunicipalityName;
                entity.CountryCode = record.CountryCode;
                entity.CountryName = record.CountryName;
                entity.Age = record.Age;
                entity.Birthdate = record.Birthdate;
                entity.Deceased = record.Deceased;
                entity.Disenfranchised = record.Disenfranchised;
                entity.ExemptDigitalPost = record.ExemptDigitalPost;

                entity.LastValidationDate = DateTime.UtcNow;
            }

            participants.UpdateRange(entities);

            await dataContext.SaveChangesAsync(cancellationToken);

            return entities.Select(mapper.Map<EvaluatedParticipant>).ToArray();
        }
    }
}
