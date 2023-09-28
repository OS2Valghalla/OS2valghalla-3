using AutoMapper;
using Valghalla.Database.Entities.Tables;
using Valghalla.Internal.Application.Modules.Administration.AuditLog.Responses;

namespace Valghalla.Internal.Infrastructure.Automapper
{
    internal class AuditLogProfile : Profile
    {
        public AuditLogProfile()
        {
            CreateMap<AuditLogEntity, AuditLogListingItemResponse>()
                .ForMember(dest => dest.DoneBy, opts => opts.MapFrom(src => src.DoneByUser != null ? src.DoneByUser.Name : null));
        }
    }
}
