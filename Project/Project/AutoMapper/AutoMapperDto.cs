using AutoMapper;
using Project.Dto;
using Project.Models;

namespace Project.AutoMapper
{
    public class AutoMapperDto : Profile
    {
        public AutoMapperDto() 
        {
            CreateMap<User, UserDto>();
            CreateMap<CrudUserDto, User>().BeforeMap((dto, entity)=>
            {
                if(dto != null) dto.Id = Guid.NewGuid();
            });
        }
    }
}
