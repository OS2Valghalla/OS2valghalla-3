using FluentValidation;
using Valghalla.Application.Abstractions.Messaging;
using Valghalla.Internal.Application.Modules.Administration.Communication.Interfaces;

namespace Valghalla.Internal.Application.Modules.Administration.Communication.Queries
{
    public sealed record GetCommunicationTemplateQuery(Guid Id) : IQuery<Response>;

    public sealed class GetCommunicationTemplateQueryValidator : AbstractValidator<GetCommunicationTemplateQuery>
    {
        public GetCommunicationTemplateQueryValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty();
        }
    }

    internal sealed class GetCommunicationTemplateQueryHandler : IQueryHandler<GetCommunicationTemplateQuery>
    {
        private readonly ICommunicationQueryRepository communicationQueryRepository;

        public GetCommunicationTemplateQueryHandler(ICommunicationQueryRepository communicationQueryRepository)
        {
            this.communicationQueryRepository = communicationQueryRepository;
        }

        public async Task<Response> Handle(GetCommunicationTemplateQuery query, CancellationToken cancellationToken)
        {
            var result = await communicationQueryRepository.GetCommunicationTemplateAsync(query, cancellationToken);
            return Response.Ok(result);
        }
    }
}
