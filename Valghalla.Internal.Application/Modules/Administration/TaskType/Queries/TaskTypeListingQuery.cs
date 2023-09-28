using Valghalla.Application.QueryEngine;
using Valghalla.Internal.Application.Modules.Administration.TaskType.Responses;

namespace Valghalla.Internal.Application.Modules.Administration.TaskType.Queries
{
    public sealed record TaskTypeListingQueryForm() : QueryForm<TaskTypeListingItemResponse, VoidQueryFormParameters>;
}
