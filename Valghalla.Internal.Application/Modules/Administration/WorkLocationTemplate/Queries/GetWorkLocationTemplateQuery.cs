using FluentValidation;
using Valghalla.Application.Abstractions.Messaging;
using Valghalla.Internal.Application.Modules.Administration.WorkLocationTemplate.Interfaces;

namespace Valghalla.Internal.Application.Modules.Administration.WorkLocationTemplate.Queries
{
    public sealed record GetWorkLocationTemplateQuery(Guid WorkLocationTemplateId) : IQuery<Response>;
    
    public sealed class GetWorkLocationTemplateQueryValidator : AbstractValidator<GetWorkLocationTemplateQuery>
    {
        public GetWorkLocationTemplateQueryValidator()
        {
            RuleFor(x => x.WorkLocationTemplateId)
                .NotEmpty();
        }
    }

    internal sealed class GetWorkLocationTemplateQueryHandler : IQueryHandler<GetWorkLocationTemplateQuery>
    {
        private readonly IWorkLocationTemplateQueryRepository WorkLocationTemplateQueryRepository;

        public GetWorkLocationTemplateQueryHandler(IWorkLocationTemplateQueryRepository WorkLocationTemplateQueryRepository)
        {
            this.WorkLocationTemplateQueryRepository = WorkLocationTemplateQueryRepository;
        }

        public async Task<Response> Handle(GetWorkLocationTemplateQuery query, CancellationToken cancellationToken)
        {
            var result = await WorkLocationTemplateQueryRepository.GetWorkLocationTemplateAsync(query, cancellationToken);
            return Response.Ok(result);
        }
    }
}
