using AutoMapper;
using Valghalla.Application.QueryEngine;
using Valghalla.Database;
using Valghalla.Database.Entities.Tables;
using Valghalla.Database.QueryEngine;
using Valghalla.Internal.Application.Modules.Administration.ElectionType.Queries;
using Valghalla.Internal.Application.Modules.Administration.ElectionType.Responses;

namespace Valghalla.Internal.Infrastructure.Modules.Administration.ElectionType
{
    internal class ElectionTypeListingQueryRepository : QueryEngineRepository<ElectionTypeListingQueryForm, ElectionTypeResponse, VoidQueryFormParameters, ElectionTypeEntity>
    {
        public ElectionTypeListingQueryRepository(DataContext dataContext, IMapper mapper) : base(dataContext, mapper)
        {
            Order((queryable, order) =>
            {
                if (order.Name == "title")
                {
                    return queryable.SortBy(i => i.Title, order);
                }

                return queryable;
            });

            QueryFor(x => x.Search)
                .Use((queryable, query) => queryable
                    .Where(i =>
                        i.Title.ToLower().Contains(query.Value)));
        }

        public override async Task<QueryResult<ElectionTypeResponse>> ExecuteQuery(ElectionTypeListingQueryForm form, CancellationToken cancellationToken)
        {
            var (keys, items, _) = await ApplyQuery(form).ExecuteAsync(i => i.Id, cancellationToken);
            return new QueryResult<ElectionTypeResponse>(keys, items);
        }
    }
}
