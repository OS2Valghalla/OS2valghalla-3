﻿using AutoMapper;

using Valghalla.Database.Entities.Tables;
using Valghalla.Internal.Application.Modules.Administration.WorkLocation.Responses;
using Valghalla.Internal.Application.Modules.Shared.WorkLocation.Responses;
using Valghalla.Internal.Application.Modules.Tasks.Responses;

namespace Valghalla.Internal.Infrastructure.Automapper
{
    internal class WorkLocationProfile : Profile
    {
        public WorkLocationProfile()
        {
            CreateMap<WorkLocationEntity, WorkLocationResponse>()
                .ForMember(f => f.AreaName, opt => opt.MapFrom(src => src.Area != null ? src.Area.Name : string.Empty))
                .ForMember(dest => dest.ElectionId, opt => opt.MapFrom(src => src.ElectionWorkLocations.Select(e => e.ElectionId).FirstOrDefault()))
                .ForMember(dest => dest.TemplateId, opt => opt.MapFrom(src => src.WorkLocationTemplateId != null ? src.WorkLocationTemplateId.ToString() : string.Empty))
                .ForMember(dest => dest.ElectionTitle, opt => opt.MapFrom(src => src.Elections.Select(e => e.Title).FirstOrDefault() ?? string.Empty))
                .ForMember(dest => dest.TemplateTitle, opt => opt.MapFrom(src => src.WorkLocationTemplate != null ? src.WorkLocationTemplate.Title : string.Empty));
            ;
            CreateMap<WorkLocationEntity, WorkLocationDetailResponse>()
                .ForMember(f => f.AreaName, opt => opt.MapFrom(src => src.Area != null ? src.Area.Name : string.Empty))
                .ForMember(f => f.TaskTypeIds, x => x.MapFrom(src => src.WorkLocationTaskTypes.Select(i => i.TaskTypeId)))
                .ForMember(f => f.TaskTypeTemplateIds, x => x.MapFrom(src => src.WorkLocationTaskTypes.Where(wltt => wltt.TaskType != null && wltt.TaskType.TaskTypeTemplate != null).Select(wltt => wltt.TaskType.TaskTypeTemplate.Id)))
                .ForMember(f => f.TeamIds, x => x.MapFrom(src => src.WorkLocationTeams.Select(i => i.TeamId)))
                .ForMember(f => f.ResponsibleIds, x => x.MapFrom(src => src.WorkLocationResponsibles.Select(i => i.ParticipantId)))
                .ForMember(f => f.HasActiveElection, a => a.Ignore())
                .ForMember(f => f.ElectionId, a => a.MapFrom(src => src.ElectionWorkLocations.Select(i => i.ElectionId).FirstOrDefault()));

            CreateMap<WorkLocationResponsibleEntity, WorkLocationResponsibleResponse>()
                .ForMember(f => f.ParticipantFirstName, a => a.Ignore())
                .ForMember(f => f.ParticipantLastName, a => a.Ignore())
                .ForMember(f => f.ParticipantMobileNumber, a => a.Ignore())
                .ForMember(f => f.Id, a => a.Ignore())
                .ForMember(f => f.ParticipantEmail, a => a.Ignore());

            CreateMap<WorkLocationEntity, WorkLocationSharedResponse>();
            CreateMap<WorkLocationEntity, WorkLocationWithTeamIdsResponse>()
                .ForMember(f => f.TeamIds, a => a.Ignore());
        }
    }
}