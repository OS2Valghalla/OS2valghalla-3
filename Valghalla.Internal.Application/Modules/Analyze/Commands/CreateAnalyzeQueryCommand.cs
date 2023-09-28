using FluentValidation;
using Valghalla.Application.Abstractions.Messaging;
using Valghalla.Internal.Application.Modules.Analyze.Interfaces;
using Valghalla.Internal.Application.Modules.Analyze.Requests;

namespace Valghalla.Internal.Application.Modules.Analyze.Commands
{
    public sealed record CreateAnalyzeQueryCommand(CreateQueryRequest Request) : ICommand<Response<int>>;

    public sealed class CreateAnalyzeQueryCommandValidator : AbstractValidator<CreateAnalyzeQueryCommand>
    {
        public CreateAnalyzeQueryCommandValidator()
        {
            RuleFor(x => x.Request.ElectionId)
                .NotEmpty();

            RuleFor(x => x.Request.ListTypeId)
                .GreaterThan(0);

            RuleFor(x => x.Request.ColumnIds)
                .NotEmpty();
        }
    }

    internal sealed class CreateAnalyzeQueryCommandHandler : ICommandHandler<CreateAnalyzeQueryCommand, Response<int>>
    {
        private readonly IAnalyzeCommandRepository analyzeCommandRepository;

        public CreateAnalyzeQueryCommandHandler(IAnalyzeCommandRepository analyzeCommandRepository)
        {
            this.analyzeCommandRepository = analyzeCommandRepository;
        }

        public async Task<Response<int>> Handle(CreateAnalyzeQueryCommand command, CancellationToken cancellationToken)
        {
            var queryId = await analyzeCommandRepository.CreateQuery(command.Request, cancellationToken);

            return Response.Ok(queryId);
        }
    }
}
