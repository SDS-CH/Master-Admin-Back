using AutoMapper;
using Master.DTO.DTOs;
using Master.Entities.Models;

namespace Master.Infrastructure.MappingProfiles
{
    public class MasterErpmodulesProfile : Profile
    {
        public MasterErpmodulesProfile()
        {
            CreateMap<MasterErpModules, MasterErpmodulesDTO>()
                .ForMember(dest => dest.Dbprefix, opt => opt.MapFrom(src => src.DbPrefix))
                .ReverseMap()
                .ForMember(dest => dest.DbPrefix, opt => opt.MapFrom(src => src.Dbprefix));
        }
    }
}
