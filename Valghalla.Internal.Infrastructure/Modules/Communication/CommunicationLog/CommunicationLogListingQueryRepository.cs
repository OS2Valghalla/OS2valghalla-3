using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Valghalla.Application.Communication;
using Valghalla.Application.QueryEngine;
using Valghalla.Database;
using Valghalla.Database.Entities.Tables;
using Valghalla.Database.QueryEngine;
using Valghalla.Internal.Application.Modules.Communication.CommunicationLog.Queries;

namespace Valghalla.Internal.Infrastructure.Modules.Communication.CommunicationLog
{
    internal class CommunicationLogListingQueryRepository : QueryEngineRepository<CommunicationLogListingQueryForm, CommunicationLogListingItem, CommunicationLogListingQueryFormParameters, CommunicationLogEntity>
    {
        public CommunicationLogListingQueryRepository(DataContext dataContext, IMapper mapper) : base(dataContext, mapper)
        {
            Order((queryable, order) =>
            {
                if (order.Name == "participantName")
                {
                    return queryable.SortBy(i => i.Participant.FirstName + " " + i.Participant.LastName, order);
                }
                else if (order.Name == "createdAt")
                {
                    return queryable.SortBy(i => i.CreatedAt, order);
                }

                return queryable;
            });

            Query(queryable => queryable.Include(i => i.Participant));

            QueryFor(x => x.Search)
                .Use((queryable, query) => queryable.Where(i =>
                    (i.Participant.FirstName != null && i.Participant.FirstName.ToLower().Contains(query.Value) ||
                    (i.Participant.LastName != null && i.Participant.LastName.ToLower().Contains(query.Value)) ||
                    (i.ShortMessage != null && i.ShortMessage.ToLower().Contains(query.Value)))));

            QueryFor(x => x.Status)
                .With(request =>
                {
                    var data = new CommunicationLogStatus[]
                    {
                        CommunicationLogStatus.Success,
                        CommunicationLogStatus.Error
                    };

                    var result = data.Select(i => new SelectOption<int>(i.Value, i.Text));
                    return Task.FromResult(result);
                })
                .Use((queryable, query) => queryable.Where(i =>
                    query.Values.Contains(i.Status)));
        }

        public override async Task<QueryResult<CommunicationLogListingItem>> ExecuteQuery(CommunicationLogListingQueryForm form, CancellationToken cancellationToken)
        {
            var (keys, items, _) = await ApplyQuery(form).ExecuteAsync(i => i.Id, cancellationToken);
            return new QueryResult<CommunicationLogListingItem>(keys, items);
        }
    }
}
