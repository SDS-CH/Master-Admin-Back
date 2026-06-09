using AutoMapper;
using Master.DTO.DTOs;
using Master.Entities.Models;

namespace Master.Infrastructure.MappingProfiles
{
    public class GeddirectoryProfile : Profile
    {
        public GeddirectoryProfile()
        {
            CreateMap<GedDirectory, GedDirectoryDTO>().ReverseMap();
        }
    }
}
