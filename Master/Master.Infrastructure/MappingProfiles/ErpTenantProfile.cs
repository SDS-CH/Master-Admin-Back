using AutoMapper;
using Master.DTO.DTOs;
using Master.Entities.Models;
using System;

namespace Master.Infrastructure.MappingProfiles
{
    public class ErpTenantProfile : Profile
    {
        public ErpTenantProfile()
        {
            CreateMap<ErpTenants, ErpTenantDTO>()
                .ForMember(dest => dest.AddNewTime,
                    opt => opt.MapFrom(src => src.AddNewTime.ToDateTime(TimeOnly.MinValue)));

            CreateMap<ErpTenantDTO, ErpTenants>()
                .ForMember(dest => dest.AddNewTime,
                    opt => opt.MapFrom(src => DateOnly.FromDateTime(
                        src.AddNewTime.HasValue ? src.AddNewTime.Value : DateTime.UtcNow)));
        }
    }
}
