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
    

        public async Task<ShoppingCart> GetCartByUserIdAsync(int userId)
        {
            var cart = await _context.ShoppingCarts
                .Include(c => c.CartItems)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null)
                throw new KeyNotFoundException($"Cart for user with id {userId} not found.");

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

        public Task MergeCartsAsync(string sessionId, int userId)
        {
            throw new NotImplementedException();
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
