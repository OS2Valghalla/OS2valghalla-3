using Valghalla.Application.Abstractions.Messaging;
using Valghalla.Internal.Application.Modules.Shared.TaskTypeTemplate.Interfaces;

namespace Valghalla.Internal.Application.Modules.Shared.TaskTypeTemplate.Queries
{
    public sealed record GetTaskTypeTemplatesSharedQuery() : IQuery<Response>;

    internal class GetTaskTypeTemplatesSharedQueryHandler : IQueryHandler<GetTaskTypeTemplatesSharedQuery>
    {
        private readonly ITaskTypeTemplateSharedQueryRepository TaskTypeTemplateQueryRepository;

        public GetTaskTypeTemplatesSharedQueryHandler(ITaskTypeTemplateSharedQueryRepository TaskTypeTemplateQueryRepository)
        {
            this.TaskTypeTemplateQueryRepository = TaskTypeTemplateQueryRepository;
        }

        public async Task<Response> Handle(GetTaskTypeTemplatesSharedQuery query, CancellationToken cancellationToken)
        {
            var result = await TaskTypeTemplateQueryRepository.GetTaskTypeTemplatesAsync(query, cancellationToken);
            return Response.Ok(result);
        }
    }
}
