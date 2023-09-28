using AutoMapper;
using Valghalla.Database.Entities.Tables;
using Valghalla.Internal.Application.Modules.Administration.SpecialDiet.Responses;
using Valghalla.Internal.Application.Modules.Shared.SpecialDiet.Responses;

namespace Valghalla.Internal.Infrastructure.Automapper
{
    internal class SpecialDietProfile : Profile
    {
        public SpecialDietProfile()
        {
            CreateMap<SpecialDietEntity, SpecialDietResponse>();
            CreateMap<SpecialDietEntity, SpecialDietSharedResponse>();
        }
    }
}
