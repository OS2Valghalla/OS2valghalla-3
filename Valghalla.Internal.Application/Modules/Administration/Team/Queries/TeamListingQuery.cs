using FluentValidation;
using Valghalla.Application.QueryEngine;
using Valghalla.Internal.Application.Modules.Administration.Team.Responses;

namespace Valghalla.Internal.Application.Modules.Administration.Team.Queries
{
    public sealed record TeamListingQueryFormParameters : QueryFormParameters
    {
    }

    public sealed class TeamListingQueryFormParametersValidator : AbstractValidator<TeamListingQueryFormParameters>
    {
        public TeamListingQueryFormParametersValidator()
        {
        }
    }

    public sealed record TeamListingQueryForm : QueryForm<ListTeamsItemResponse, TeamListingQueryFormParameters>
    {
        public override Order? Order { get; init; } = new Order("name", false);
    }

    public sealed class TeamListingQueryFormValidator : AbstractValidator<TeamListingQueryForm>
    {
        public TeamListingQueryFormValidator()
        {
        }
    }
}
