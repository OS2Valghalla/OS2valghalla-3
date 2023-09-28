using FluentValidation;
using Valghalla.Application.Abstractions.Messaging;
using Valghalla.Application.AuditLog;
using Valghalla.Internal.Application.Modules.Tasks.Interfaces;
using Valghalla.Internal.Application.Modules.Tasks.Requests;

namespace Valghalla.Internal.Application.Modules.Tasks.Queries
{
    public sealed record GetParticipantsTasksQuery(Guid ElectionId, ParticipantsTasksFilterRequest Filters) : IQuery<Response>;

    public sealed class GetParticipantsTasksQueryValidator : AbstractValidator<GetParticipantsTasksQuery>
    {
        public GetParticipantsTasksQueryValidator()
        {
            RuleFor(x => x.ElectionId)
                .NotEmpty();
        }
    }

    internal sealed class GetParticipantsTasksQueryHandler : IQueryHandler<GetParticipantsTasksQuery>
    {
        private readonly IAuditLogService auditLogService;
        private readonly IFilteredTasksQueryRepository filteredTasksQueryRepository;

        public GetParticipantsTasksQueryHandler(IAuditLogService auditLogService, IFilteredTasksQueryRepository filteredTasksQueryRepository)
        {
            this.auditLogService = auditLogService;
            this.filteredTasksQueryRepository = filteredTasksQueryRepository;
        }

        public async Task<Response> Handle(GetParticipantsTasksQuery query, CancellationToken cancellationToken)
        {
            var result = await filteredTasksQueryRepository.GetParticipantTasksAsync(query, cancellationToken);
            await auditLogService.AddAuditLogAsync(new ParticipantListGenerateAuditLog(), cancellationToken);

            return Response.Ok(result);
        }
    }
}
