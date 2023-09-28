using FluentValidation;
using MediatR;
using System.Text.Json;
using Valghalla.Application.Abstractions.Messaging;
using Valghalla.Application.Excel;
using Valghalla.Internal.Application.Modules.Analyze.Interfaces;

namespace Valghalla.Internal.Application.Modules.Analyze.Queries
{
    public sealed record ExportQueryToExcelQuery(Guid ElectionId, int QueryId) : IRequest<Response<MemoryStream>>;

    public sealed class ExportQueryToExcelQueryValidator : AbstractValidator<ExportQueryToExcelQuery>
    {
        public ExportQueryToExcelQueryValidator()
        {
            RuleFor(x => x.ElectionId)
                .NotEmpty();

            RuleFor(x => x.QueryId)
                .GreaterThan(0);
        }
    }

    internal sealed class ExportQueryToExcelQueryHandler : IRequestHandler<ExportQueryToExcelQuery, Response<MemoryStream>>
    {
        private readonly IAnalyzeQueryRepository analyzeQueryRepository;
        private readonly IExcelService excelService;

        public ExportQueryToExcelQueryHandler(IAnalyzeQueryRepository analyzeQueryRepository, IExcelService excelService)
        {
            this.analyzeQueryRepository = analyzeQueryRepository;
            this.excelService = excelService;
        }

        public async Task<Response<MemoryStream>> Handle(ExportQueryToExcelQuery query, CancellationToken cancellationToken)
        {
            var queryResult = await analyzeQueryRepository.GetQueryResult(query.ElectionId, query.QueryId, 0, null);
            var excelModel = new ExcelModel
            {
                Headers = queryResult.HeaderMappings.Select(x => new ExcelHeader
                {
                    HeaderName = x.HeaderName,
                    PropertyName = x.PropertyName,
                    //PropertyType = x.Type // skip for now
                }).ToList()
            };
            string json = JsonSerializer.Serialize(queryResult.Data);
            excelModel.Rows = JsonSerializer.Deserialize<List<Dictionary<string, string>>>(json);

            var ms = excelService.GetExcelStream(excelModel, cancellationToken);

            return Response.Ok(ms);
        }
    }
}
