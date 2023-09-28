namespace Valghalla.Internal.Application.Modules.Analyze.Requests
{
    public class CreateQueryRequest
    {
        public Guid ElectionId { get; set; }
        public int ListTypeId { get; set; }
        public IList<int> ColumnIds { get; set; } = new List<int>();
        public IList<AnalyzeQueryRelatedListTypeRequest> RelatedListTypes { get; set; } = new List<AnalyzeQueryRelatedListTypeRequest>();
    }

    public class AnalyzeQueryRelatedListTypeRequest
    {
        public int ListTypeId { get; set; }
        public IList<int> ColumnIds { get; set; } = new List<int>();
    }
}
