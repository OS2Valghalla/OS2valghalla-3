using AutoMapper;
using Valghalla.Application.QueryEngine;
using Valghalla.Database;
using Valghalla.Database.Entities.Tables;
using Valghalla.Database.QueryEngine;
using Valghalla.Internal.Application.Modules.Administration.Area.Queries;
using Valghalla.Internal.Application.Modules.Administration.Area.Responses;

namespace Valghalla.Internal.Infrastructure.Modules.Administration.Area
{
    internal class AreaListingQueryRepository : QueryEngineRepository<AreaListingQueryForm, AreaListingItemResponse, VoidQueryFormParameters, AreaEntity>
    {
        public AreaListingQueryRepository(DataContext dataContext, IMapper mapper) : base(dataContext, mapper)
        {
            Order((queryable, order) =>
            {
                if (order.Name == "name")
                {
                    return queryable.SortBy(i => i.Name, order);
                }

                return queryable;
            });

            QueryFor(x => x.Search)
                .Use((queryable, query) => queryable
                    .Where(i =>
                        i.Name.ToLower().Contains(query.Value)));
        }

        public override async Task<QueryResult<AreaListingItemResponse>> ExecuteQuery(AreaListingQueryForm form, CancellationToken cancellationToken)
        {
            var (keys, items, _) = await ApplyQuery(form).ExecuteAsync(i => i.Id, cancellationToken);
            return new QueryResult<AreaListingItemResponse>(keys, items);
        }
    }
}
