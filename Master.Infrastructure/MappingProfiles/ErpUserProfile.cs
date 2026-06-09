using AutoMapper;
using Master.DTO.DTOs;
using Master.Entities.Models;

namespace Master.Infrastructure.MappingProfiles
{
    public class ErpUserProfile : Profile
    {
        public ErpUserProfile()
        {
            CreateMap<ErpUsers, ErpUserDTO>().ReverseMap();
        }
    }
}
