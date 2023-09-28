using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Valghalla.Database;
using Valghalla.Database.Entities.Tables;
using Valghalla.Internal.Application.Modules.Shared.Participant.Interfaces;
using Valghalla.Internal.Application.Modules.Shared.Participant.Queries;
using Valghalla.Internal.Application.Modules.Shared.Participant.Responses;

namespace Valghalla.Internal.Infrastructure.Modules.Shared.Participant
{
    internal class ParticipantSharedQueryRepository : IParticipantSharedQueryRepository
    {
        private readonly IMapper mapper;
        private readonly IQueryable<ParticipantEntity> participants;

        public ParticipantSharedQueryRepository(DataContext dataContext, IMapper mapper)
        {
            this.mapper = mapper;
            participants = dataContext.Set<ParticipantEntity>().AsNoTracking();
        }

        public async Task<IEnumerable<ParticipantSharedResponse>> GetPariticipantsAsync(GetParticipantsSharedQuery query, CancellationToken cancellationToken)
        {
            var entities = await participants
                .Where(i => query.Values.Contains(i.Id))
                .ToArrayAsync(cancellationToken);

            return entities.Select(mapper.Map<ParticipantSharedResponse>);
        }
    }
}
