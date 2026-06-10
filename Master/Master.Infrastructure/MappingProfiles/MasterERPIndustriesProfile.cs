using AutoMapper;
using Master.DTO;
using Master.Entities.Models;

namespace Master.Infrastructure.MappingProfiles
{
    public class MasterERPIndustriesProfile : Profile
    {
        public MasterERPIndustriesProfile()
        {
            CreateMap<MasterErpIndustries, MasterERPIndustriesDTO>().ReverseMap();
        }
    }
}
