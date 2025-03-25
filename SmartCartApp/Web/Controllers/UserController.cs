using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Web.Data;
using Web.Models.Domain;
using Web.Models.DTO;
using Web.Models.DTO.UrserDTOs;
using Web.Repositories.Contracts;
using Web.Repositories.Interfaces.IServices;
using Web.Services;

namespace Web.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        // Get current user profile
        [HttpGet("Profile")]
        public async Task<IActionResult> GetProfile()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            try
            {
                var user = await _userService.GetUserByIdAsync(userId);
                return Ok(user);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while processing your request", error = ex.Message });
            }
        }
        [HttpPost("Login")]
        public async Task<ActionResult> LoginAsync([FromBody] UserLoginDto loginDto)
        {
            var token = await _userService.LoginAsync(loginDto);
            if (token == null)
                return Unauthorized();
            return Ok(token);
        }

        [HttpPost("Register")]
        public async Task<ActionResult> RegisterAsync([FromBody] UserRegisterDto registerDto)
        {
            string respon = await _userService.RegisterAsync(registerDto);
            if (respon.Equals("Email đã được đăng ký rồi"))
                return BadRequest("Email đã được đăng ký rồi");
            return Ok("Tạo tài khoản thành công");
        }
    }
}
