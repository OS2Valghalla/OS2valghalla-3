using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Valghalla.Database;
using Valghalla.Database.Entities.Tables;
using Valghalla.Worker.Infrastructure.Modules.Participant.Responses;

namespace Valghalla.Worker.Infrastructure.Modules.Participant.Repositories
{
    public interface IParticipantQueryRepository
    {
        Task<IEnumerable<ParticipantResponse>> GetParticipantsAsync(CancellationToken cancellationToken);
    }

    internal class ParticipantQueryRepository : IParticipantQueryRepository
    {
        public readonly IMapper mapper;
        public readonly IQueryable<ParticipantEntity> participants;

        public ParticipantQueryRepository(DataContext dataContext, IMapper mapper)
        {
            this.mapper = mapper;
            participants = dataContext.Set<ParticipantEntity>().AsNoTracking();
        }

        public async Task<IEnumerable<ParticipantResponse>> GetParticipantsAsync(CancellationToken cancellationToken)
        {
            var entities = await participants.ToArrayAsync(cancellationToken);
            return entities.Select(mapper.Map<ParticipantResponse>).ToArray();
        }
    }
}
