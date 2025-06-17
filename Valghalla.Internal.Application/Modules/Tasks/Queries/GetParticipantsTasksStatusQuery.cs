using FluentValidation;

using Valghalla.Application.Abstractions.Messaging;
using Valghalla.Application.AuditLog;
using Valghalla.Internal.Application.Modules.Tasks.Interfaces;
using Valghalla.Internal.Application.Modules.Tasks.Requests;

namespace Valghalla.Internal.Application.Modules.Tasks.Queries
{
    public sealed record GetParticipantsTasksStatusQuery(Guid ElectionId) : IQuery<Response>;

    public sealed class GetParticipantsTasksStatusQueryValidator : AbstractValidator<GetParticipantsTasksQuery>
    {
        public GetParticipantsTasksStatusQueryValidator()
        {
            RuleFor(x => x.ElectionId)
                .NotEmpty();
        }
    }

    internal sealed class GetParticipantsTasksStatusQueryHandler : IQueryHandler<GetParticipantsTasksStatusQuery>
    {
        private readonly IAuditLogService auditLogService;
        private readonly IFilteredTasksQueryRepository filteredTasksQueryRepository;

        public GetParticipantsTasksStatusQueryHandler(IAuditLogService auditLogService, IFilteredTasksQueryRepository filteredTasksQueryRepository)
        {
            this.auditLogService = auditLogService;
            this.filteredTasksQueryRepository = filteredTasksQueryRepository;
        }

        public async Task<Response> Handle(GetParticipantsTasksStatusQuery query, CancellationToken cancellationToken)
        {
            var result = await filteredTasksQueryRepository.GetParticipantTasksStatusAsync(query, cancellationToken);
            await auditLogService.AddAuditLogAsync(new ParticipantListGenerateAuditLog(), cancellationToken);

            return Response.Ok(result);
        }
    }
}
