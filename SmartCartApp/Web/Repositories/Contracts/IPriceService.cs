using Web.Models.DTO.ProductDTOs;

namespace Web.Services
{
    /// <summary>
    /// Interface for price calculation service that handles product pricing with discounts
    /// </summary>
    public interface IPriceService
    {
        /// <summary>
        /// Calculate price information for a single product including any applicable discounts
        /// </summary>
        /// <param name="productId">The ID of the product to calculate price for</param>
        /// <param name="userId">Optional user ID for membership-based discounts</param>
        /// <returns>ProductPriceInfoViewModel with calculated pricing information</returns>
        Task<ProductPriceInfoViewModel> CalculateProductPriceAsync(int productId, int? userId = null);

        /// <summary>
        /// Calculate price information for multiple products
        /// </summary>
        /// <param name="productIds">List of product IDs to calculate prices for</param>
        /// <param name="userId">Optional user ID for membership-based discounts</param>
        /// <returns>List of ProductPriceInfoViewModel with calculated pricing information</returns>
        Task<List<ProductPriceInfoViewModel>> CalculateProductPricesAsync(List<int> productIds, int? userId = null);

        /// <summary>
        /// Get all products with active discounts
        /// </summary>
        /// <param name="userId">Optional user ID for membership-based discounts</param>
        /// <returns>List of products currently on sale</returns>
        Task<List<ProductPriceInfoViewModel>> GetProductsOnSaleAsync(int? userId = null);

        /// <summary>
        /// Calculate the final price after applying discounts
        /// </summary>
        /// <param name="originalPrice">The original price of the product</param>
        /// <param name="discountPercentage">The discount percentage to apply</param>
        /// <returns>The final discounted price</returns>
        decimal CalculateFinalPrice(decimal originalPrice, decimal discountPercentage);

        /// <summary>
        /// Check if a product has any active discounts
        /// </summary>
        /// <param name="productId">The ID of the product to check</param>
        /// <returns>True if the product has active discounts, false otherwise</returns>
        Task<bool> HasActiveDiscountAsync(int productId);

        /// <summary>
        /// Get the highest applicable discount percentage for a product
        /// </summary>
        /// <param name="productId">The ID of the product</param>
        /// <param name="userId">Optional user ID for membership-based discounts</param>
        /// <returns>The highest discount percentage applicable to the product</returns>
        Task<decimal> GetHighestDiscountPercentageAsync(int productId, int? userId = null);
    }
}
