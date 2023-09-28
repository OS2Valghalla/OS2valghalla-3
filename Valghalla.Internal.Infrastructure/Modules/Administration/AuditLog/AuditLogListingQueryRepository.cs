using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Valghalla.Application.AuditLog;
using Valghalla.Application.QueryEngine;
using Valghalla.Database;
using Valghalla.Database.Entities.Tables;
using Valghalla.Database.QueryEngine;
using Valghalla.Internal.Application.Modules.Administration.AuditLog.Queries;
using Valghalla.Internal.Application.Modules.Administration.AuditLog.Responses;

namespace Valghalla.Internal.Infrastructure.Modules.Administration.AuditLog
{
    internal class AuditLogListingQueryRepository : QueryEngineRepository<AuditLogListingQueryForm, AuditLogListingItemResponse, AuditLogListingQueryFormParameters, AuditLogEntity>
    {
        public AuditLogListingQueryRepository(DataContext dataContext, IMapper mapper) : base(dataContext, mapper)
        {
            Order((queryable, order) =>
            {
                if (order.Name == "eventDate")
                {
                    return queryable.SortBy(i => i.EventDate, order);
                }

                return queryable;
            });

            Query(queryable => queryable
                .Include(i => i.DoneByUser));

            QueryFor(x => x.EventTable)
                .With(async request =>
                {
                    await Task.CompletedTask;
                    var data = AuditLogEventTable.GetAll();
                    return data.Select(i => new SelectOption<string>(i.Value, i.Name));
                })
                .Use((queryable, query) =>
                {
                    if (query.Values.Contains(AuditLogEventTable.Others.Value))
                    {
                        return queryable.Where(i =>
                            (i.EventTable != AuditLogEventTable.API.Value &&
                            i.EventTable != AuditLogEventTable.Participant.Value &&
                            i.EventTable != AuditLogEventTable.TeamResponsible.Value &&
                            i.EventTable != AuditLogEventTable.List.Value &&
                            i.EventTable != AuditLogEventTable.WorkLocationResponsible.Value) ||
                            query.Values.Contains(i.EventTable));
                    }

                    return queryable.Where(i => query.Values.Contains(i.EventTable));
                });

            QueryFor(x => x.EventType)
                .With(async request =>
                {
                    await Task.CompletedTask;
                    var data = AuditLogEventType.GetAll();
                    return data.Select(i => new SelectOption<string>(i.Value, i.Name));
                })
                .Use((queryable, query) =>
                {
                    return queryable.Where(i => query.Values.Contains(i.EventType));
                });
        }

        public async override Task<QueryResult<AuditLogListingItemResponse>> ExecuteQuery(AuditLogListingQueryForm form, CancellationToken cancellationToken)
        {
            var (keys, items, _) = await ApplyQuery(form).ExecuteAsync(i => i.Id, cancellationToken);
            return new QueryResult<AuditLogListingItemResponse>(keys, items);
        }
    }
}
