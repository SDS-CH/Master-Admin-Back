using AutoMapper;
using Master.DTO.Config;
using Master.Entities.Models;

namespace Master.Infrastructure.MappingProfiles
{
    public class MasterErpmailTemplateProfile : Profile
    {
        public MasterErpmailTemplateProfile()
        {
            CreateMap<MasterErpMailTemplates, MasterErpmailTemplateDTO>().ReverseMap();
        }
    }
}
