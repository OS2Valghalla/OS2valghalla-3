using FluentValidation;
using Valghalla.Application.Abstractions.Messaging;
using Valghalla.Application.User;
using Valghalla.External.Application.Modules.WorkLocation.Interfaces;

namespace Valghalla.External.Application.Modules.WorkLocation.Queries
{
    public sealed record GetWorkLocationDatesQuery(Guid WorkLocationId) : IQuery<Response>;

    public sealed class GetWorkLocationDatesQueryValidator : AbstractValidator<GetWorkLocationDatesQuery>
    {
        public GetWorkLocationDatesQueryValidator()
        {
            RuleFor(x => x.WorkLocationId)
              .NotEmpty();
        }
    }

    internal class GetWorkLocationDatesQueryHandler : IQueryHandler<GetWorkLocationDatesQuery>
    {
        private readonly IUserContextProvider userContextProvider;
        private readonly IWorkLocationQueryRepository workLocationQueryRepository;

        public GetWorkLocationDatesQueryHandler(IUserContextProvider userContextProvider, IWorkLocationQueryRepository workLocationQueryRepository)
        {
            this.userContextProvider = userContextProvider;
            this.workLocationQueryRepository = workLocationQueryRepository;
        }

        public async Task<Response> Handle(GetWorkLocationDatesQuery query, CancellationToken cancellationToken)
        {
            var participantId = userContextProvider.CurrentUser.ParticipantId!.Value;
            var result = await workLocationQueryRepository.GetWorkLocationDatesAsync(query.WorkLocationId, participantId, cancellationToken);

            return Response.Ok(result);
        }
    }
}
