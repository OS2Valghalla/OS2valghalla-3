namespace Valghalla.Application.Excel
{
    public class ExcelModel
    {
        public IList<ExcelHeader> Headers { get; set; } = new List<ExcelHeader>();
        public IList<Dictionary<string, string>> Rows { get; set; } = new List<Dictionary<string, string>>();
    }

    public class ExcelHeader
    {
        public string HeaderName { get; set; } = null!;
        public string PropertyName { get; set; } = null!;
        public Type PropertyType { get; set; } = null!;
    }
}
