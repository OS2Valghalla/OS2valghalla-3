using AutoMapper;

using Microsoft.EntityFrameworkCore;

using Valghalla.Application.QueryEngine;
using Valghalla.Database;
using Valghalla.Database.Entities.Tables;
using Valghalla.Database.QueryEngine;
using Valghalla.Internal.Application.Modules.Administration.TaskType.Queries;
using Valghalla.Internal.Application.Modules.Administration.TaskType.Responses;

namespace Valghalla.Internal.Infrastructure.Modules.Administration.TaskType
{
    internal class TaskTypeListingQueryRepository : QueryEngineRepository<TaskTypeListingQueryForm, TaskTypeListingItemResponse, TaskTypeListingQueryFormParameters, TaskTypeEntity>
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
                else if (order.Name == "electionTitle")
                {
                    return queryable.SortBy(i => i.WorkLocationTaskTypes
                        .Select(n => n.WorkLocation.ElectionWorkLocations
                        .Select(y => y.Election.Title))
                        .FirstOrDefault(), order);
                }
                else if (order.Name == "templateTitle")
                {
                    return queryable.SortBy(i => i.TaskTypeTemplate != null ? i.TaskTypeTemplate.Title : string.Empty, order);
                }

                return queryable;
            });


            Query(queryable =>
            {
                return queryable
                    .Include(i => i.TaskTypeTemplate)
                    .Include(i => i.TaskAssignments).ThenInclude(ta => ta.Election)
                    .Include(i => i.TaskAssignments).ThenInclude(ta => ta.WorkLocation.Area)
                       .Include(i => i.WorkLocationTaskTypes).ThenInclude(wltt => wltt.WorkLocation).ThenInclude(wl => wl.Area)
                .Include(i => i.WorkLocationTaskTypes).ThenInclude(wltt => wltt.WorkLocation).ThenInclude(wl => wl.ElectionWorkLocations)
                .Include(i => i.WorkLocationTaskTypes).ThenInclude(wltt => wltt.WorkLocation).ThenInclude(wl => wl.Elections);
            });

            QueryFor(x => x.Search)
                .Use((queryable, query) => queryable
                    .Where(i =>
                        i.Title.ToLower().Contains(query.Value) ||
                        i.ShortName.ToLower().Contains(query.Value)));

            QueryFor(x => x.Area)
                .With(async request =>
                {
                    var data = await dataContext.Areas.AsNoTracking()
                        .Select(i => new { i.Id, i.Name })
                        .ToListAsync();

                    return data.Select(i => new SelectOption<Guid>(i.Id, i.Name));
                })
                .Use((queryable, query) => queryable.Where(i => i.WorkLocationTaskTypes.Any(wltt => wltt.WorkLocation.AreaId == query.Value)));

            QueryFor(x => x.Election)
                .With(async request =>
                {
                    var data = await dataContext.Elections.AsNoTracking()
                        .Select(i => new { i.Id, i.Title })
                        .ToListAsync();

                    return data.Select(i => new SelectOption<Guid>(i.Id, i.Title));
                })
                .Use((queryable, query) => queryable.Where(i => i.WorkLocationTaskTypes.Any(wltt => wltt.WorkLocation.ElectionWorkLocations.Where(ewl => ewl.ElectionId == query.Value).Any())));
        }

        public override async Task<QueryResult<TaskTypeListingItemResponse>> ExecuteQuery(TaskTypeListingQueryForm form, CancellationToken cancellationToken)
        {
            var (keys, items, _) = await ApplyQuery(form).ExecuteAsync(i => i.Id, cancellationToken);
            return new QueryResult<TaskTypeListingItemResponse>(keys, items);
        }
    }
}
