using Web.Models.DTO;

namespace Web.Repositories.Interfaces.IServices
{
    public interface IDiscountService
    {
        Task<IEnumerable<DiscountDTO>> GetAllDiscountsAsync();
        Task<DiscountDTO> GetDiscountByIdAsync(int id);
        Task<IEnumerable<DiscountDTO>> GetActiveDiscountsAsync();
        Task<IEnumerable<DiscountDTO>> GetDiscountsByProductIdAsync(int productId);
        Task<DiscountDTO> CreateDiscountAsync(CreateDiscountDTO discountDto);
        Task<DiscountDTO> UpdateDiscountAsync(int id, UpdateDiscountDTO discountDto);
        Task<bool> DeleteDiscountAsync(int id);
        Task<decimal> CalculateDiscountedPriceAsync(int productId, decimal originalPrice);
    }
}