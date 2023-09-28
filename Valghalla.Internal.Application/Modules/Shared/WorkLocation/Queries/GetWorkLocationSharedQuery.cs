using FluentValidation;
using Valghalla.Application.Abstractions.Messaging;
using Valghalla.Internal.Application.Modules.Shared.WorkLocation.Interfaces;

namespace Valghalla.Internal.Application.Modules.Shared.WorkLocation.Queries
{
    public sealed record GetWorkLocationSharedQuery(Guid WorkLocationId, Guid? ElectionId) : IQuery<Response>;

    public sealed class GetWorkLocationSharedQueryValidator : AbstractValidator<GetWorkLocationSharedQuery>
    {
        public GetWorkLocationSharedQueryValidator()
        {
            RuleFor(x => x.WorkLocationId)
                .NotEmpty();
        }
    }

    internal sealed class GetWorkLocationSharedQueryHandler : IQueryHandler<GetWorkLocationSharedQuery>
    {
        private readonly IWorkLocationSharedQueryRepository workLocationSharedQueryRepository;

        public GetWorkLocationSharedQueryHandler(IWorkLocationSharedQueryRepository workLocationSharedQueryRepository)
        {
            this.workLocationSharedQueryRepository = workLocationSharedQueryRepository;
        }

        public async Task<Response> Handle(GetWorkLocationSharedQuery query, CancellationToken cancellationToken)
        {
            var result = await workLocationSharedQueryRepository.GetWorkLocationAsync(query, cancellationToken);
            return Response.Ok(result);
        }
    }
}
