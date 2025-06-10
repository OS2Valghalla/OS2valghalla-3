using FluentValidation;
using Valghalla.Application.Abstractions.Messaging;
using Valghalla.Internal.Application.Modules.Administration.TaskTypeTemplate.Interfaces;

namespace Valghalla.Internal.Application.Modules.Administration.TaskTypeTemplate.Queries
{
    public sealed record GetAllTaskTypeTemplatesQuery() : IQuery<Response>;

    public sealed class GetAllTaskTypeTemplatesQueryValidator : AbstractValidator<GetAllTaskTypeTemplatesQuery>
    {
        public GetAllTaskTypeTemplatesQueryValidator()
        {
        }
    }

    internal sealed class GetAllTaskTypeTemplatesQueryHandler : IQueryHandler<GetAllTaskTypeTemplatesQuery>
    {
        private readonly ITaskTypeTemplateQueryRepository TaskTypeTemplateQueryRepository;

        public GetAllTaskTypeTemplatesQueryHandler(ITaskTypeTemplateQueryRepository TaskTypeTemplateQueryRepository)
        {
            this.TaskTypeTemplateQueryRepository = TaskTypeTemplateQueryRepository;
        }

        public async Task<Response> Handle(GetAllTaskTypeTemplatesQuery query, CancellationToken cancellationToken)
        {
            var result = await TaskTypeTemplateQueryRepository.GetAllTaskTypeTemplatesAsync(cancellationToken);
            return Response.Ok(result);
        }
    }
}
