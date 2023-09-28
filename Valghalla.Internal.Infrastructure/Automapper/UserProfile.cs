using AutoMapper;
using Valghalla.Database.Entities.Tables;
using Valghalla.Internal.Application.Modules.Administration.User.Responses;
using Valghalla.Internal.Application.Modules.App.Responses;

namespace Valghalla.Internal.Infrastructure.Automapper
{
    internal class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<UserEntity, GetUsersItem>()
                .ForMember(dest => dest.SocialSecurityNumber, opt => opt.Ignore())
                .ForMember(dest => dest.Identifier, opt => opt.Ignore());

        }
    }
}
