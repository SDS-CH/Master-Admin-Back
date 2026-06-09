using AutoMapper;
using Master.DTO.DTOs;
using Master.Entities.Models;

namespace Master.Infrastructure.MappingProfiles
{
    public class MasterERPDatabaseRefProfile : Profile
    {
        public MasterERPDatabaseRefProfile()
        {
            CreateMap<MasterErpDatabaseRef, MasterERPDatabaseRefDTO>()
                .ForMember(dest => dest.DbInstanceId, opt => opt.MapFrom(src => src.SqlServerInstanceId))
                .ReverseMap()
                .ForMember(dest => dest.SqlServerInstanceId, opt => opt.MapFrom(src => src.DbInstanceId));
        }
    }
}
