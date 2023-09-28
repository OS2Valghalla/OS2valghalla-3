using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Valghalla.Application.QueryEngine;
using Valghalla.Database;
using Valghalla.Database.Entities.Tables;
using Valghalla.Database.QueryEngine;
using Valghalla.Internal.Application.Modules.Shared.Participant.Queries;
using Valghalla.Internal.Application.Modules.Shared.Participant.Responses;

namespace Valghalla.Internal.Infrastructure.Modules.Shared.Participant
{
    internal class ParticipantSharedListingQueryRepository : QueryEngineRepository<ParticipantSharedListingQueryForm, ParticipantSharedListingItemResponse, ParticipantSharedListingQueryFormParameters, ParticipantEntity>
    {
        public ParticipantSharedListingQueryRepository(DataContext dataContext, IMapper mapper) : base(dataContext, mapper)
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

            QueryFor(x => x.Search)
                .Use((queryable, query) => queryable.Where(i =>
                    (i.FirstName != null && i.FirstName.ToLower().Contains(query.Value)) ||
                    (i.LastName != null && i.LastName.ToLower().Contains(query.Value)) ||
                    (i.MobileNumber != null && i.MobileNumber.ToLower().Contains(query.Value)) ||
                    (i.Email != null && i.Email.ToLower().Contains(query.Value))));

            QueryFor(x => x.Birthdate)
                .Use((queryable, query) =>
                {
                    var value = query.Value.ToUniversalTime();
                    return queryable.Where(i => i.Birthdate == value);
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
                    i.TeamResponsibles.Any(j => query.Values.Contains(j.TeamId))));
        }

        public override async Task<QueryResult<ParticipantSharedListingItemResponse>> ExecuteQuery(ParticipantSharedListingQueryForm form, CancellationToken cancellationToken)
        {
            var (keys, items, _) = await ApplyQuery(form).ExecuteAsync(i => i.Id, cancellationToken);
            return new QueryResult<ParticipantSharedListingItemResponse>(keys, items);
        }
    }
}
