using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Valghalla.Database;
using Valghalla.Database.Entities.Tables;
using Valghalla.Internal.Application.Modules.Administration.SpecialDiet.Commands;
using Valghalla.Internal.Application.Modules.Administration.SpecialDiet.Interfaces;
using Valghalla.Internal.Application.Modules.Administration.SpecialDiet.Queries;
using Valghalla.Internal.Application.Modules.Administration.SpecialDiet.Responses;

namespace Valghalla.Internal.Infrastructure.Modules.Administration.SpecialDiet
{
    internal class SpecialDietQueryRepository : ISpecialDietQueryRepository
    {
        private readonly IQueryable<SpecialDietEntity> specialDiets;
        private readonly IQueryable<ElectionEntity> election;
        private readonly IMapper mapper;

        public SpecialDietQueryRepository(DataContext dataContext, IMapper mapper)
        {
            specialDiets = dataContext.Set<SpecialDietEntity>().AsNoTracking();
            election = dataContext.Set<ElectionEntity>().AsNoTracking();
            this.mapper = mapper;
        }

        public async Task<bool> CheckIfSpecialDietExistsAsync(CreateSpecialDietCommand command, CancellationToken cancellationToken)
        {
            var title = command.Title.Trim().ToLower();
            return await specialDiets.AnyAsync(i => i.Title!.ToLower() == title, cancellationToken);
        }
        public async Task<bool> CheckIfSpecialDietExistsAsync(UpdateSpecialDietCommand command, CancellationToken cancellationToken)
        {
            var title = command.Title.Trim().ToLower();
            return await specialDiets.AnyAsync(i => i.Id != command.Id && i.Title!.ToLower() == title, cancellationToken);
        }

        public async Task<bool> CheckHasActiveElectionAsync(CancellationToken cancellationToken)
        {
            return await election.AnyAsync(x => x.Active, cancellationToken);
        }

        public async Task<SpecialDietResponse?> GetSpecialDietAsync(GetSpecialDietQuery query, CancellationToken cancellationToken)
        {
            var entity = await specialDiets.SingleOrDefaultAsync(i => i.Id == query.SpecialDietId, cancellationToken);
            if (entity == null) return null;
            return mapper.Map<SpecialDietResponse>(entity);
        }
    }
}
