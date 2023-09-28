using Valghalla.Application.Abstractions.Messaging;
using Valghalla.Internal.Application.Modules.Shared.TaskType.Interfaces;

namespace Valghalla.Internal.Application.Modules.Shared.TaskType.Queries
{
    public sealed record GetTaskTypesSharedQuery() : IQuery<Response>;

    internal class GetTaskTypesSharedQueryHandler : IQueryHandler<GetTaskTypesSharedQuery>
    {
        private readonly ITaskTypeSharedQueryRepository taskTypeQueryRepository;

        public GetTaskTypesSharedQueryHandler(ITaskTypeSharedQueryRepository taskTypeQueryRepository)
        {
            this.taskTypeQueryRepository = taskTypeQueryRepository;
        }

        public async Task<Response> Handle(GetTaskTypesSharedQuery query, CancellationToken cancellationToken)
        {
            var result = await taskTypeQueryRepository.GetTaskTypesAsync(query, cancellationToken);
            return Response.Ok(result);
        }
    }
}
