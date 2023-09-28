using AutoMapper;
using Valghalla.Database.Entities.Analyze;
using Valghalla.Internal.Application.Modules.Analyze.Responses;

namespace Valghalla.Internal.Infrastructure.Automapper
{
    internal class AnalyzeProfile : Profile
    {
        public AnalyzeProfile()
        {
            CreateMap<ListTypeEntity, AnalyzeListTypeResponse>();

            CreateMap<ListTypeEntity, AnalyzeListTypeSelectionsResponse>();

            CreateMap<ListTypeColumnEntity, AnalyzeListTypeColumnResponse>();

            CreateMap<ColumnEntity, AnalyzeColumnResponse>();

            CreateMap<QueryEntity, AnalyzeQueryResponse>();

            CreateMap<QueryEntity, AnalyzeQueryDetailResponse>()
                .ForMember(x => x.ResultColumns, y => y.MapFrom(src => src.ResultColumns != null ? src.ResultColumns.Select(c => c.ColumnId).ToList() : new List<int>()));
        }
    }
}
