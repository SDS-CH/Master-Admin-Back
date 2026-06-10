using AutoMapper;
using Master.DTO.DTOs;
using Master.Entities.Models;

namespace Master.Infrastructure.MappingProfiles
{
    public class CurrencyProfile : Profile
    {
        public CurrencyProfile()
        {
            CreateMap<Currency, CurrencyDTO>().ReverseMap();
        }
    }
}
