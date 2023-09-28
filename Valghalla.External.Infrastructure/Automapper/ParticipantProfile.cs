using AutoMapper;
using Valghalla.Database.Entities.Tables;
using Valghalla.External.Application.Modules.MyProfile.Responses;
using Valghalla.External.Application.Modules.Registration.Responses;
using Valghalla.External.Application.Modules.WorkLocation.Responses;

namespace Valghalla.External.Infrastructure.Automapper
{
    internal class ParticipantProfile : Profile
    {
        public ParticipantProfile()
        {
            CreateMap<ParticipantEntity, MyProfileResponse>()
                .ForMember(dest => dest.ParticipantId, opts => opts.MapFrom(src => src.Id))
                .ForMember(dest => dest.SpecialDietIds, opts => opts.MapFrom(src => src.SpecialDietParticipants.Select(i => i.SpecialDietId)));

            CreateMap<ParticipantEntity, MyProfileRegistrationResponse>()
                .ForMember(dest => dest.ParticipantId, opts => opts.MapFrom(src => src.Id))
                .ForMember(dest => dest.RegisteredTeamName, opts => opts.MapFrom(src => src.TeamForMembers.First().Name))
                .ForMember(dest => dest.SpecialDietIds, opts => opts.MapFrom(src => src.SpecialDietParticipants.Select(i => i.SpecialDietId)));

            CreateMap<ParticipantEntity, WorkLocationParticipantDetailsResponse>()
                .ForMember(dest => dest.FullName, opts => opts.MapFrom(src => src.FirstName + " " + src.LastName))
                .ForMember(dest => dest.TaskTypes, opt => opt.Ignore())
                .ForMember(dest => dest.Teams, opt => opt.Ignore());
        }
    }
}
