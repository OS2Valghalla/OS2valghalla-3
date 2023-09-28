using AutoMapper;
using Valghalla.Database.Entities.Tables;
using Valghalla.Worker.Infrastructure.Modules.Job.Responses;

namespace Valghalla.Worker.Infrastructure.Automapper
{
    internal class JobProfile : Profile
    {
        public JobProfile()
        {
            CreateMap<JobEntity, JobResponse>();
        }
    }
}
