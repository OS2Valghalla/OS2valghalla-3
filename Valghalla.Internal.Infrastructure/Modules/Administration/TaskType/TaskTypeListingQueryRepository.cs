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

            Order((queryable, order) => {

                return order.Name?.ToLower() switch
                {

                    "title" => queryable.SortBy(i => i.Title, order),

                    "shortname" => queryable.SortBy(i => i.ShortName, order),

                    "shortName" => queryable.SortBy(i => i.ShortName, order), // safeguard in case casing retained
                    "trusted" => queryable.SortBy(i => i.Trusted, order),

                    "electiontitle" => queryable.SortBy(i => i.WorkLocationTaskTypes

                        .Select(n => n.WorkLocation.ElectionWorkLocations

                            .Select(y => y.Election.Title))

                        .FirstOrDefault(), order),

                    "electionTitle" => queryable.SortBy(i => i.WorkLocationTaskTypes

                        .Select(n => n.WorkLocation.ElectionWorkLocations

                            .Select(y => y.Election.Title))

                        .FirstOrDefault(), order), // safeguard
                        _ => queryable                
                };
                }
            );



            Query(queryable => {

                return queryable.Include(i => i.TaskAssignments).ThenInclude(ta => ta.Election)

                    .Include(i => i.TaskAssignments).ThenInclude(ta => ta.WorkLocation.Area)

                    .Include(i => i.WorkLocationTaskTypes).ThenInclude(wltt => wltt.WorkLocation).ThenInclude(wl => wl.Area)

                    .Include(i => i.WorkLocationTaskTypes).ThenInclude(wltt => wltt.WorkLocation).ThenInclude(wl => wl.ElectionWorkLocations)

                    .Include(i => i.WorkLocationTaskTypes).ThenInclude(wltt => wltt.WorkLocation).ThenInclude(wl => wl.Elections);

            });


            QueryFor(x => x.Search)

                .Use((queryable, query) => {

                    var value = query.Value.ToLower();

                    return queryable.Where(i => i.Title.ToLower().Contains(value) || i.ShortName.ToLower().Contains(value));

                });


            QueryFor(x => x.Area)

                .With(async request => {

                    var data = await dataContext.Areas.AsNoTracking()

                        .Select(i => new { i.Id, i.Name })

                        .ToListAsync();

                    return data.Select(i => new SelectOption<Guid>(i.Id, i.Name));

                })

                .Use((queryable, query) => queryable.Where(i => i.WorkLocationTaskTypes.Select(wl => wl.WorkLocation.AreaId).Contains(query.Value)));

        }


        public override async Task<QueryResult<TaskTypeListingItemResponse>> ExecuteQuery(TaskTypeListingQueryForm form, CancellationToken cancellationToken)

        {

            var (keys, items, _) = await ApplyQuery(form).ExecuteAsync(i => i.Id, cancellationToken);

            return new QueryResult<TaskTypeListingItemResponse>(keys, items);

        }

    }

}