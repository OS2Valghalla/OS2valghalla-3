using FluentValidation;
using Valghalla.Application.Abstractions.Messaging;
using Valghalla.Internal.Application.Modules.Analyze.Interfaces;
using Valghalla.Internal.Application.Modules.Analyze.Requests;

namespace Valghalla.Internal.Application.Modules.Analyze.Commands
{
    public sealed record SaveAnalyzeQueryCommand(SaveAnalyzeQueryRequest Request) : ICommand<Response>;

    public sealed class SaveAnalyzeQueryCommandValidator : AbstractValidator<SaveAnalyzeQueryCommand>
    {
        public SaveAnalyzeQueryCommandValidator(IAnalyzeQueryRepository analyzeQueryRepository)
        {
            RuleFor(x => x.Request.Name)
                .NotEmpty();

            RuleFor(x => x.Request.Name)
                .Must((name) => !analyzeQueryRepository.IsNameUsed(name, default).Result)
                .WithMessage("analyze.validation.name_already_used");

            RuleFor(x => x.Request.QueryId)
                .NotEmpty()
                .When(x => x.Request.CreateNewQueryRequest == null)
                .GreaterThan(0)
                .When(x => x.Request.CreateNewQueryRequest == null);

            RuleFor(x => x.Request.CreateNewQueryRequest)
                .NotEmpty()
                .When(x => !x.Request.QueryId.HasValue);

            RuleFor(x => x.Request.CreateNewQueryRequest.ElectionId)
                .NotEmpty()
                .When(x => x.Request.CreateNewQueryRequest != null);

            RuleFor(x => x.Request.CreateNewQueryRequest.ListTypeId)
                .GreaterThan(0)
                .When(x => x.Request.CreateNewQueryRequest != null);

            RuleFor(x => x.Request.CreateNewQueryRequest.ColumnIds)
                .NotEmpty()
                .When(x => x.Request.CreateNewQueryRequest != null);
        }
    }

    internal sealed class SaveAnalyzeQueryCommandHandler : ICommandHandler<SaveAnalyzeQueryCommand, Response>
    {
        private readonly IAnalyzeCommandRepository analyzeCommandRepository;

        public SaveAnalyzeQueryCommandHandler(IAnalyzeCommandRepository analyzeCommandRepository)
        {
            this.analyzeCommandRepository = analyzeCommandRepository;
        }

        public async Task<Response> Handle(SaveAnalyzeQueryCommand command, CancellationToken cancellationToken)
        {
            var queryId = await analyzeCommandRepository.SaveQuery(command.Request, cancellationToken);

            if (queryId == -1) return Response.FailWithItemNotFoundError();

            return Response.Ok(queryId);
        }
    }
}
