using FluentValidation;

using Valghalla.Application.Abstractions.Messaging;
using Valghalla.Application.AuditLog;
using Valghalla.Internal.Application.Modules.Tasks.Interfaces;
using Valghalla.Internal.Application.Modules.Tasks.Requests;

namespace Valghalla.Internal.Application.Modules.Tasks.Queries
{
    public sealed record GetRejectedTasksQuery(Guid ElectionId) : IQuery<Response>;

    public sealed class GetRejectedTasksQueryValidator : AbstractValidator<GetParticipantsTasksQuery>
    {
        public GetRejectedTasksQueryValidator()
        {
            RuleFor(x => x.ElectionId)
                .NotEmpty();
        }
    }

    internal sealed class GetRejectedTasksQueryHandler : IQueryHandler<GetRejectedTasksQuery>
    {
        private readonly IAuditLogService auditLogService;
        private readonly IFilteredTasksQueryRepository filteredTasksQueryRepository;

        public GetRejectedTasksQueryHandler(IAuditLogService auditLogService, IFilteredTasksQueryRepository filteredTasksQueryRepository)
        {
            this.auditLogService = auditLogService;
            this.filteredTasksQueryRepository = filteredTasksQueryRepository;
        }

        public async Task<Response> Handle(GetRejectedTasksQuery query, CancellationToken cancellationToken)
        {
            var result = await filteredTasksQueryRepository.GetRejectedTasks(query, cancellationToken);
            
            return Response.Ok(result);
        }
    }
}
