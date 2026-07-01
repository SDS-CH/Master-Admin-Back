using AutoMapper;
using DMS.DTO.DTOs;
using DMS.Entities.Models;

namespace DMS.Infrastructure.MappingProfiles
{
    public class ArticleProfile : Profile
    {
        public ArticleProfile()
        {
            CreateMap<TnArticle, ArticleDto>().ReverseMap();
            CreateMap<ProductServiceCategry, ProductServiceCategoryDto>().ReverseMap();
        }
    }
}