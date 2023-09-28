using AutoMapper;
using Valghalla.Application.TaskValidation;
using Valghalla.Database.Entities.Tables;

namespace Valghalla.Infrastructure.Automapper
{
    internal class ParticipantProfile : Profile
    {
        public ParticipantProfile()
        {
            CreateMap<ParticipantEntity, EvaluatedParticipant>();
        }
    }
}
