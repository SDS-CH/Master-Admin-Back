using AutoMapper;
using Master.DTO.Config;
using Master.Entities.Models;

namespace Master.Infrastructure.MappingProfiles
{
    public class MasterErpglobalConfigProfile : Profile
    {
        public MasterErpglobalConfigProfile()
        {
            CreateMap<MasterErpGlobalConfigs, MasterErpglobalConfigDTO>().ReverseMap();
        }
    }
}
