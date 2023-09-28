using AutoMapper;
using Valghalla.Application.Storage;
using Valghalla.Database.Entities.Tables;

namespace Valghalla.Database.Automapper
{
    internal class FileStorageProfile : Profile
    {
        public FileStorageProfile()
        {
            CreateMap<FileReferenceEntity, FileReferenceInfo>();
        }
    }
}
