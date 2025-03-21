using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        [HttpGet]
        public async Task<ActionResult> GetAllUsersAsync()
        {
            
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }


        [HttpGet("{id}")]
        public async Task<ActionResult> GetUserByIdAsync(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
                return NotFound();
            return Ok(user);
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
