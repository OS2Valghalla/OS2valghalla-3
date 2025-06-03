using FluentValidation;
using Valghalla.Application.Abstractions.Messaging;
using Valghalla.Internal.Application.Modules.Administration.WorkLocationTemplate.Interfaces;

namespace Valghalla.Internal.Application.Modules.Administration.WorkLocationTemplate.Queries
{
    public sealed record GetWorkLocationTemplateResponsibleParticipantsQuery(Guid WorkLocationTemplateId) : IQuery<Response>;

    public sealed class GetWorkLocationTemplateResponsibleParticipantsQueryValidator : AbstractValidator<GetWorkLocationTemplateResponsibleParticipantsQuery>
    {
        public GetWorkLocationTemplateResponsibleParticipantsQueryValidator()
        {
            RuleFor(x => x.WorkLocationTemplateId)
                .NotEmpty();
        }
    }

    internal sealed class GetWorkLocationTemplateResponsibleParticipantsQueryHandler : IQueryHandler<GetWorkLocationTemplateResponsibleParticipantsQuery>
    {
        private readonly IWorkLocationTemplateQueryRepository WorkLocationTemplateQueryRepository;

        public GetWorkLocationTemplateResponsibleParticipantsQueryHandler(IWorkLocationTemplateQueryRepository WorkLocationTemplateQueryRepository)
        {
            this.WorkLocationTemplateQueryRepository = WorkLocationTemplateQueryRepository;
        }

        public async Task<Response> Handle(GetWorkLocationTemplateResponsibleParticipantsQuery query, CancellationToken cancellationToken)
        {
            //var result = await WorkLocationTemplateQueryRepository.GetWorkLocationTemplateResponsiblesAsync(query, cancellationToken);
            //return Response.Ok(result);
            return Response.Ok(); // Placeholder for actual implementation
        }
    }
}
