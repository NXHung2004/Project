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
        public UserController(IMapper mapper, DatabaseContext dbContext)
        {
            _mapper = mapper;
            _dbContext = dbContext;
        }
        [HttpGet]
        public async Task<List<UserDto>> GetListAsync()
        {
            var queryable = _dbContext.Users.AsQueryable();
            var users = queryable.ToList();
            var result = users.Select(p => _mapper.Map<User, UserDto>(p)).ToList();
            return result;
        }
        [HttpPost]
        public async Task<UserDto> CreateAsync(CrudUserDto dto)
        {
            var entity = _mapper.Map<CrudUserDto, User>(dto);
            entity.Password = HashPassword("123456@Aa");
            entity.TimeNew = DateTime.Now;
            _dbContext.Users.Add(entity);
            await _dbContext.SaveChangesAsync();
            var result = _mapper.Map<User, UserDto>(entity);
            return result;
        }
        [HttpPut]
        public async Task<UserDto> UpdateAsync(CrudUserDto dto, Guid userId)
        {
            var user = await GetById(userId);
            if(user == null)
            {
                throw new Exception($"user with ID: {userId} not found!");
            }
            _mapper.Map<CrudUserDto, User>(dto, user);
            _dbContext.Users.Update(user);
            await _dbContext.SaveChangesAsync();
            var result = _mapper.Map<User, UserDto>(user);
            return result;
        }
        [HttpPut("UpdatePasword")]
        public async Task<string> UpdatePasswordAsync(Guid userId, string newPassword)
        {
            var user = await GetById(userId);
            if (user == null)
            {
                throw new Exception($"user with ID: {userId} not found!");
            }
            user.Password = HashPassword(newPassword);
            _dbContext.Users.Update(user);
            await _dbContext.SaveChangesAsync();
            return "Password changed successfully!";
        }
        private async Task<User> GetById(Guid id)
        {
            var queryable = _dbContext.Users.AsQueryable();
            queryable = queryable.Where(p => p.Id == id);
            var result = queryable.FirstOrDefault();
            return result;
        }
        private string HashPassword(string password)
        {
            // Tạo salt mới cho mỗi mật khẩu
            string salt = BCrypt.Net.BCrypt.GenerateSalt();

            // Hash mật khẩu với salt
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password, salt);

            return hashedPassword;
        }
    }
}
