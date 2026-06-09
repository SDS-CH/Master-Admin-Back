using AutoMapper;
using Master.DTO.DTOs;
using Master.Entities.Models;

namespace Master.Infrastructure.MappingProfiles
{
    public class ERPCountriesProfile : Profile
    {
        public ERPCountriesProfile()
        {
            CreateMap<ErpCountries, ErpCountryDTO>().ReverseMap();
        }
    }
}
