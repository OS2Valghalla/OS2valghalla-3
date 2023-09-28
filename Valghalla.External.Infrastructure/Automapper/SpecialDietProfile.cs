using AutoMapper;
using Valghalla.Database.Entities.Tables;
using Valghalla.External.Application.Modules.Shared.SpecialDiet.Responses;

namespace Valghalla.External.Infrastructure.Automapper
{
    internal class SpecialDietProfile : Profile
    {
        public SpecialDietProfile()
        {
            CreateMap<SpecialDietEntity, SpecialDietSharedResponse>();
        }
    }
}
