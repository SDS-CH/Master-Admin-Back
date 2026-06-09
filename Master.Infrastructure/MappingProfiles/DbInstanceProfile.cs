using AutoMapper;
using Master.DTO.DTOs;
using Master.Entities.Models;

namespace Master.Infrastructure.MappingProfiles
{
    public class DbInstanceProfile : Profile
    {
        public DbInstanceProfile()
        {
            CreateMap<DbInstance, DbInstanceDTO>().ReverseMap();
        }
    }
}
