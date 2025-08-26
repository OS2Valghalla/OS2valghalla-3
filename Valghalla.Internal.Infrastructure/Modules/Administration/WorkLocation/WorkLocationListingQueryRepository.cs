using AutoMapper;

using Microsoft.EntityFrameworkCore;

using Valghalla.Application.QueryEngine;
using Valghalla.Database;
using Valghalla.Database.Entities.Tables;
using Valghalla.Database.QueryEngine;
using Valghalla.Internal.Application.Modules.Administration.WorkLocation.Queries;
using Valghalla.Internal.Application.Modules.Administration.WorkLocation.Responses;

namespace Valghalla.Internal.Infrastructure.Modules.Administration.WorkLocation
{
    // Specific repository to handle apply query engine
    // QueryEngineRepository contains a number of fluent api like methods to build query expression
    // All api is expression and is only called if a query properties exists in query from
    internal class WorkLocationListingQueryRepository : QueryEngineRepository<WorkLocationListingQueryForm, WorkLocationResponse, WorkLocationListingQueryFormParameters, WorkLocationEntity>
    {
        private readonly IMapper mapper;

        public WorkLocationListingQueryRepository(DataContext dataContext, IMapper mapper) : base(dataContext, mapper)
        {
            this.mapper = mapper;

            // set query order
            Order((queryable, order) =>
            {
                if (order.Name == "title")
                {
                    return queryable.SortBy(i => i.Title, order);
                }
                else if (order.Name == "areaName")
                {
                    return queryable.SortBy(i => i.Area.Name, order);
                }

                return queryable;
            });

            Query(queryable =>
            {
                return queryable
                    .Include(i => i.Area);
            });

            // QueryFor is to identity which property to build query expression
            // Chain method "Use" is to build queryable entity

            QueryFor(x => x.Search)
                .Use((queryable, query) => queryable.Where(i =>
                    i.Title.ToLower().Contains(query.Value) ||
                    i.Area.Name.ToLower().Contains(query.Value)
                    ));

            // For built-in property like SingleSelectionFilterValue & MultipleSelectionFiterValue
            // it will contain "With" method to build filter data for that property

            QueryFor(x => x.Title)
                .Use((queryable, query) => queryable.Where(i => i.Title.Contains(query.Value)));

            QueryFor(x => x.PostalCode)
                .Use((queryable, query) => queryable.Where(i => i.PostalCode!.Contains(query.Value)));

            QueryFor(x => x.Area)
                .With(async request =>
                {
                    var data = await dataContext.Areas.AsNoTracking()
                        .Select(i => new { i.Id, i.Name })
                        .ToListAsync();

                    return data.Select(i => new SelectOption<Guid>(i.Id, i.Name));
                })
                .Use((queryable, query) => queryable.Where(i => i.AreaId == query.Value));
        }

        // All repository need to overwrite "ExecuteQuery" method, this will be called by query engine handler
        public override async Task<QueryResult<WorkLocationResponse>> ExecuteQuery(WorkLocationListingQueryForm form, CancellationToken cancellationToken)
        {
            // ApplyQuery will take care all expression build in constructor automatically as well as paging and order
            // ExecuteAsync will trigger EF query
            var (keys, items, _) = await ApplyQuery(form).ExecuteAsync(i => i.Id, cancellationToken);

            return new QueryResult<WorkLocationResponse>(keys, items);
        }
    }
}
