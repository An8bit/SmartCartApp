using Microsoft.EntityFrameworkCore;
using Web.Data;
using Web.Models.Domain;
using Web.Repositories.Contracts;

namespace Web.Repositories.Implementations
{
    public class ShoppingCartRepository : BaseRepository<ShoppingCart>, IShoppingCartRepository
    {
        public ShoppingCartRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<CartItem> AddCartItemAsync(CartItem cartItem)
        {
            try
            {
                _context.CartItems.Add(cartItem);
                await _context.SaveChangesAsync();
                return cartItem;
            }
            catch (Exception ex)
            {
                // Log the full exception details (including inner exception)
                Console.WriteLine($"Error: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner Exception: {ex.InnerException.Message}");
                }
                throw;
            }
        }


        public async Task ClearCartItemsAsync(int cartId)
        {
            var cartItems = await _context.CartItems
                 .Where(i => i.CartId == cartId)
                 .ToListAsync();

            _context.CartItems.RemoveRange(cartItems);
            await _context.SaveChangesAsync();
        }

        public async Task<ShoppingCart> CreateCartAsync(ShoppingCart cart)
        {
            _context.ShoppingCarts.Add(cart);
            await _context.SaveChangesAsync();
            return cart;
        }

        public async Task DeleteCartAsync(int cartId)
        {
            var cart = await _context.ShoppingCarts.FindAsync(cartId);
            if (cart != null)
            {
                _context.ShoppingCarts.Remove(cart);
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteCartItemAsync(int cartItemId)
        {
            var cartItem = await _context.CartItems.FindAsync(cartItemId);
            if (cartItem != null)
            {
                _context.CartItems.Remove(cartItem);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<ShoppingCart?> GetCartBySessionIdAsync(string sessionId)
        {
            return await _context.ShoppingCarts
                 .Include(c => c.CartItems!)
                     .ThenInclude(i => i.Product)
                 .Include(c => c.CartItems!)
                     .ThenInclude(i => i.ProductVariant)
                 .FirstOrDefaultAsync(c => c.SessionId == sessionId);
        }
    

        public async Task<ShoppingCart>? GetCartByUserIdAsync(int userId)
        {
            var cart = await _context.ShoppingCarts
                .Include(c => c.CartItems)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null)
                return null;

            // Load related entities if cart exists and has items
            if (cart.CartItems != null && cart.CartItems.Any())
            {
                // Load product data for each cart item
                foreach (var item in cart.CartItems)
                {
                    await _context.Entry(item)
                        .Reference(i => i.Product)
                        .LoadAsync();

                    if (item.ProductVariantId.HasValue)
                    {
                        await _context.Entry(item)
                            .Reference(i => i.ProductVariant)
                            .LoadAsync();
                    }
                }
            }

            return cart;
        }

        public async Task<CartItem?> GetCartItemAsync(int cartId, int productId, int? productVariantId)
        {
            return await _context.CartItems
               .FirstOrDefaultAsync(i => i.CartId == cartId &&
                                        i.ProductId == productId &&
                                        i.ProductVariantId == productVariantId);
        }

        public async Task<CartItem?> GetCartItemByIdAsync(int cartItemId)
        {
            return await _context.CartItems
               .Include(i => i.Product)
               .Include(i => i.ProductVariant)
               .FirstOrDefaultAsync(i => i.CartItemId == cartItemId);
        }

        public async Task MergeCartsAsync(string sessionId, int userId)
        {
            if (string.IsNullOrEmpty(sessionId) || userId <= 0)
            {
                throw new ArgumentException("SessionId hoặc UserId không hợp lệ.");
            }

            var sessionCart = await _context.ShoppingCarts
                .Include(c => c.CartItems)
                .FirstOrDefaultAsync(c => c.SessionId == sessionId);

            var userCart = await _context.ShoppingCarts
                .Include(c => c.CartItems)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (sessionCart == null && userCart == null)
            {
                // Không có giỏ hàng nào tồn tại
                return;
            }

            if (sessionCart != null)
            {
                if (userCart == null)
                {
                    // Nếu người dùng chưa có giỏ hàng, gán giỏ hàng tạm thời cho người dùng
                    sessionCart.UserId = userId;
                }
                else
                {
                    // Kết hợp các mục trong giỏ hàng
                    foreach (var item in sessionCart.CartItems ?? new List<CartItem>())
                    {
                        var existingItem = userCart.CartItems!
                            .FirstOrDefault(ci => ci.ProductId == item.ProductId);

                        if (existingItem != null)
                        {
                            existingItem.Quantity += item.Quantity;
                        }
                        else
                        {
                            userCart.CartItems!.Add(item);
                        }
                    }

                    // Xóa giỏ hàng tạm thời
                    _context.ShoppingCarts.Remove(sessionCart);
                }

                await _context.SaveChangesAsync();
            }
        }

        public async Task UpdateCartAsync(ShoppingCart cart)
        {
            cart.UpdatedAt = DateTime.UtcNow;
            _context.ShoppingCarts.Update(cart);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateCartItemAsync(CartItem cartItem)
        {
            cartItem.UpdatedAt = DateTime.UtcNow;
            _context.CartItems.Update(cartItem);
            await _context.SaveChangesAsync();
        }
    }
}
