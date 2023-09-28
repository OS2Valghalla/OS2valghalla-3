namespace Valghalla.Application.Excel
{
    public interface IExcelService
    {
        MemoryStream GetExcelStream(ExcelModel excelModel, CancellationToken cancellationToken);
    }
}
