using AutoMapper;
using LMS_WebApi.DTO;
using LMS_WebApi.Interface;
using LMS_WebApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;

namespace LMS_WebApi.Controller
{
    [ApiController]
    [Route("api/v1/member")]
    public class MemberController : ControllerBase
    {
        private readonly IMemberService<Member> _userService;
        private readonly IMapper _mapper;

        public MemberController(IMemberService<Member> userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }

        [HttpGet]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userService.GetAllAsync();
            var usersResponse = users.Select(_mapper.Map<MemberResponseDto>);
            return Ok(usersResponse);
        }

        [HttpGet("me")]
        public async Task<IActionResult> GetCurrentUser()
        {
            string token = null;

            // Step 1: Check for the token in the Authorization header
            var authHeader = Request.Headers["Authorization"].ToString();
            if (!string.IsNullOrEmpty(authHeader) && authHeader.StartsWith("Bearer "))
            {
                token = authHeader["Bearer ".Length..].Trim();
            }
            // Step 2: If no token is found in the Authorization header, check the authToken cookie
            if (string.IsNullOrEmpty(token))
            {
                token = Request.Cookies["authToken"];
            }

            // Step 3: If no token is found in both the Authorization header and the cookie, return Unauthorized
            if (string.IsNullOrEmpty(token))
            {
                return Unauthorized();
            }

            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadToken(token) as JwtSecurityToken;

            // Assuming the userId is stored in the token's claims
            var userIdClaim = jwtToken?.Claims.FirstOrDefault(claim => claim.Type == "userId");
            if (userIdClaim == null)
            {
                return Unauthorized();
            }

            var userId = Guid.Parse(userIdClaim.Value);
            var user = await _userService.GetByIdAsync(userId);

            if (user == null)
            {
                return NotFound();
            }

            var userResponse = _mapper.Map<MemberResponseDto>(user);
            return Ok(userResponse);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> GetUserById(Guid id)
        {
            var user = await _userService.GetByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            var userResponse = _mapper.Map<MemberResponseDto>(user);
            return Ok(userResponse);
        }

        [HttpPost]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> AddUser(MemberRequestDto userRequestDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = _mapper.Map<Member>(userRequestDto);
            var createdUser = await _userService.AddAsync(user);
            var userRes = _mapper.Map<MemberResponseDto>(createdUser);
            return Ok(userRes);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> UpdateUser(Guid id, MemberUpdateRequestDto userUpdateRequestDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = _mapper.Map<Member>(userUpdateRequestDto);
            var updatedUser = await _userService.UpdateAsync(id, user);
            var userRes = _mapper.Map<MemberResponseDto>(updatedUser);
            return Ok(userRes);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            await _userService.DeleteAsync(id);
            return NoContent();
        }
    }
}
