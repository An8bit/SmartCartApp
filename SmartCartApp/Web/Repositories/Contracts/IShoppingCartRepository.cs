using Web.Models.Domain;

namespace Web.Repositories.Contracts
{
    public interface IShoppingCartRepository
    {
        // Giỏ hàng
        Task<ShoppingCart>? GetCartByUserIdAsync(int userId);
        Task<ShoppingCart?> GetCartBySessionIdAsync(string sessionId);
        Task<ShoppingCart> CreateCartAsync(ShoppingCart cart);
        Task UpdateCartAsync(ShoppingCart cart);
        Task DeleteCartAsync(int cartId);

        // Mục giỏ hàng
        Task<CartItem?> GetCartItemByIdAsync(int cartItemId);
        Task<CartItem?> GetCartItemAsync(int cartId, int productId, int? productVariantId);
        Task<CartItem> AddCartItemAsync(CartItem cartItem);
        Task UpdateCartItemAsync(CartItem cartItem);
        Task DeleteCartItemAsync(int cartItemId);
        Task ClearCartItemsAsync(int cartId);

        // Hợp nhất giỏ hàng
        Task MergeCartsAsync(string sessionId, int userId);
    }
}
