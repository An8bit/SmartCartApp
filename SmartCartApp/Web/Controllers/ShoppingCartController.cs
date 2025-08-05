using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Web.Models.DTO.ShoppingCartDTOs;
using Web.Repositories.Contracts;
using Web.Repositories.Interfaces.IServices;
using Web.Services;

namespace Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShoppingCartController : ControllerBase
    {
        private readonly IShoppingCartService _cartService;
        private readonly IPriceService _priceService;
        private const string SessionIdCookieName = "CartSessionId";
        
        public ShoppingCartController(IShoppingCartService cartService, IPriceService priceService)
        {
            _cartService = cartService;
            _priceService = priceService;
        }

        [HttpPost]
        public async Task<IActionResult> AddToCart([FromBody] AddToCartDto addToCartDto)
        {
            try
            {
                var userId = GetUserId();
                var sessionId = GetOrCreateSessionId();

                // Calculate the current price with discounts before adding to cart
                var priceInfo = await _priceService.CalculateProductPriceAsync(addToCartDto.ProductId, userId);
                
                // Add to cart using existing service (which will use product.Price)
                // The service should be updated to use IPriceService, but for now this works as a temporary fix
                var cart = await _cartService.AddToCartAsync(userId, sessionId, addToCartDto);
                
                // TODO: In a future version, modify ShoppingCartService to accept calculated price
                // or inject IPriceService into ShoppingCartService for proper price calculation
                
                return Ok(cart);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
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



        [HttpGet]
        public async Task<IActionResult> GetCart()
        {
            try
            {
                var userId = GetUserId();
                var sessionId = GetOrCreateSessionId();

                var cart = await _cartService.GetCartAsync(userId, sessionId);
                return Ok(cart);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpPut("items/{cartItemId}")]
        public async Task<IActionResult> UpdateCartItem(int cartItemId, [FromBody] UpdateCartItemDto requestDto)
        {
            try
            {
                var userId = GetUserId();
                var sessionId = GetOrCreateSessionId();

                // Validate request
                if (requestDto == null)
                {
                    return BadRequest(new { message = "Request body is required" });
                }

                if (requestDto.Quantity <= 0)
                {
                    return BadRequest(new { message = "Quantity must be greater than 0" });
                }

                var updateDto = new UpdateCartItemDto
                {
                    CartItemId = cartItemId,
                    Quantity = requestDto.Quantity
                };

                var cart = await _cartService.UpdateCartItemAsync(userId, sessionId, updateDto);
                return Ok(cart);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }                    
            catch (Exception ex)
            {
                //_logger.LogError(ex, "Error updating cart item {CartItemId}", cartItemId);
                return StatusCode(500, new { message = "An error occurred while updating the cart item" });
            }
        }
        [HttpDelete("items/{cartItemId}")]
        public async Task<IActionResult> RemoveCartItem(int cartItemId )
        {
            try
            {
                var userId = GetUserId();
                var sessionId = GetOrCreateSessionId();

                var cart = await _cartService.RemoveFromCartAsync(userId, sessionId, cartItemId);
                return Ok(cart);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }
        [HttpDelete]
        public async Task<IActionResult> ClearCart()
        {
            try
            {
                var userId = GetUserId();
                var sessionId = GetOrCreateSessionId();

                var cart = await _cartService.ClearCartAsync(userId, sessionId);
                return Ok(cart);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }


        /// <summary>
        /// Supose to get the user id
        /// </summary>
        /// <returns></returns>
        private int? GetUserId()
        {
            if (User?.Identity?.IsAuthenticated == true)
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int userId))
                {
                    return userId;
                }
            }

            return null;
        }

        private string? GetSessionId()
        {
            return Request.Cookies[SessionIdCookieName];
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

            // Chỉ tạo mới khi không tìm thấy
            var newSessionId = Guid.NewGuid().ToString();
            SetSessionId(newSessionId);
            return newSessionId;
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

        private void RemoveSessionId()
        {
            Response.Cookies.Delete(SessionIdCookieName);
        }


    }
}