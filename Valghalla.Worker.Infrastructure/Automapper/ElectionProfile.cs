using AutoMapper;
using Valghalla.Database.Entities.Tables;
using Valghalla.Worker.Infrastructure.Modules.Election.Responses;

namespace Valghalla.Worker.Infrastructure.Automapper
{
    internal class ElectionProfile : Profile
    {
        public ElectionProfile()
        {
            CreateMap<ElectionEntity, ElectionResponse>();
        }
    }
}
