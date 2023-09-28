namespace Valghalla.Internal.Application.Modules.Analyze.Responses
{
    public class AnalyzeListTypeColumnResponse
    {
        public int ColumnId { get; set; }

        public int ListTypeId { get; set; }

        public int Ordinal { get; set; }

        public int RelatedListTypeId { get; set; }

        public AnalyzeColumnResponse Column { get; set; } = null!;
    }
}
