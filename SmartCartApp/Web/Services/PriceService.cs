using AutoMapper;
using Web.Models.Domain;
using Web.Models.DTO.ProductDTOs;
using Web.Repositories.Contracts;

namespace Web.Services
{
    /// <summary>
    /// Service implementation for price calculation that handles product pricing with discounts
    /// </summary>
    public class PriceService : IPriceService
    {
        private readonly IProductRepository _productRepository;
        private readonly IDiscountRepository _discountRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public PriceService(
            IProductRepository productRepository,
            IDiscountRepository discountRepository,
            IUserRepository userRepository,
            IMapper mapper)
        {
            _productRepository = productRepository;
            _discountRepository = discountRepository;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Calculate price information for a single product including any applicable discounts
        /// </summary>
        public async Task<ProductPriceInfoViewModel> CalculateProductPriceAsync(int productId, int? userId = null)
        {
            var product = await _productRepository.GetByIdAsync(productId);
            if (product == null)
            {
                throw new ArgumentException($"Product with ID {productId} not found");
            }

            var discountPercentage = await GetHighestDiscountPercentageAsync(productId, userId);
            
            var finalPrice = CalculateFinalPrice(product.Price, discountPercentage);

            return new ProductPriceInfoViewModel
            {
                ProductId = product.ProductId,
                Name = product.Name,
                OriginalPrice = product.Price,
                FinalPrice = finalPrice,
                DiscountPercentage = discountPercentage,
                IsOnSale = discountPercentage > 0
            };
        }

        /// <summary>
        /// Calculate price information for multiple products
        /// </summary>
        public async Task<List<ProductPriceInfoViewModel>> CalculateProductPricesAsync(List<int> productIds, int? userId = null)
        {
            var results = new List<ProductPriceInfoViewModel>();

            foreach (var productId in productIds)
            {
                try
                {
                    var priceInfo = await CalculateProductPriceAsync(productId, userId);
                    results.Add(priceInfo);
                }
                catch (ArgumentException)
                {
                    // Skip products that don't exist
                    continue;
                }
            }

            return results;
        }

        /// <summary>
        /// Get all products with active discounts
        /// </summary>
        public async Task<List<ProductPriceInfoViewModel>> GetProductsOnSaleAsync(int? userId = null)
        {
            var activeDiscounts = await _discountRepository.GetActiveDiscountsAsync();
            var productIds = activeDiscounts.Select(d => d.ProductId).Distinct().ToList();
            
            return await CalculateProductPricesAsync(productIds, userId);
        }

        /// <summary>
        /// Calculate the final price after applying discounts
        /// </summary>
        public decimal CalculateFinalPrice(decimal originalPrice, decimal discountPercentage)
        {
            if (discountPercentage <= 0)
                return originalPrice;

            var discountAmount = originalPrice * (discountPercentage / 100);
            var finalPrice = originalPrice - discountAmount;

            // Ensure price doesn't go below 0
            return Math.Max(finalPrice, 0);
        }

        /// <summary>
        /// Check if a product has any active discounts
        /// </summary>
        public async Task<bool> HasActiveDiscountAsync(int productId)
        {
            var discountPercentage = await GetHighestDiscountPercentageAsync(productId);
            return discountPercentage > 0;
        }

        /// <summary>
        /// Get the highest applicable discount percentage for a product
        /// </summary>
        public async Task<decimal> GetHighestDiscountPercentageAsync(int productId, int? userId = null)
        {
            try
            {
                // 1. Lấy tất cả khuyến mãi cho sản phẩm
                var productDiscounts = await _discountRepository.GetDiscountsByProductIdAsync(productId);

                if (productDiscounts?.Any() != true)
                {
                    Console.WriteLine($"No discounts found for productId: {productId}");
                    return 0;
                }

                // 2. Lấy thời gian hiện tại theo múi giờ Việt Nam
                var vietnamTimeZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
                var nowVietnam = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, vietnamTimeZone);

                // Hoặc nếu dữ liệu trong DB là UTC, thì dùng UTC
                var now = DateTime.UtcNow;            
                var discountsList = productDiscounts.ToList();             

                

                // 4. Lọc discount đang hoạt động  
                var activeDiscounts = discountsList
                    .Where(d => d.IsActive &&
                               now >= d.StartDate &&
                               now <= d.EndDate)
                    .ToList();
              

                if (!activeDiscounts.Any())
                {
                    return 0;
                }

                // 5. Lấy discount cao nhất
                var highestPercentage = activeDiscounts.Max(d => d.DiscountPercentage);             

                return highestPercentage;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetHighestDiscountPercentageAsync: {ex.Message}");
                return 0;
            }
        }

        #region Private Helper Methods

        /// <summary>
        /// Validates that a product exists and is not deleted
        /// </summary>
        private async Task<Product> ValidateProductExistsAsync(int productId)
        {
            var product = await _productRepository.GetByIdAsync(productId);
            
            if (product == null || product.IsDeleted)
            {
                throw new ArgumentException($"Product with ID {productId} not found or has been deleted");
            }

            return product;
        }

        #endregion
    }
}
