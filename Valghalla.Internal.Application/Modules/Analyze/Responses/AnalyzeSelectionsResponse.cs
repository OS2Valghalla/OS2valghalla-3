namespace Valghalla.Internal.Application.Modules.Analyze.Responses
{
    public class AnalyzeListTypeSelectionsResponse
    {
        public int ListTypeId { get; set; }

        public string Name { get; set; } = null!;

        public ICollection<AnalyzeListTypeColumnResponse> ListTypeColumns { get; set; } = new List<AnalyzeListTypeColumnResponse>();

        public ICollection<AnalyzeListTypeSelectionsResponse> RelatedListTypes { get; set; } = new List<AnalyzeListTypeSelectionsResponse>();
    }
}
