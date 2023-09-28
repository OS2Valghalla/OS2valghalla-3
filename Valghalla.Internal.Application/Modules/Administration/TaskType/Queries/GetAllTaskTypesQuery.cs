using FluentValidation;
using Valghalla.Application.Abstractions.Messaging;
using Valghalla.Internal.Application.Modules.Administration.TaskType.Interfaces;

namespace Valghalla.Internal.Application.Modules.Administration.TaskType.Queries
{
    public sealed record GetAllTaskTypesQuery() : IQuery<Response>;

    public sealed class GetAllTaskTypesQueryValidator : AbstractValidator<GetAllTaskTypesQuery>
    {
        public GetAllTaskTypesQueryValidator()
        {
        }
    }

    internal sealed class GetAllTaskTypesQueryHandler : IQueryHandler<GetAllTaskTypesQuery>
    {
        private readonly ITaskTypeQueryRepository taskTypeQueryRepository;

        public GetAllTaskTypesQueryHandler(ITaskTypeQueryRepository taskTypeQueryRepository)
        {
            this.taskTypeQueryRepository = taskTypeQueryRepository;
        }

        public async Task<Response> Handle(GetAllTaskTypesQuery query, CancellationToken cancellationToken)
        {
            var result = await taskTypeQueryRepository.GetAllTaskTypesAsync(cancellationToken);
            return Response.Ok(result);
        }
    }
}
