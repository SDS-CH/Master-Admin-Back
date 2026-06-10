using AutoMapper;
using Master.DTO;
using Master.Entities.Models;

namespace Master.Infrastructure.MappingProfiles
{
    public class MasterERPIndustriesClientProfile : Profile
    {
        public MasterERPIndustriesClientProfile()
        {
            CreateMap<MasterErpIndustriesClient, MasterERPIndustriesClientDTO>().ReverseMap();
        }
    }
}
