using AutoMapper;
using Valghalla.Application.QueryEngine;
using Valghalla.Database;
using Valghalla.Database.Entities.Tables;
using Valghalla.Database.QueryEngine;
using Valghalla.Internal.Application.Modules.Participant.Queries;
using Valghalla.Internal.Application.Modules.Participant.Responses;

namespace Valghalla.Internal.Infrastructure.Modules.Participant
{
    internal class ParticipantEventLogListingQueryRepository : QueryEngineRepository<ParticipantEventLogListingQueryForm, ParticipantEventLogListingItemResponse, VoidQueryFormParameters, ParticipantEventLogEntity>
    {
        public ParticipantEventLogListingQueryRepository(DataContext dataContext, IMapper mapper) : base(dataContext, mapper)
        {
            Order((queryable, order) =>
            {
                if (order.Name == "createdAt")
                {
                    return queryable.SortBy(i => i.CreatedAt, order);
                }

                return queryable;
            });

            QueryFor(x => x.ParticipantId)
                .Use((queryable, participantId) => queryable.Where(i => i.ParticipantId == participantId));
        }

        public override async Task<QueryResult<ParticipantEventLogListingItemResponse>> ExecuteQuery(ParticipantEventLogListingQueryForm form, CancellationToken cancellationToken)
        {
            var (keys, items, _) = await ApplyQuery(form).ExecuteAsync(i => i.Id, cancellationToken);
            return new QueryResult<ParticipantEventLogListingItemResponse>(keys, items);
        }
    }
}
