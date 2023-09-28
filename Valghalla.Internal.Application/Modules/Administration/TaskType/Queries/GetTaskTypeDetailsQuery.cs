using FluentValidation;
using Valghalla.Application.Abstractions.Messaging;
using Valghalla.Internal.Application.Modules.Administration.TaskType.Interfaces;

namespace Valghalla.Internal.Application.Modules.Administration.TaskType.Queries
{
    public sealed record GetTaskTypeDetailsQuery(Guid Id) : IQuery<Response>;

    public sealed class GetTaskTypeDetailsQueryValidator : AbstractValidator<GetTaskTypeDetailsQuery>
    {
        public GetTaskTypeDetailsQueryValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty();
        }
    }

    internal sealed class GetTaskTypeDetailsQueryHandler : IQueryHandler<GetTaskTypeDetailsQuery>
    {
        private readonly ITaskTypeQueryRepository taskTypeQueryRepository;

        public GetTaskTypeDetailsQueryHandler(ITaskTypeQueryRepository taskTypeQueryRepository)
        {
            this.taskTypeQueryRepository = taskTypeQueryRepository;
        }

        public async Task<Response> Handle(GetTaskTypeDetailsQuery query, CancellationToken cancellationToken)
        {
            var result = await taskTypeQueryRepository.GetTaskTypeAsync(query.Id, cancellationToken);
            return Response.Ok(result);
        }
    }
}
