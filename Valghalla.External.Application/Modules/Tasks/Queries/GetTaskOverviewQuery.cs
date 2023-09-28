using FluentValidation;
using Valghalla.Application.Abstractions.Messaging;
using Valghalla.Application.User;
using Valghalla.External.Application.Modules.Tasks.Interfaces;

namespace Valghalla.External.Application.Modules.Tasks.Queries
{
    public sealed record GetTaskOverviewQuery : IQuery<Response>
    {
        public DateTime? TaskDate { get; init; }
        public Guid? TaskTypeId { get; init; }
        public Guid? WorkLocationId { get; init; }
        public Guid? TeamId { get; init; }
    }

    internal class GetTaskOverviewQueryHandler : IQueryHandler<GetTaskOverviewQuery>
    {
        private readonly IUserContextProvider userContextProvider;
        private readonly ITaskQueryRepository taskQueryRepository;

        public GetTaskOverviewQueryHandler(IUserContextProvider userContextProvider, ITaskQueryRepository taskQueryRepository)
        {
            this.userContextProvider = userContextProvider;
            this.taskQueryRepository = taskQueryRepository;
        }

        public async Task<Response> Handle(GetTaskOverviewQuery query, CancellationToken cancellationToken)
        {
            var participantId = userContextProvider.CurrentUser.ParticipantId!.Value;
            var result = await taskQueryRepository.GetTaskOverviewAsync(participantId, query, cancellationToken);
            return Response.Ok(result);
        }
    }
}
