using AutoMapper;
using Master.DTO.DTOs;
using Master.Entities.Models;
using System;

namespace Master.Infrastructure.MappingProfiles
{
    public class ErpUserTenantProfile : Profile
    {
        public ErpUserTenantProfile()
        {
            CreateMap<ErpUserTenants, ErpUserTenantDTO>()
                .ForMember(dest => dest.AddNewTime,
                    opt => opt.MapFrom(src => src.AddNewTime.ToDateTime(TimeOnly.MinValue)))
                .ForMember(dest => dest.TenantGuid,
                    opt => opt.MapFrom(src => src.Tenant != null ? src.Tenant.TenantId : Guid.Empty))
                .ForMember(dest => dest.EntityName,
                    opt => opt.MapFrom(src => src.Tenant != null ? src.Tenant.EntityName : null));

            CreateMap<ErpUserTenantDTO, ErpUserTenants>()
                .ForMember(dest => dest.AddNewTime,
                    opt => opt.MapFrom(src => DateOnly.FromDateTime(
                        src.AddNewTime == default ? DateTime.UtcNow : src.AddNewTime)));
        }
    }
}
