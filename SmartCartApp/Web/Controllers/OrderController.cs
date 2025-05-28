using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Web.Models.DTO.OrderDTOs;
using Web.Models.DTO.UserDTOs;
using Web.Repositories.Interfaces.IServices;

namespace Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly IUserService _userService;
        private const string SessionIdCookieName = "CartSessionId";

        public OrderController(IOrderService orderService,IUserService userService)
        {
            _orderService = orderService;
            _userService = userService;
        }


        [HttpGet]
        public async Task<IActionResult> GetUserOrders()
        {
            try
            {
                var userId = GetUserId();
                var orders = await _orderService.GetUserOrdersAsync(userId);
                return Ok(orders);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        // GET: api/Order/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderById(int id)
        {
            try
            {
                var userId = GetUserId();
                
                var order = await _orderService.GetOrderByIdAsync(id, userId);
                return Ok(order);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Forbid();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        // POST: api/Order
        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderDto createOrderDto)
        {
            try
            {
                var userId = GetUserId();
               if(userId == 0)
                {
                    return BadRequest(new { message = "Đang nhập để tiếp tục thanh đặt hàng" });
                }
                var order = await _orderService.CreateOrderFromCartAsync(userId, createOrderDto);
                //decimal totalAmount =  await _orderService.GetTotalAmount(userId);
                //await _userService.UpdateUserAsync(userId, new UserUpdateDto
                //{
                //    TotalSpending = totalAmount,
                   
                //});

                return CreatedAtAction(nameof(GetOrderById), new { id = order.OrderId }, order);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message ="lỖI "+ ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "CreateOrder " + ex.Message });
            }
        }

        // PUT: api/Order/{id}/status
        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateOrderStatus(int id, [FromBody] OrderStatusUpdateDto updateDto)
        {
            try
            {
                var userId = GetUserId();
                var order = await _orderService.UpdateOrderStatusAsync(id, userId, updateDto);
                return Ok(order);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Forbid();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        // GET: api/Order/{id}/history
        [HttpGet("{id}/history")]
        public async Task<IActionResult> GetOrderHistory(int id)
        {
            try
            {
                var userId = GetUserId();
                var history = await _orderService.GetOrderHistoryAsync(id, userId);
                return Ok(history);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Forbid();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        // GET: api/Order/admin
        [HttpGet("admin")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllOrders([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                var orders = await _orderService.GetAllOrdersAsync(page, pageSize);
                return Ok(orders);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        // GET: api/Order/statistics
        [HttpGet("statistics")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetOrderStatistics(
            [FromQuery] DateTime? startDate = null,
            [FromQuery] DateTime? endDate = null)
        {
            try
            {
                var statistics = await _orderService.GetOrderStatisticsAsync(startDate, endDate);
                return Ok(statistics);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        // Helper method to get user ID from claims
        private int GetUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
            {
                throw new UnauthorizedAccessException("Invalid user identification");
            }
            return userId;
        }

        private string GetOrCreateSessionId()
        {
            // Kiểm tra cookies trước
            if (Request.Cookies.TryGetValue("CartSessionId", out var existingSessionId) && !string.IsNullOrEmpty(existingSessionId))
            {
                return existingSessionId;
            }

            // Kiểm tra trong request body hoặc query
            var sessionIdFromRequest = HttpContext.Request.Query["sessionId"].ToString();
            if (string.IsNullOrEmpty(sessionIdFromRequest))
            {
                // Cố gắng đọc từ body (nếu có)
                try
                {
                    // Logic đọc từ body (tùy thuộc vào framework)
                }
                catch { }
            }

            if (!string.IsNullOrEmpty(sessionIdFromRequest))
            {
                // Sử dụng sessionId từ request
                SetSessionId(sessionIdFromRequest);
                return sessionIdFromRequest;
            }
            return "";
            
        }
        private void SetSessionId(string sessionId)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddDays(30),
                SameSite = SameSiteMode.Lax,
                Secure = false,
                Domain = null,
            };

            Response.Cookies.Append(SessionIdCookieName, sessionId, cookieOptions);
        }
        private string? GetSessionId()
        {
            return Request.Cookies[SessionIdCookieName];
        }
    }
}