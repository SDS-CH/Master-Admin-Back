using AutoMapper;
using Master.DTO.Users;
using Master.Entities.Models;

namespace Master.Infrastructure.MappingProfiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<MasterAdminUsers, MasterAdminUsersDTO>().ReverseMap();
        }
    }
}
