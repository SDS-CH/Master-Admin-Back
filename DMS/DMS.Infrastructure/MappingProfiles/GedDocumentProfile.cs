using AutoMapper;
using DMS.DTO.DTOs;
using DMS.Entities.Models;

namespace DMS.Infrastructure.MappingProfiles
{
    public class GedDocumentProfile : Profile
    {
        public GedDocumentProfile()
        {
            CreateMap<GedDocumentCategory, GedDocumentCategoryDto>().ReverseMap();
            CreateMap<GedDocumentType, GedDocumentTypeDto>().ReverseMap();
        }
    }
}
