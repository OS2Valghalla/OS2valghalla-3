namespace Valghalla.Internal.Application.Modules.Analyze.Responses
{
    public class AnalyzeResult
    {
        public IEnumerable<AnalyzeResultColumn> HeaderMappings { get; set; } = new List<AnalyzeResultColumn>();
        public List<dynamic> Data { get; set; } = new List<dynamic>();
        public int Count { get; set; }
    }
    public class AnalyzeResultColumn
    {
        public string PropertyName { get; set; } = null!;
        public string HeaderName { get; set; } = null!;
        public int Ordinal { get; set; }
        public dynamic Type { get; set; }
    }
}
