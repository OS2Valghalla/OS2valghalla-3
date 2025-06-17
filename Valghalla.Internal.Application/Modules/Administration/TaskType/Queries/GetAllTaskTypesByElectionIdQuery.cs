using FluentValidation;

using Valghalla.Application.Abstractions.Messaging;
using Valghalla.Internal.Application.Modules.Administration.TaskType.Interfaces;

namespace Valghalla.Internal.Application.Modules.Administration.TaskType.Queries
{
    public sealed record GetAllTaskTypesByElectionIdQuery(Guid electionId) : IQuery<Response>;

    public sealed class GetAllTaskTypesElectionIdQueryValidator : AbstractValidator<GetAllTaskTypesQuery>
    {
        public GetAllTaskTypesElectionIdQueryValidator()
        {
        }
    }

    internal sealed class GetAllTaskTypesElectionIdQueryHandler : IQueryHandler<GetAllTaskTypesByElectionIdQuery>
    {
        private readonly ITaskTypeQueryRepository taskTypeQueryRepository;

        public GetAllTaskTypesElectionIdQueryHandler(ITaskTypeQueryRepository taskTypeQueryRepository)
        {
            this.taskTypeQueryRepository = taskTypeQueryRepository;
        }

        public async Task<Response> Handle(GetAllTaskTypesByElectionIdQuery query, CancellationToken cancellationToken)
        {
            var result = await taskTypeQueryRepository.GetAllTaskTypesByElectionIdAsync(query.electionId, cancellationToken);
            return Response.Ok(result);
        }
    }
}
