using AutoMapper;
using Master.DTO.DTOs;
using Master.Entities.Models;

namespace Master.Infrastructure.MappingProfiles
{
    public class MasterErpmoduleClientProfile : Profile
    {
        public MasterErpmoduleClientProfile()
        {
            CreateMap<MasterErpModuleClient, MasterErpmoduleClientDTO>().ReverseMap();
        }
    }
}
