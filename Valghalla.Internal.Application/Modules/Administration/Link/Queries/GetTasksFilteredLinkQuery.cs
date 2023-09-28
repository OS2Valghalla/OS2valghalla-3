using FluentValidation;
using Valghalla.Application.Abstractions.Messaging;
using Valghalla.Internal.Application.Modules.Administration.Link.Interfaces;

namespace Valghalla.Internal.Application.Modules.Administration.Link.Queries
{
    public sealed record GetTasksFilteredLinkQuery(string HashValue, Guid ElectionId) : IQuery<Response>;
    public sealed class GetTasksFilteredLinkQueryValidator : AbstractValidator<GetTasksFilteredLinkQuery>
    {
        public GetTasksFilteredLinkQueryValidator()
        {
            RuleFor(x => x.HashValue)
                .NotEmpty();
            RuleFor(x => x.ElectionId)
                .NotEmpty();
        }
    }
    internal sealed class GetTasksFilteredLinkQueryHandler : IQueryHandler<GetTasksFilteredLinkQuery>
    {
        private readonly ILinkQueryRepository tasksFilteredLinkQueryRepository;

        public GetTasksFilteredLinkQueryHandler(ILinkQueryRepository tasksFilteredLinkQueryRepository)
        {
            this.tasksFilteredLinkQueryRepository = tasksFilteredLinkQueryRepository;
        }

        public async Task<Response> Handle(GetTasksFilteredLinkQuery query, CancellationToken cancellationToken)
        {
            var result = await tasksFilteredLinkQueryRepository.GetTasksFilteredLinkAsync(query, cancellationToken);

            if (result == null)
            {
                return Response.FailWithItemNotFoundError();
            }

            return Response.Ok(result.HashValue);
        }
    }
}
