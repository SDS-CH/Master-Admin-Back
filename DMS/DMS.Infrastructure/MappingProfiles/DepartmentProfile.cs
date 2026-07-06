using AutoMapper;
using DMS.DTO.DTOs;
using DMS.Entities.Models;

namespace DMS.Infrastructure.MappingProfiles
{
    public class DepartmentProfile : Profile
    {
        public DepartmentProfile()
        {
            CreateMap<Department, DepartmentDTO>().ReverseMap();

            CreateMap<TnCodesTaxis, TnCodesTaxisDTO>().ReverseMap();
        }
    }
}
