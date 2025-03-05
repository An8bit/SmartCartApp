using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Web.Data;
using Web.Models.Domain;
using Web.Models.DTO;
using Web.Repositories.Contracts;
using Web.Services;

namespace Web.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;

        public UserController(UserService userService)
        {
            _userService = userService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllUser()
        {
            var usersDto = await _userService.GetAllUser();
            if (usersDto == null)
            {
                return BadRequest(new { message = "Lỗi dữ liệu" });
            }
            return Ok(UserDto.ConvertToDtoList(usersDto));
        }
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await _userService.GetUserById(id);
            if (user == null)
            {
                return BadRequest(new { message = "Không tìm thấy User" });
            }
            return Ok(UserDto.ConvertToDto(user));
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] AddUserRequestDto addUserRequestDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                await _userService.CreateUser(addUserRequestDto);
                return Ok(new { message = "Tạo thành công User" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteUserById(int id)
        {
            var result = await _userService.DeleteUserById(id);
            if (!result)
            {
                return BadRequest(new { massage = "Xóa User thất bại" });
            }
            return Ok(new { message = "Xóa User Thành Công" });
        }
        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> UpdateUserById(int id, [FromBody] UpdateUserRequestDto updateUserRequestDto)
        {

            bool result = await _userService.UpdateUserById(id, updateUserRequestDto);
            if (!result)
            {
                return BadRequest(new { message = "Cập nhật User thất bại" });
            }
            return Ok(new { message = "Cập nhật User Thành Công" });
        }

        [HttpGet]
        [Route("email/{email}")]
        public async Task<IActionResult> FindUserByEmail(string email)
        {
            var user = await _userService.GetUserByEmail(email);
            if (user == null)
            {
                return BadRequest(new { message = "Không tìm thấy User" });
            }
            return Ok(UserDto.ConvertToDto(user));
        }
        [HttpGet]
        [Route("{id}/orders")]
        public async Task<IActionResult> GetOrdersByUserId(int id)
        {
            var user = await _userService.GetOrdersByUserId(id);
            if (user == null)
            {
                return BadRequest(new { message = "Không tìm thấy User" });
            }
            return Ok(UserDto.ConvertToDto(user));
        }
    }
}
