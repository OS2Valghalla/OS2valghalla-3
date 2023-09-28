using FluentValidation;
using Valghalla.Application.Abstractions.Messaging;
using Valghalla.Application.User;
using Valghalla.External.Application.Modules.WorkLocation.Interfaces;


namespace Valghalla.External.Application.Modules.WorkLocation.Queries
{
    public sealed record GetMyWorkLocationsQuery() : IQuery<Response>;

    public sealed class GetMyWorkLocationsQueryValidator : AbstractValidator<GetMyWorkLocationsQuery>
    {
        public GetMyWorkLocationsQueryValidator()
        {
        }
    }

    internal class GetMyWorkLocationsQueryHandler : IQueryHandler<GetMyWorkLocationsQuery>
    {
        private readonly IUserContextProvider userContextProvider;
        private readonly IWorkLocationQueryRepository workLocationQueryRepository;

        public GetMyWorkLocationsQueryHandler(IUserContextProvider userContextProvider, IWorkLocationQueryRepository workLocationQueryRepository)
        {
            this.userContextProvider = userContextProvider;
            this.workLocationQueryRepository = workLocationQueryRepository;
        }

        public async Task<Response> Handle(GetMyWorkLocationsQuery query, CancellationToken cancellationToken)
        {
            var participantId = userContextProvider.CurrentUser.ParticipantId!.Value;
            var result = await workLocationQueryRepository.GetMyWorkLocationsAsync(participantId, cancellationToken);

            return Response.Ok(result);
        }
    }
}
