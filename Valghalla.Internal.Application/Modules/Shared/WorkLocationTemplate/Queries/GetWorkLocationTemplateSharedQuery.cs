using FluentValidation;
using Valghalla.Application.Abstractions.Messaging;
using Valghalla.Internal.Application.Modules.Shared.WorkLocationTemplate.Interfaces;

namespace Valghalla.Internal.Application.Modules.Shared.WorkLocationTemplate.Queries
{
    public sealed record GetWorkLocationTemplateSharedQuery(Guid WorkLocationId, Guid? ElectionId) : IQuery<Response>;

    public sealed class GetWorkLocationTemplateSharedQueryValidator : AbstractValidator<GetWorkLocationTemplateSharedQuery>
    {
        public GetWorkLocationTemplateSharedQueryValidator()
        {
            RuleFor(x => x.WorkLocationId)
                .NotEmpty();
        }
    }

    internal sealed class GetWorkLocationTemplateSharedQueryHandler : IQueryHandler<GetWorkLocationTemplateSharedQuery>
    {
        private readonly IWorkLocationTemplateSharedQueryRepository WorkLocationTemplateSharedQueryRepository;

        public GetWorkLocationTemplateSharedQueryHandler(IWorkLocationTemplateSharedQueryRepository WorkLocationTemplateSharedQueryRepository)
        {
            this.WorkLocationTemplateSharedQueryRepository = WorkLocationTemplateSharedQueryRepository;
        }

        public async Task<Response> Handle(GetWorkLocationTemplateSharedQuery query, CancellationToken cancellationToken)
        {
            var result = await WorkLocationTemplateSharedQueryRepository.GetWorkLocationTemplateAsync(query, cancellationToken);
            return Response.Ok(result);
        }
    }
}
