using FluentValidation;
using Valghalla.Application.Abstractions.Messaging;
using Valghalla.Application.User;
using Valghalla.External.Application.Modules.Tasks.Interfaces;

namespace Valghalla.External.Application.Modules.Tasks.Queries
{
    public sealed record GetTeamResponsibleTasksFiltersOptionsQuery() : IQuery<Response>;

    public sealed class GetTeamResponsibleTasksFiltersOptionsQueryValidator : AbstractValidator<GetTeamResponsibleTasksFiltersOptionsQuery>
    {
        public GetTeamResponsibleTasksFiltersOptionsQueryValidator()
        {
        }
    }

    internal class GetTeamResponsibleTasksFiltersOptionsQueryHandler : IQueryHandler<GetTeamResponsibleTasksFiltersOptionsQuery>
    {
        private readonly IUserContextProvider userContextProvider;
        private readonly ITaskQueryRepository taskQueryRepository;

        public GetTeamResponsibleTasksFiltersOptionsQueryHandler(IUserContextProvider userContextProvider, ITaskQueryRepository taskQueryRepository)
        {
            this.userContextProvider = userContextProvider;
            this.taskQueryRepository = taskQueryRepository;
        }

        public async Task<Response> Handle(GetTeamResponsibleTasksFiltersOptionsQuery query, CancellationToken cancellationToken)
        {
            var participantId = userContextProvider.CurrentUser.ParticipantId!.Value;

            var result = await taskQueryRepository.GetTeamResponsibleTasksFiltersOptionsAsync(participantId, cancellationToken);

            return Response.Ok(result);
        }
    }
}
