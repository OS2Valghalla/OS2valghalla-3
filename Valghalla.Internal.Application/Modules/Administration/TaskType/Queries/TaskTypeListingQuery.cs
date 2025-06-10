using FluentValidation;

using Valghalla.Application.QueryEngine;
using Valghalla.Application.QueryEngine.Values;
using Valghalla.Internal.Application.Modules.Administration.TaskType.Responses;

namespace Valghalla.Internal.Application.Modules.Administration.TaskType.Queries
{
    public sealed record TaskTypeListingQueryFormParameters : QueryFormParameters
    {
    }

    public sealed class TaskTypeListingQueryFormParametersValidator : AbstractValidator<TaskTypeListingQueryFormParameters>
    {
        public TaskTypeListingQueryFormParametersValidator()
        {
        }
    }

    public sealed record TaskTypeListingQueryForm : QueryForm<TaskTypeListingItemResponse, TaskTypeListingQueryFormParameters>
    {
        public SingleSelectionFilterValue<Guid>? Area { get; init; }
        public SingleSelectionFilterValue<Guid>? Election { get; init; }        
    }

    public sealed class TaskTypeListingQueryFormValidator : AbstractValidator<TaskTypeListingQueryForm>
    {
        public TaskTypeListingQueryFormValidator()
        {
        }
    }
}