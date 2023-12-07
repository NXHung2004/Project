using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using OfficeOpenXml.FormulaParsing.LexicalAnalysis;
using Project.Models;
using Project.Modes;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;

namespace Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly DatabaseContext _dbContext;
        private readonly IMapper _mapper;
        public LoginController(DatabaseContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
        [HttpPost]
        public async Task<IActionResult> LoginAsync(string userName, string password)
        {
            var user = await GetByUser(userName);
            if (user == null)
            {
                throw new Exception($"user with username: {userName} not found!");
            }
            var status = BCrypt.Net.BCrypt.Verify(password, user.Password);
            if (status)
            {
                var claims = new List<Claim>
                {
                     new Claim(ClaimTypes.Name, user.UserName),
                     new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
                };
                var token = CreateToken(claims);
                var tokenInfo = GetJwtTokenInfo(token);
                return Ok(new { Token = token, TokenType = tokenInfo.Item1, AlgorithmType = tokenInfo.Item2});
            }
            return null;
        }

        private async Task<User> GetByUser(string userName)
        {
            var queryable = _dbContext.Users.AsQueryable();
            queryable = queryable.Where(p => p.UserName == userName);
            return await queryable.FirstOrDefaultAsync();
        }
        private string CreateToken(List<Claim> claims)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("ABCDeujujusik@!!!Ashsnskajuh"));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1), // Thời gian hết hạn của token
                signingCredentials: credentials
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            return tokenString;
        }
        private Tuple<string, string> GetJwtTokenInfo(string token)
        {
            try
            {
                var handler = new JwtSecurityTokenHandler();
                var jsonToken = handler.ReadToken(token) as JwtSecurityToken;

                if (jsonToken != null)
                {
                    var tokenType = jsonToken.Header.Alg;
                    return Tuple.Create("JWT", tokenType);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Token decoding error: {ex.Message}");
            }

            return Tuple.Create("UnknownType", "UnknownAlgorithm");
        }

    }
}
