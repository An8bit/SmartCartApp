using Web.Models.DTO.ShoppingCartDTOs;

namespace Web.Repositories.Interfaces.IServices
{
    public interface IShoppingCartService
    {
        // Lấy thông tin giỏ hàng
        Task<ShoppingCartDto> GetCartAsync(int? userId, string sessionId);
        Task<CartSummaryDto> GetCartSummaryAsync(int? userId, string sessionId);

        // Thêm sản phẩm vào giỏ
        Task<ShoppingCartDto> AddToCartAsync(int? userId, string sessionId, AddToCartDto addToCartDto);

        // Cập nhật sản phẩm trong giỏ
        Task<ShoppingCartDto> UpdateCartItemAsync(int? userId, string sessionId, UpdateCartItemDto updateCartItemDto);

        // Xóa sản phẩm khỏi giỏ
        Task<ShoppingCartDto> RemoveFromCartAsync(int? userId, string sessionId, int cartItemId);

        // Xóa toàn bộ giỏ hàng
        Task<ShoppingCartDto> ClearCartAsync(int? userId, string sessionId);

        // Chuyển giỏ hàng từ session sang user khi đăng nhập
        Task MergeCartsAsync(string sessionId, int userId);


     


    }
}
