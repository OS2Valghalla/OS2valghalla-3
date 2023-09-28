using FluentValidation;
using Valghalla.Application.QueryEngine;
using Valghalla.Internal.Application.Modules.Participant.Responses;

namespace Valghalla.Internal.Application.Modules.Participant.Queries
{
    public sealed record ParticipantEventLogListingQueryForm : QueryForm<ParticipantEventLogListingItemResponse, VoidQueryFormParameters>
    {
        public Guid ParticipantId { get; init; }
    }

    public sealed class ParticipantEventLogListingQueryFormValidator : AbstractValidator<ParticipantEventLogListingQueryForm>
    {
        public ParticipantEventLogListingQueryFormValidator()
        {
            RuleFor(x => x.ParticipantId)
                .NotEmpty();
        }
    }
}
