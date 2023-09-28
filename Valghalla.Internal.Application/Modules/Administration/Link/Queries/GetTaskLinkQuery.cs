using FluentValidation;
using Valghalla.Application.Abstractions.Messaging;
using Valghalla.Internal.Application.Modules.Administration.Link.Interfaces;

namespace Valghalla.Internal.Application.Modules.Administration.Link.Queries
{
    public sealed record GetTaskLinkQuery(string HashValue, Guid ElectionId) : IQuery<Response>;
    public sealed class GetTaskLinkQueryValidator : AbstractValidator<GetTaskLinkQuery>
    {
        public GetTaskLinkQueryValidator()
        {
            RuleFor(x => x.HashValue)
                .NotEmpty();
            RuleFor(x => x.ElectionId)
                .NotEmpty();
        }
    }
    internal sealed class GetTaskLinkQueryHandler : IQueryHandler<GetTaskLinkQuery>
    {
        private readonly ILinkQueryRepository taskLinkQueryRepository;

        public GetTaskLinkQueryHandler(ILinkQueryRepository taskLinkQueryRepository)
        {
            this.taskLinkQueryRepository = taskLinkQueryRepository;
        }

        public async Task<Response> Handle(GetTaskLinkQuery query, CancellationToken cancellationToken)
        {
            var result = await taskLinkQueryRepository.GetTaskLinkAsync(query, cancellationToken);

            if (result == null)
            {
                return Response.FailWithItemNotFoundError();
            }

            return Response.Ok(result.HashValue);
        }
    }
}
