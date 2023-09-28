using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Valghalla.Application.QueryEngine;
using Valghalla.Database;
using Valghalla.Database.Entities.Tables;
using Valghalla.Database.QueryEngine;
using Valghalla.Internal.Application.Modules.Administration.Team.Queries;
using Valghalla.Internal.Application.Modules.Administration.Team.Responses;

namespace Valghalla.Internal.Infrastructure.Modules.Administration.Team
{
    internal class TeamListingQueryRepository : QueryEngineRepository<TeamListingQueryForm, ListTeamsItemResponse, TeamListingQueryFormParameters, TeamEntity>
    {
        public TeamListingQueryRepository(DataContext dataContext, IMapper mapper) : base(dataContext, mapper)
        {
            Order((queryable, order) =>
            {
                if (order.Name == "name")
                {
                    return queryable.SortBy(i => i.Name, order);
                }

                if (order.Name == "shortName")
                {
                    return queryable.SortBy(i => i.ShortName, order);
                }

                return queryable;
            });

            Query(queryable =>
            {
                return queryable
                    .Include(i => i.TeamResponsibles);
            });

            QueryFor(x => x.Search)
                .Use((queryable, query) => queryable
                    .Where(i =>
                        i.Name.ToLower().Contains(query.Value) ||
                        i.ShortName.ToLower().Contains(query.Value)
                        ));
        }

        public override async Task<QueryResult<ListTeamsItemResponse>> ExecuteQuery(TeamListingQueryForm form, CancellationToken cancellationToken)
        {
            var (keys, items, _) = await ApplyQuery(form).ExecuteAsync(i => i.Id, cancellationToken);
            return new QueryResult<ListTeamsItemResponse>(keys, items);
        }
    }
}
