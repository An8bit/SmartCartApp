using AutoMapper;
using Web.Models.Domain;
using Web.Models.DTO.ShoppingCartDTOs;
using Web.Repositories.Contracts;
using Web.Repositories.Interfaces.IServices;
using Web.Services;

namespace Web.Services
{
    public class ShoppingCartService : IShoppingCartService
    {
        private readonly IShoppingCartRepository _cartRepository;
        private readonly IProductRepository _productRepository;
        private readonly IPriceService _priceService;
        private readonly IMapper _mapper;
        
        public ShoppingCartService(
           IShoppingCartRepository cartRepository,
           IProductRepository productRepository,
           IPriceService priceService,
           IMapper mapper)
        {
            _cartRepository = cartRepository;
            _productRepository = productRepository;
            _priceService = priceService;
            _mapper = mapper;
        }
        public async Task<ShoppingCartDto> AddToCartAsync(int? userId, string sessionId, AddToCartDto addToCartDto)
        {
            if (userId == null && string.IsNullOrEmpty(sessionId))
            {
                throw new ArgumentException("Phải cung cấp userId hoặc sessionId");
            }

            // Kiểm tra sản phẩm tồn tại
            var product = await _productRepository.GetByIdAsync(addToCartDto.ProductId);
            if (product == null)
            {
                throw new KeyNotFoundException($"Không tìm thấy sản phẩm với ID: {addToCartDto.ProductId}");
            }

            //  Kiểm tra biến thể sản phẩm nếu có
            ProductVariant? variant = null;
            if (addToCartDto.ProductVariantId.HasValue)
            {
                variant = product.Variants?.FirstOrDefault(v => v.VariantId == addToCartDto.ProductVariantId);
                if (variant == null)
                {
                    throw new KeyNotFoundException($"Không tìm thấy biến thể sản phẩm với ID: {addToCartDto.ProductVariantId.Value}");
                }
            }

            // Kiểm tra tồn kho
            //int inStock = variant != null ? variant.StockQuantity : product.ProductId;
            //if (inStock < addToCartDto.Quantity)
            //{
            //    throw new InvalidOperationException($"Sản phẩm không đủ số lượng. Hiện chỉ còn {inStock} sản phẩm.");
            //}

            // Lấy hoặc tạo giỏ hàng
            ShoppingCart cart;

            if (userId.HasValue)
            {
                cart = await _cartRepository.GetCartByUserIdAsync(userId.Value);
                if (cart == null)
                {
                    cart = new ShoppingCart
                    {
                        UserId = userId.Value,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow,
                        CartItems = new List<CartItem>()
                    };
                    cart = await _cartRepository.CreateCartAsync(cart);
                }
            }
            else
            {
                cart = await _cartRepository.GetCartBySessionIdAsync(sessionId) ?? new ShoppingCart
                {
                    SessionId = sessionId,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    CartItems = new List<CartItem>()
                };
                if (cart.CartId == 0)
                {
                    cart = await _cartRepository.CreateCartAsync(cart);
                }
            }

            // Kiểm tra xem sản phẩm đã có trong giỏ chưa
            var existingItem = await _cartRepository.GetCartItemAsync(
                cart.CartId,
                addToCartDto.ProductId,
                addToCartDto.ProductVariantId);

            if (existingItem != null)
            {
                // Nếu đã có, cập nhật số lượng và recalculate price
                existingItem.Quantity += addToCartDto.Quantity;
                
                // Recalculate price with current discounts
                var priceInfo = await _priceService.CalculateProductPriceAsync(addToCartDto.ProductId, userId);
                existingItem.UnitPrice = priceInfo.FinalPrice;
                
                existingItem.UpdatedAt = DateTime.UtcNow;
                await _cartRepository.UpdateCartItemAsync(existingItem);
            }
            else
            {
                // Nếu chưa có, thêm mới
                // Use PriceService to calculate the current price with discounts
                var priceInfo = await _priceService.CalculateProductPriceAsync(addToCartDto.ProductId, userId);
                decimal price = priceInfo.FinalPrice;

                var newItem = new CartItem
                {
                    CartId = cart.CartId,
                    ProductId = addToCartDto.ProductId,
                    ProductVariantId = addToCartDto.ProductVariantId,
                    Quantity = addToCartDto.Quantity,
                    UnitPrice = price,
                    AddedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };
                await _cartRepository.AddCartItemAsync(newItem);
            }

            // Cập nhật thời gian cập nhật giỏ hàng
            cart.UpdatedAt = DateTime.UtcNow;
            await _cartRepository.UpdateCartAsync(cart);

            // Lấy giỏ hàng cập nhật và trả về
            var updatedCart = await GetCartEntityAsync(userId, sessionId);
            return _mapper.Map<ShoppingCartDto>(updatedCart);
        }

        public async Task<ShoppingCartDto> ClearCartAsync(int? userId, string sessionId)
        {
            // Lấy giỏ hàng
            var cart = await GetCartEntityAsync(userId, sessionId);
            if (cart == null)
            {
                throw new KeyNotFoundException("Không tìm thấy giỏ hàng");
            }

            // Xóa tất cả items
            await _cartRepository.ClearCartItemsAsync(cart.CartId);

            // Cập nhật thời gian cập nhật giỏ hàng
            cart.UpdatedAt = DateTime.UtcNow;
            await _cartRepository.UpdateCartAsync(cart);

            // Lấy giỏ hàng cập nhật và trả về
            return await GetCartAsync(userId, sessionId);
        }

        public async Task<ShoppingCartDto> GetCartAsync(int? userId, string sessionId)
        {
            var cart = await GetCartEntityAsync(userId, sessionId);
            if (cart == null)
            {
                // Nếu không có giỏ hàng, trả về giỏ trống
                return new ShoppingCartDto
                {
                    UserId = userId,
                    SessionId = sessionId,
                    Items = new List<CartItemDto>(),
                    TotalAmount = 0,
                    TotalItems = 0,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };
            }

            var cartDto = _mapper.Map<ShoppingCartDto>(cart);
            // Thêm dòng này để đảm bảo Items được ánh xạ đúng
            cartDto.Items = cart.CartItems != null
                ? _mapper.Map<List<CartItemDto>>(cart.CartItems)
                : new List<CartItemDto>();
            cartDto.TotalAmount = cart.TotalAmount;
            cartDto.TotalItems = cart.TotalItems;
            
            return cartDto;
        }

       

        public async Task<CartSummaryDto> GetCartSummaryAsync(int? userId, string sessionId)
        {
            var cart = await GetCartEntityAsync(userId, sessionId);
            if (cart == null)
            {
                return new CartSummaryDto { TotalItems = 0, TotalAmount = 0 };
            }

            return new CartSummaryDto
            {
                TotalItems = cart.TotalItems,
                TotalAmount = cart.TotalAmount
            };
        }

      

        public Task MergeCartsAsync(string sessionId, int userId)
        {
            throw new NotImplementedException();
        }

        public async Task<ShoppingCartDto> RemoveFromCartAsync(int? userId, string sessionId, int cartItemId)
        {
            // Lấy giỏ hàng
            var cart = await GetCartEntityAsync(userId, sessionId);
            if (cart == null)
            {
                throw new KeyNotFoundException("Không tìm thấy giỏ hàng");
            }

            // Lấy item trong giỏ hàng
            var cartItem = await _cartRepository.GetCartItemByIdAsync(cartItemId);
            if (cartItem == null)
            {
                throw new KeyNotFoundException($"Không tìm thấy sản phẩm với ID: {cartItemId} trong giỏ hàng");
            }

            // Kiểm tra cartItem có thuộc giỏ hàng hiện tại không
            if (cartItem.CartId != cart.CartId)
            {
                throw new UnauthorizedAccessException("Bạn không có quyền xóa mục này");
            }

            // Xóa item
            await _cartRepository.DeleteCartItemAsync(cartItemId);

            // Cập nhật thời gian cập nhật giỏ hàng
            cart.UpdatedAt = DateTime.UtcNow;
            await _cartRepository.UpdateCartAsync(cart);

            // Lấy giỏ hàng cập nhật và trả về
            return await GetCartAsync(userId, sessionId);
        }

        public async Task<ShoppingCartDto> UpdateCartItemAsync(int? userId, string sessionId, UpdateCartItemDto updateCartItemDto)
        {
            if (userId == null && string.IsNullOrEmpty(sessionId))
            {
                throw new ArgumentException("Phải cung cấp userId hoặc sessionId");
            }

            // Lấy giỏ hàng (domain model, not DTO)
            ShoppingCart cart;
            if (userId.HasValue)
            {
                cart = await _cartRepository.GetCartByUserIdAsync(userId.Value);
            }
            else
            {
                cart = await _cartRepository.GetCartBySessionIdAsync(sessionId) ?? throw new InvalidOperationException("Cart not found for the given sessionId");
            }

            if (cart == null)
            {
                throw new KeyNotFoundException("Không tìm thấy giỏ hàng");
            }

            // Lấy item trong giỏ hàng
            var cartItem = await _cartRepository.GetCartItemByIdAsync(updateCartItemDto.CartItemId);
            if (cartItem == null)
            {
                throw new KeyNotFoundException($"Không tìm thấy sản phẩm với ID: {updateCartItemDto.CartItemId} trong giỏ hàng");
            }

            // Kiểm tra cartItem có thuộc giỏ hàng hiện tại không
            if (cartItem.CartId != cart.CartId)
            {
                throw new UnauthorizedAccessException("Bạn không có quyền cập nhật mục này");
            }

            // Kiểm tra tồn kho
            //int inStock;
            //if (cartItem.ProductVariantId.HasValue && cartItem.ProductVariant != null)
            //{
            //    inStock = cartItem.ProductVariant.InStock;
            //}
            //else
            //{
            //    inStock = cartItem.Product.InStock;
            //}

            //if (inStock < updateCartItemDto.Quantity)
            //{
            //    throw new InvalidOperationException($"Sản phẩm không đủ số lượng. Hiện chỉ còn {inStock} sản phẩm.");
            //}

            // Cập nhật số lượng
            cartItem.Quantity = updateCartItemDto.Quantity;
            cartItem.UpdatedAt = DateTime.UtcNow;
            await _cartRepository.UpdateCartItemAsync(cartItem);

            // Cập nhật thời gian cập nhật giỏ hàng
            cart.UpdatedAt = DateTime.UtcNow;
            await _cartRepository.UpdateCartAsync(cart);

            // Lấy giỏ hàng cập nhật và trả về
            var updatedCartDto = await GetCartAsync(userId, sessionId);
            return updatedCartDto;
        }
        // Helper method để lấy entity giỏ hàng
        private async Task<ShoppingCart?> GetCartEntityAsync(int? userId, string sessionId)
        {
            ShoppingCart? cart = null;

            if (userId.HasValue)
            {
                cart = await _cartRepository.GetCartByUserIdAsync(userId.Value);
            }
            else if (!string.IsNullOrEmpty(sessionId))
            {
                cart = await _cartRepository.GetCartBySessionIdAsync(sessionId);
            }

            return cart;
        }
    }
}
