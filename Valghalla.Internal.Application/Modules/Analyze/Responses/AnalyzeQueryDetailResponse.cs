namespace Valghalla.Internal.Application.Modules.Analyze.Responses
{
    public class AnalyzeQueryDetailResponse
    {
        public int QueryId { get; set; }
        public int ListTypeId { get; set; }
        public string Name { get; set; } = null!;
        public IList<int> ResultColumns { get; set; } = new List<int>();
    }
}
