using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Valghalla.Database;
using Valghalla.Database.Entities.Tables;
using Valghalla.External.Application.Modules.Shared.SpecialDiet.Interfaces;
using Valghalla.External.Application.Modules.Shared.SpecialDiet.Queries;
using Valghalla.External.Application.Modules.Shared.SpecialDiet.Responses;

namespace Valghalla.External.Infrastructure.Modules.Shared.SpecialDiet
{
    internal class SpecialDietSharedQueryRepository : ISpecialDietSharedQueryRepository
    {
        private readonly IMapper mapper;
        private readonly IQueryable<SpecialDietEntity> specialDiets;

        public SpecialDietSharedQueryRepository(DataContext dataContext, IMapper mapper)
        {
            this.mapper = mapper;
            specialDiets = dataContext.Set<SpecialDietEntity>().AsNoTracking();
        }

        public async Task<IEnumerable<SpecialDietSharedResponse>> GetSpecialDietsAsync(GetSpecialDietsSharedQuery query, CancellationToken cancellationToken)
        {
            var entities = await specialDiets
                .OrderBy(i => i.Title)
                .ToArrayAsync(cancellationToken);

            return entities.Select(mapper.Map<SpecialDietSharedResponse>);
        }
    }
}
