using AutoMapper;
using Valghalla.Database.Entities.Tables;
using Valghalla.Worker.Infrastructure.Modules.Participant.Responses;

namespace Valghalla.Worker.Infrastructure.Automapper
{
    internal class ParticipantProfile : Profile
    {
        public ParticipantProfile()
        {
            CreateMap<ParticipantEntity, ParticipantResponse>();
        }
    }
}
