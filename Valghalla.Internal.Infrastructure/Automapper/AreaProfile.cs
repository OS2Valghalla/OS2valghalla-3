using AutoMapper;
using Valghalla.Database.Entities.Tables;
using Valghalla.Internal.Application.Modules.Administration.Area.Responses;
using Valghalla.Internal.Application.Modules.Shared.Area.Responses;

namespace Valghalla.Internal.Infrastructure.Automapper
{
    internal class AreaProfile : Profile
    {
        public AreaProfile()
        {
            CreateMap<AreaEntity, AreaListingItemResponse>();
            CreateMap<AreaEntity, AreaDetailsResponse>();
            CreateMap<AreaEntity, AreaSharedResponse>();
        }
    }
}
