using Valghalla.Application.QueryEngine;
using Valghalla.Internal.Application.Modules.Administration.TaskTypeTemplate.Responses;

namespace Valghalla.Internal.Application.Modules.Administration.TaskTypeTemplate.Queries
{
    public sealed record TaskTypeTemplateListingQueryForm() : QueryForm<TaskTypeTemplateListingItemResponse, VoidQueryFormParameters>;
}
