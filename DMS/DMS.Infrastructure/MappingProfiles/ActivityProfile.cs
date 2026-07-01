using AutoMapper;
using DMS.DTO.DTOs;
using DMS.Entities.Models;

namespace DMS.Infrastructure.MappingProfiles
{
    public class ActivityProfile : Profile
    {
        public ActivityProfile()
        {
            CreateMap<TnActivite, ActivityDto>().ReverseMap();
        }
    }
}
