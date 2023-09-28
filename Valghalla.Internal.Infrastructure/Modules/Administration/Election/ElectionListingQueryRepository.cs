using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Valghalla.Application.QueryEngine;
using Valghalla.Database;
using Valghalla.Database.Entities.Tables;
using Valghalla.Database.QueryEngine;
using Valghalla.Internal.Application.Modules.Administration.Election.Queries;
using Valghalla.Internal.Application.Modules.Administration.Election.Responses;

namespace Valghalla.Internal.Infrastructure.Modules.Administration.Election
{
    internal class ElectionListingQueryRepository : QueryEngineRepository<ElectionListingQueryForm, ElectionListingItemResponse, VoidQueryFormParameters, ElectionEntity>
    {
        public ElectionListingQueryRepository(DataContext dataContext, IMapper mapper) : base(dataContext, mapper)
        {
            Order((queryable, order) =>
            {
                if (order.Name == "title")
                {
                    return queryable.SortBy(i => i.Title, order);
                }
                else if (order.Name == "electionDate")
                {
                    return queryable.SortBy(i => i.ElectionDate, order);
                }
                else if (order.Name == "active")
                {
                    return queryable.SortBy(i => i.Active, order);
                }
                else if (order.Name == "electionTypeName")
                {
                    return queryable.SortBy(i => i.ElectionType.Title, order);
                }

                return queryable;
            });

            Query(queryable =>
            {
                return queryable
                    .Include(i => i.ElectionType);
            });

            QueryFor(x => x.Search)
                .Use((queryable, query) => queryable
                    .Where(i =>
                        i.Title.ToLower().Contains(query.Value)));
        }

        public override async Task<QueryResult<ElectionListingItemResponse>> ExecuteQuery(ElectionListingQueryForm form, CancellationToken cancellationToken)
        {
            var (keys, items, _) = await ApplyQuery(form).ExecuteAsync(i => i.Id, cancellationToken);
            return new QueryResult<ElectionListingItemResponse>(keys, items);
        }
    }
}
