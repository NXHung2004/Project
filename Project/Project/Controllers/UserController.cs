using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Project.Dto;
using Project.Models;
using Project.Modes;
using System.Runtime.Serialization;

namespace Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly DatabaseContext _dbContext;
        public UserController(IMapper mapper ,DatabaseContext dbContext) 
        {
            _mapper = mapper;
            _dbContext = dbContext;
        }
        [HttpGet]
        public async Task<List<UserDto>> GetListAsync()
        {
            var queryable = _dbContext.Users.AsQueryable();
            var users =  queryable.ToList();
            var result = users.Select(p => _mapper.Map<User, UserDto>(p)).ToList();
            return result;
        }
        [HttpPost]
        public async Task<UserDto> CreateAsync(CrudUserDto dto)
        {
            dto.Id = Guid.NewGuid();
            var entity = _mapper.Map<CrudUserDto, User>(dto);
            _dbContext.Users.Add(entity);
            await _dbContext.SaveChangesAsync();
            var result = _mapper.Map<User, UserDto>(entity);
            return result;
        }
    }
}
