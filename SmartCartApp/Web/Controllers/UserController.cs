using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Web.Data;
using Web.Models.Domain;
using Web.Models.DTO;
using Web.Models.DTO.UserDTOs;
using Web.Models.DTO.UserAddressDTOs;
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
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userIdClaim == null)
            {
                return BadRequest(new { Success = false, Message = "User ID claim not found" });
            }
            try
            {
                var user = await _userService.GetUserByIdAsync(int.Parse(userIdClaim));
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
        [HttpGet("ProfileByEmail/{email}")]
        public async Task<IActionResult> GetProfileByEmail(string email)
        {
            
            try
            {
                var user = await _userService.GetUserByEmailAsync(email);
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
            var user = await _userService.GetUserByEmailAsync(loginDto.Email);

            if (user == null)
                return Unauthorized();

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(ClaimTypes.Name, user.Email)
            };

            var claimsIdentity = new ClaimsIdentity(claims, "CookieAuth");
            var authProperties = new Microsoft.AspNetCore.Authentication.AuthenticationProperties
            {
                IsPersistent = true,
                ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(60)
            };

            await HttpContext.SignInAsync("CookieAuth", new ClaimsPrincipal(claimsIdentity), authProperties);

            return Ok(new { Message = "Đăng nhập thành công" });
        }

        [HttpPost("Register")]
        public async Task<ActionResult> RegisterAsync([FromBody] UserRegisterDto registerDto)
        {
            string respon = await _userService.RegisterAsync(registerDto);
            if (respon.Equals("Email đã được đăng ký rồi"))
                return BadRequest(new { message = "Email đã được đăng ký rồi" });
            return Ok("Tạo tài khoản thành công");
        }
        [HttpPost("Logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync("CookieAuth");
            return Ok(new { Message = "Đăng xuất thành công" });
        }
    [HttpPut("UpdateAddress")]
        public async Task<IActionResult> UpdateAddressAsync([FromBody] UserAddressUpdateDto addressDto)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userIdClaim == null)
            {
                return BadRequest(new { Success = false, Message = "User ID claim not found" });
            }
            try
            {
                await _userService.UpdateUserAddressAsync(int.Parse(userIdClaim), addressDto);
                return Ok(new { Message = "Cập nhật địa chỉ thành công" });
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
    }
}
