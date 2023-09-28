using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Valghalla.Application;
using Valghalla.Application.QueryEngine;
using Valghalla.Database;
using Valghalla.Database.Entities.Tables;
using Valghalla.Database.QueryEngine;
using Valghalla.Internal.Application.Modules.Administration.Communication.Queries;
using Valghalla.Internal.Application.Modules.Administration.Communication.Responses;

namespace Valghalla.Internal.Infrastructure.Modules.Administration.Communication
{

    internal class CommunicationTemplateListingQueryRepository : QueryEngineRepository<CommunicationTemplateListingQueryForm, CommunicationTemplateListingItemResponse, CommunicationTemplateListingQueryFormParameters, CommunicationTemplateEntity>
    {
        public CommunicationTemplateListingQueryRepository(DataContext dataContext, IMapper mapper) : base(dataContext, mapper)
        {
            var excludedIds = new List<Guid> {
                new Guid(Constants.DefaultCommunicationTemplates.InvitationReminderStringId),
                new Guid(Constants.DefaultCommunicationTemplates.TaskReminderStringId),
                new Guid(Constants.DefaultCommunicationTemplates.RetractedInvitationStringId)
            };

            Order((queryable, order) =>
            {
                if (order.Name == "title")
                {
                    return queryable.SortBy(i => i.Title, order);
                }
                else if (order.Name == "subject")
                {
                    return queryable.SortBy(i => i.Subject, order);
                }
                else if (order.Name == "templateTypeName")
                {
                    return queryable.SortBy(i => i.TemplateType, order);
                }

                return queryable;
            });

            Query(queryable => queryable
              .Where(x => !excludedIds.Contains(x.Id)));

            QueryFor(x => x.Title)
               .Use((queryable, query) => queryable.Where(i => i.Title.Contains(query.Value)));

            QueryFor(x => x.TemplateType)
                .With(_ =>
                {
                    var data = CommunicationTemplateListingQueryForm.TemplateTypes.GetAll();
                    return Task.FromResult(data.Select(i => new SelectOption<int>(i.Id, i.Label)));
                })
               .Use((queryable, query) => queryable.Where(i => i.TemplateType == query.Value));

            QueryFor(x => x.Search)
                .Use((queryable, query) => queryable
                    .Where(i =>
                        i.Title.ToLower().Contains(query.Value)));
        }

        public override async Task<QueryResult<CommunicationTemplateListingItemResponse>> ExecuteQuery(CommunicationTemplateListingQueryForm form, CancellationToken cancellationToken)
        {
            var (keys, items, _) = await ApplyQuery(form).ExecuteAsync(i => i.Id, cancellationToken);
            return new QueryResult<CommunicationTemplateListingItemResponse>(keys, items);
        }
    }
}
