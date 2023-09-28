using Valghalla.Application.Abstractions.Messaging;
using Valghalla.Application.User;
using Valghalla.External.Application.Modules.Tasks.Interfaces;

namespace Valghalla.External.Application.Modules.Tasks.Queries
{
    public sealed record GetTaskOverviewFilterOptionsQuery() : IQuery<Response>;

    internal class GetTaskOverviewFilterOptionsQueryHandler : IQueryHandler<GetTaskOverviewFilterOptionsQuery>
    {
        private readonly IUserContextProvider userContextProvider;
        private readonly ITaskQueryRepository taskQueryRepository;

        public GetTaskOverviewFilterOptionsQueryHandler(IUserContextProvider userContextProvider, ITaskQueryRepository taskQueryRepository)
        {
            this.userContextProvider = userContextProvider;
            this.taskQueryRepository = taskQueryRepository;
        }

        public async Task<Response> Handle(GetTaskOverviewFilterOptionsQuery query, CancellationToken cancellationToken)
        {
            var participantId = userContextProvider.CurrentUser.ParticipantId!.Value;
            var result = await taskQueryRepository.GetTaskOverviewFilterOptionsAsync(participantId, query, cancellationToken);
            return Response.Ok(result);
        }
    }
}
