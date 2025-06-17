using FluentValidation;
using Valghalla.Application.Abstractions.Messaging;
using Valghalla.Internal.Application.Modules.Administration.WorkLocationTemplate.Interfaces;

namespace Valghalla.Internal.Application.Modules.Administration.WorkLocationTemplate.Queries
{
    public sealed record GetWorkLocationTemplatesQuery() : IQuery<Response>;
    
    public sealed class GetWorkLocationTemplatesQueryValidator : AbstractValidator<GetWorkLocationTemplatesQuery>
    {
        public GetWorkLocationTemplatesQueryValidator()
        {            
        }
    }

    internal sealed class GetWorkLocationTemplatesQueryHandler : IQueryHandler<GetWorkLocationTemplatesQuery>
    {
        private readonly IWorkLocationTemplateQueryRepository WorkLocationTemplateQueryRepository;

        public GetWorkLocationTemplatesQueryHandler(IWorkLocationTemplateQueryRepository WorkLocationTemplateQueryRepository)
        {
            this.WorkLocationTemplateQueryRepository = WorkLocationTemplateQueryRepository;
        }

        public async Task<Response> Handle(GetWorkLocationTemplatesQuery query, CancellationToken cancellationToken)
        {
            var result = await WorkLocationTemplateQueryRepository.GetWorkLocationTemplatesAsync(query, cancellationToken);
            return Response.Ok(result);
        }
    }
}
