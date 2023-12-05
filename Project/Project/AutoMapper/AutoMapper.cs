using AutoMapper;
using Project.Dto;
using Project.Models;

namespace Project.AutoMapper
{
    public class AutoMapper : Profile
    {
        public AutoMapper() 
        {
            CreateMap<User, UserDto>();
            CreateMap<CrudUserDto, User>();
        }
    }
}
