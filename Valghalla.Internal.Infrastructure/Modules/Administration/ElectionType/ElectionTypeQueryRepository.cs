using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Valghalla.Database;
using Valghalla.Database.Entities.Tables;
using Valghalla.Internal.Application.Modules.Administration.ElectionType.Interfaces;
using Valghalla.Internal.Application.Modules.Administration.ElectionType.Queries;
using Valghalla.Internal.Application.Modules.Administration.ElectionType.Responses;

namespace Valghalla.Internal.Infrastructure.Modules.Administration.ElectionType
{
    public class ElectionTypeQueryRepository: IElectionTypeQueryRepository
    {
        private readonly IMapper mapper;
        private readonly IQueryable<ElectionTypeEntity> electionTypes;
        public ElectionTypeQueryRepository(DataContext dataContext, IMapper mapper) 
        {
            electionTypes = dataContext.Set<ElectionTypeEntity>().AsNoTracking();
            this.mapper = mapper;
        }

        public async Task<ElectionTypeResponse?> GetElectionTypeAsync(GetElectionTypeQuery query, CancellationToken cancellationToken)
        {
            var entity = await electionTypes.Include(e => e.ValidationRules).SingleOrDefaultAsync(x => x.Id == query.Id, cancellationToken);

            if (entity == null) return null;
            return mapper.Map<ElectionTypeResponse>(entity);
        }

        public async Task<bool> CheckIfElectionTypeCanBeDeletedAsync(Guid electionTypeId, CancellationToken cancellationToken)
        {
            var exist = await electionTypes.Include(e => e.Elections).AnyAsync(x => x.Id == electionTypeId && x.Elections.Any(), cancellationToken);
            return !exist;
        }
    }
}
