using LMS_WebApi.DTO;
using LMS_WebApi.Enum;
using LMS_WebApi.Interface;
using LMS_WebApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using System.IdentityModel.Tokens.Jwt;

namespace LMS_WebApi.Controller
{
    [ApiController]
    [Route("api/v1/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IMemberService<Member> _userService;

        public AuthController(IConfiguration configuration, IMemberService<Member> userService)
        {
            _configuration = configuration;
            _userService = userService;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginRequestDto loginRequestDto)
        {
            // Check request body if valid
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _userService.GetByEmailAsync(loginRequestDto.Email);
            bool isVerify = BCrypt.Net.BCrypt.EnhancedVerify(loginRequestDto.Password, user.Password);
            if (user == null || !isVerify)
            {
                return Unauthorized();
            }

            if ((bool)user.IsBanned)
            {
                // Add custom error message for more detail
                return Unauthorized();
            }

            if ((bool)user.IsLocked)
            {
                return Unauthorized();

            }

            var token = GenerateJwtToken(user.Email, user.Id, user.UserRole);
            return Ok(new { token });
        }

        private string GenerateJwtToken(string email, Guid id, UserRole userRole)
        {
            // Check and assign the user role
            string role = userRole switch
            {
                UserRole.ADMIN => "ADMIN",
                UserRole.MEMBER => "MEMBER",
                UserRole.LIBRARIAN => "LIBRARIAN",
                _ => "MEMBER",
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);

            var tokenDescriptor = new Microsoft.IdentityModel.Tokens.SecurityTokenDescriptor
            {
                Subject = new System.Security.Claims.ClaimsIdentity(
                [
                new Claim("userId", id.ToString()),
                new Claim("userEmail", email),
                new Claim(ClaimTypes.Role, role)
                ]),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new Microsoft.IdentityModel.Tokens.SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}
