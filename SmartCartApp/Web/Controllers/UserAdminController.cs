using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web.Models.DTO.UserDTOs;
using Web.Repositories.Interfaces.IServices;
using Web.UserStates;

namespace Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
   // [Authorize(Roles = "Admin")]
   public class UserAdminController : ControllerBase
   {
       private readonly IUserService _userService;
        private readonly UserStateContext _userStateContext;

        public UserAdminController(IUserService userService, UserStateContext userStateContext)
        {
            _userService = userService;
            _userStateContext = userStateContext;
        }

        [HttpGet]
       public async Task<ActionResult<IEnumerable<UserDtos>>> GetAllUsers()
       {
           var users = await _userService.GetAllUsersAsync();
           return Ok(users);
       }

       [HttpGet("{id}")]
       public async Task<ActionResult<UserDtos>> GetUserById(int id)
       {
           try
           {
               var user = await _userService.GetUserByIdAsync(id);
               return Ok(user);
           }
           catch (KeyNotFoundException ex)
           {
               return NotFound(ex.Message);
           }
       }

       [HttpPost]
       public async Task<ActionResult<UserDtos>> CreateUser([FromBody] UserCreateDto createDto)
       {
           try
           {
               var createdUser = await _userService.CreateUserAsync(createDto);
               return CreatedAtAction(nameof(GetUserById), new { id = createdUser.UserId }, createdUser);
           }
           catch (InvalidOperationException ex)
           {
               return BadRequest(ex.Message);
           }
       }

       [HttpPut("{id}")]
       public async Task<ActionResult<UserDtos>> UpdateUser(int id, [FromBody] UserAdminUpdateDto updateDto)
       {
           try
           {
               var updatedUser = await _userService.AdminUpdateUserAsync(id, updateDto);
               return Ok(updatedUser);
           }
           catch (KeyNotFoundException ex)
           {
               return NotFound(ex.Message);
           }
           catch (InvalidOperationException ex)
           {
               return BadRequest(ex.Message);
           }
       }

       [HttpDelete("{id}")]
       public async Task<IActionResult> DeleteUser(int id)
       {
           try
           {
               await _userService.DeleteUserAsync(id);
               return NoContent();
           }
           catch (KeyNotFoundException ex)
           {
               return NotFound(ex.Message);
           }
       }

       [HttpGet("email/{email}")]
       public async Task<ActionResult<UserDtos>> GetUserByEmail(string email)
       {
           try
           {
               var user = await _userService.GetUserByEmailAsync(email);
               return Ok(user);
           }
           catch (KeyNotFoundException ex)
           {
               return NotFound(ex.Message);
           }
       }

        
    }
}