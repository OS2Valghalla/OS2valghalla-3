using FluentValidation;
using Valghalla.Application.Abstractions.Messaging;
using Valghalla.Internal.Application.Modules.Administration.WorkLocation.Interfaces;

namespace Valghalla.Internal.Application.Modules.Administration.WorkLocation.Queries
{
    public sealed record GetWorkLocationQuery(Guid WorkLocationId) : IQuery<Response>;
    
    public sealed class GetWorkLocationQueryValidator : AbstractValidator<GetWorkLocationQuery>
    {
        public GetWorkLocationQueryValidator()
        {
            RuleFor(x => x.WorkLocationId)
                .NotEmpty();
        }
    }

    internal sealed class GetWorkLocationQueryHandler : IQueryHandler<GetWorkLocationQuery>
    {
        private readonly IWorkLocationQueryRepository workLocationQueryRepository;

        public GetWorkLocationQueryHandler(IWorkLocationQueryRepository workLocationQueryRepository)
        {
            this.workLocationQueryRepository = workLocationQueryRepository;
        }

        public async Task<Response> Handle(GetWorkLocationQuery query, CancellationToken cancellationToken)
        {
            var result = await workLocationQueryRepository.GetWorkLocationAsync(query, cancellationToken);
            return Response.Ok(result);
        }
    }
}
