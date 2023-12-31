﻿using AutoMapper;
using Valghalla.Application.QueryEngine;
using Valghalla.Database;
using Valghalla.Database.Entities.Tables;
using Valghalla.Database.QueryEngine;
using Valghalla.Internal.Application.Modules.Administration.TaskType.Queries;
using Valghalla.Internal.Application.Modules.Administration.TaskType.Responses;

namespace Valghalla.Internal.Infrastructure.Modules.Administration.TaskType
{
    internal class TaskTypeListingQueryRepository : QueryEngineRepository<TaskTypeListingQueryForm, TaskTypeListingItemResponse, VoidQueryFormParameters, TaskTypeEntity>
    {
        public TaskTypeListingQueryRepository(DataContext dataContext, IMapper mapper) : base(dataContext, mapper)
        {
            Order((queryable, order) =>
            {
                if (order.Name == "title")
                {
                    return queryable.SortBy(i => i.Title, order);
                }
                else if (order.Name == "shortName")
                {
                    return queryable.SortBy(i => i.ShortName, order);
                }
                else if (order.Name == "trusted")
                {
                    return queryable.SortBy(i => i.Trusted, order);
                }

                return queryable;
            });

            QueryFor(x => x.Search)
                .Use((queryable, query) => queryable
                    .Where(i =>
                        i.Title.ToLower().Contains(query.Value) ||
                        i.ShortName.ToLower().Contains(query.Value)));
        }

        public override async Task<QueryResult<TaskTypeListingItemResponse>> ExecuteQuery(TaskTypeListingQueryForm form, CancellationToken cancellationToken)
        {
            var (keys, items, _) = await ApplyQuery(form).ExecuteAsync(i => i.Id, cancellationToken);
            return new QueryResult<TaskTypeListingItemResponse>(keys, items);
        }
    }
}
