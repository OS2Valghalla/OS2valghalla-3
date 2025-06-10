using FluentValidation;
using Valghalla.Application.Abstractions.Messaging;
using Valghalla.Internal.Application.Modules.Administration.TaskTypeTemplate.Interfaces;

namespace Valghalla.Internal.Application.Modules.Administration.TaskTypeTemplate.Queries
{
    public sealed record GetTaskTypeTemplateDetailsQuery(Guid Id) : IQuery<Response>;

    public sealed class GetTaskTypeTemplateDetailsQueryValidator : AbstractValidator<GetTaskTypeTemplateDetailsQuery>
    {
        public GetTaskTypeTemplateDetailsQueryValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty();
        }
    }

    internal sealed class GetTaskTypeTemplateDetailsQueryHandler : IQueryHandler<GetTaskTypeTemplateDetailsQuery>
    {
        private readonly ITaskTypeTemplateQueryRepository TaskTypeTemplateQueryRepository;

        public GetTaskTypeTemplateDetailsQueryHandler(ITaskTypeTemplateQueryRepository TaskTypeTemplateQueryRepository)
        {
            this.TaskTypeTemplateQueryRepository = TaskTypeTemplateQueryRepository;
        }

        public async Task<Response> Handle(GetTaskTypeTemplateDetailsQuery query, CancellationToken cancellationToken)
        {
            var result = await TaskTypeTemplateQueryRepository.GetTaskTypeTemplateAsync(query.Id, cancellationToken);
            return Response.Ok(result);
        }
    }
}
