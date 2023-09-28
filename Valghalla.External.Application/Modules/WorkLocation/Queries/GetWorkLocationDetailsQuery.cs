using FluentValidation;
using Valghalla.Application.Abstractions.Messaging;
using Valghalla.Application.User;
using Valghalla.External.Application.Modules.WorkLocation.Interfaces;

namespace Valghalla.External.Application.Modules.WorkLocation.Queries
{
    public sealed record GetWorkLocationDetailsQuery(Guid WorkLocationId, DateTime TaskDate) : IQuery<Response>;

    public sealed class GetWorkLocationDetailsQueryValidator : AbstractValidator<GetWorkLocationDetailsQuery>
    {
        public GetWorkLocationDetailsQueryValidator()
        {
            RuleFor(x => x.WorkLocationId)
              .NotEmpty();

            RuleFor(x => x.TaskDate)
             .NotEmpty();
        }
    }

    internal class GetWorkLocationDetailsQueryHandler : IQueryHandler<GetWorkLocationDetailsQuery>
    {
        private readonly IUserContextProvider userContextProvider;
        private readonly IWorkLocationQueryRepository workLocationQueryRepository;

        public GetWorkLocationDetailsQueryHandler(IUserContextProvider userContextProvider, IWorkLocationQueryRepository workLocationQueryRepository)
        {
            this.userContextProvider = userContextProvider;
            this.workLocationQueryRepository = workLocationQueryRepository;
        }

        public async Task<Response> Handle(GetWorkLocationDetailsQuery query, CancellationToken cancellationToken)
        {
            var participantId = userContextProvider.CurrentUser.ParticipantId!.Value;
            var result = await workLocationQueryRepository.GetWorkLocationDetailsAsync(query.WorkLocationId, query.TaskDate, participantId, cancellationToken);

            return Response.Ok(result);
        }
    }
}
