using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Valghalla.Application.QueryEngine;
using Valghalla.Database;
using Valghalla.Database.Entities.Tables;
using Valghalla.Database.Extensions;
using Valghalla.Database.QueryEngine;
using Valghalla.Internal.Application.Modules.Participant.Queries;
using Valghalla.Internal.Application.Modules.Participant.Responses;

namespace Valghalla.Internal.Infrastructure.Modules.Participant
{
    internal class ParticipantListingQueryRepository : QueryEngineRepository<ParticipantListingQueryForm, ParticipantListingItemResponse, ParticipantListingQueryFormParameters, ParticipantEntity>
    {
        public ParticipantListingQueryRepository(DataContext dataContext, IMapper mapper) : base(dataContext, mapper)
        {
            Order((queryable, order) =>
            {
                if (order.Name == "name")
                {
                    return queryable.SortBy(i => i.FirstName + " " + i.LastName, order);
                }
                else if (order.Name == "birthdate")
                {
                    return queryable.SortBy(i => i.Birthdate, order);
                }

                return queryable;
            });

            Query(queryable => queryable
                .Include(i => i.TeamMembers)
                .Include(i => i.TaskAssignments));

            QueryFor(x => x.Search)
                .Use((queryable, query) => queryable.Where(i =>
                    ((i.FirstName + " " + i.LastName).ToLower().Contains(query.Value)) ||
                    (i.MobileNumber != null && i.MobileNumber.ToLower().Contains(query.Value)) ||
                    (i.Cpr.ToLower().Contains(query.Value)) ||
                    (i.Email != null && i.Email.ToLower().Contains(query.Value))));

            QueryFor(x => x.Birthdate)
                .Use((queryable, query) =>
                {
                    var value = query.Value.ToUniversalTime();
                    return queryable.Where(i => i.Birthdate.Date == value.Date);
                });

            QueryFor(x => x.Teams)
                .With(async request =>
                {
                    var data = await dataContext.Teams.AsNoTracking()
                        .Select(i => new { i.Id, i.Name })
                        .OrderBy(i => i.Name)
                        .ToArrayAsync();
                    return data.Select(i => new SelectOption<Guid>(i.Id, i.Name));
                })
                .Use((queryable, query) => queryable.Where(i =>
                    i.TeamMembers.Any(j => query.Values.Contains(j.TeamId))));

            QueryFor(x => x.DigitalPost)
                .Use((queryable, query) => queryable.Where(i => i.ExemptDigitalPost == !query.Value));

            QueryFor(x => x.AssignedTask)
                .Use((queryable, query) => query.Value ?
                    queryable.Where(i => i.TaskAssignments.Any()) :
                    queryable.Where(i => !i.TaskAssignments.Any()));

            QueryFor(x => x.TaskTypes)
                .With(async request =>
                {
                    var data = await dataContext.TaskTypes.AsNoTracking()
                        .Select(i => new { i.Id, i.Title })
                        .OrderBy(i => i.Title)
                        .ToArrayAsync();
                    return data.Select(i => new SelectOption<Guid>(i.Id, i.Title));
                })
                .Use((queryable, query) => queryable.Where(i =>
                    i.TaskAssignments.Any(j => query.Values.Contains(j.TaskTypeId))));
        }

        public override async Task<QueryResult<ParticipantListingItemResponse>> ExecuteQuery(ParticipantListingQueryForm form, CancellationToken cancellationToken)
        {
            var (keys, items, _) = await ApplyQuery(form).ExecuteAsync(i => i.Id, cancellationToken);
            return new QueryResult<ParticipantListingItemResponse>(keys, items);
        }
    }
}
