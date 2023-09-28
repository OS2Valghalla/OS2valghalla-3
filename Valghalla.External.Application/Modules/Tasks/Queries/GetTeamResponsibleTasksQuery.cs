using FluentValidation;
using Valghalla.Application.Abstractions.Messaging;
using Valghalla.Application.User;
using Valghalla.External.Application.Modules.Tasks.Interfaces;

namespace Valghalla.External.Application.Modules.Tasks.Queries
{
    public sealed record GetTeamResponsibleTasksQuery(Guid TeamId, Guid? WorkLocationId, Guid? TaskTypeId, DateTime? TaskDate) : IQuery<Response>;

    public sealed class GetTeamResponsibleTasksQueryValidator : AbstractValidator<GetTeamResponsibleTasksQuery>
    {
        public GetTeamResponsibleTasksQueryValidator()
        {
            RuleFor(x => x.TeamId)
                .NotEmpty();
        }
    }

    internal class GetTeamResponsibleTasksQueryHandler : IQueryHandler<GetTeamResponsibleTasksQuery>
    {
        private readonly IUserContextProvider userContextProvider;
        private readonly ITaskQueryRepository taskQueryRepository;

        public GetTeamResponsibleTasksQueryHandler(IUserContextProvider userContextProvider, ITaskQueryRepository taskQueryRepository)
        {
            this.userContextProvider = userContextProvider;
            this.taskQueryRepository = taskQueryRepository;
        }

        public async Task<Response> Handle(GetTeamResponsibleTasksQuery query, CancellationToken cancellationToken)
        {
            var participantId = userContextProvider.CurrentUser.ParticipantId!.Value;

            var result = await taskQueryRepository.GetTeamResponsibleTasksAsync(participantId, query, cancellationToken);

            return Response.Ok(result);
        }
    }
}
