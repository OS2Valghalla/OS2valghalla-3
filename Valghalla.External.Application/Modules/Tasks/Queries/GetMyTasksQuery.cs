using FluentValidation;
using Valghalla.Application.Abstractions.Messaging;
using Valghalla.Application.User;
using Valghalla.External.Application.Modules.Tasks.Interfaces;

namespace Valghalla.External.Application.Modules.Tasks.Queries
{
    public sealed record GetMyTasksQuery() : IQuery<Response>;

    public sealed class GetMyTasksQueryValidator : AbstractValidator<GetMyTasksQuery>
    {
        public GetMyTasksQueryValidator()
        {
        }
    }

    internal class GetMyTasksQueryHandler : IQueryHandler<GetMyTasksQuery>
    {
        private readonly IUserContextProvider userContextProvider;
        private readonly ITaskQueryRepository taskQueryRepository;

        public GetMyTasksQueryHandler(IUserContextProvider userContextProvider, ITaskQueryRepository taskQueryRepository)
        {
            this.userContextProvider = userContextProvider;
            this.taskQueryRepository = taskQueryRepository;
        }

        public async Task<Response> Handle(GetMyTasksQuery query, CancellationToken cancellationToken)
        {
            var participantId = userContextProvider.CurrentUser.ParticipantId!.Value;

            var result = await taskQueryRepository.GetMyTasksAsync(participantId, cancellationToken);

            return Response.Ok(result);
        }
    }
}
