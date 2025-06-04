using Valghalla.Application.Abstractions.Messaging;
using Valghalla.Internal.Application.Modules.Shared.WorkLocationTemplate.Interfaces;

namespace Valghalla.Internal.Application.Modules.Shared.WorkLocationTemplate.Queries
{
    public sealed record GetWorkLocationTemplatesSharedQuery() : IQuery<Response>;

    internal class GetWorkLocationTemplatesSharedQueryHandler : IQueryHandler<GetWorkLocationTemplatesSharedQuery>
    {
        private readonly IWorkLocationTemplateSharedQueryRepository WorkLocationTemplateSharedQueryRepository;

        public GetWorkLocationTemplatesSharedQueryHandler(IWorkLocationTemplateSharedQueryRepository WorkLocationTemplateSharedQueryRepository)
        {
            this.WorkLocationTemplateSharedQueryRepository = WorkLocationTemplateSharedQueryRepository;
        }

        public async Task<Response> Handle(GetWorkLocationTemplatesSharedQuery query, CancellationToken cancellationToken)
        {
            var result = await WorkLocationTemplateSharedQueryRepository.GetWorkLocationTemplatesAsync(query, cancellationToken);
            return Response.Ok(result);
        }
    }
}
