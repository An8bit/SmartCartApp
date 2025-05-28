using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;
using Web.Models.DTO.UserDTOs;
using Web.Repositories.Interfaces.IServices;
using Web.UserStates;

namespace Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MembershipController : ControllerBase
    {
        private readonly UserStateContext _userStateContext;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public MembershipController(IUserService userService, UserStateContext userStateContext, IMapper mapper)
        {
            _userStateContext = userStateContext;
            _userService = userService;
            _mapper = mapper;
        }

        // API để lấy trạng thái hiện tại của người dùng
        [HttpGet("current-state")]
        public async Task<IActionResult> GetCurrentState([FromQuery] string userId)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userIdClaim == null)
            {
                return BadRequest(new { Success = false, Message = "User ID claim not found" });
            }
            var user = await _userService.GetUserByIdAsync(int.Parse(userIdClaim));

            if (user == null)
            {
                return NotFound("Người dùng không tồn tại.");
            }

            var userState = _userStateContext.GetUserState(user); 
            return Ok(new
            {
                TierName = userState.TierName,
                TierCode = userState.TierCode,
                DiscountPercentage = userState.DiscountPercentage,
                BadgeColor = userState.BadgeColor
            });
        }

        // API để kiểm tra và cập nhật hạng thành viên
        [HttpPost("update-tier")]
        public IActionResult UpdateUserTier([FromBody] UserDtos user)
        {
            if (user == null)
            {
                return BadRequest("Thông tin người dùng không hợp lệ.");
            }

            bool isUpdated = _userStateContext.CheckAndUpdateUserTier(user);

            if (isUpdated)
            {
                return Ok("Hạng thành viên đã được cập nhật.");
            }

            return Ok("Hạng thành viên không thay đổi.");
        }

        
    }
}