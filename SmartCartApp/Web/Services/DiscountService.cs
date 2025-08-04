using Web.Models.Domain;
using Web.Models.DTO;
using Web.Repositories.Contracts;
using Web.Repositories.Interfaces.IServices;

namespace Web.Services
{
    public class DiscountService : IDiscountService
    {
        private readonly IDiscountRepository _discountRepository;

        public DiscountService(IDiscountRepository discountRepository)
        {
            _discountRepository = discountRepository;
        }

        public async Task<IEnumerable<DiscountDTO>> GetAllDiscountsAsync()
        {
            var discounts = await _discountRepository.GetAllAsync();
            return discounts.Select(MapToDTO);
        }

        public async Task<DiscountDTO> GetDiscountByIdAsync(int id)
        {
            var discount = await _discountRepository.GetByIdAsync(id);
            return discount != null ? MapToDTO(discount) : null;
        }

        public async Task<IEnumerable<DiscountDTO>> GetActiveDiscountsAsync()
        {
            var discounts = await _discountRepository.GetActiveDiscountsAsync();
            return discounts.Select(MapToDTO);
        }

        public async Task<IEnumerable<DiscountDTO>> GetDiscountsByProductIdAsync(int productId)
        {
            var discounts = await _discountRepository.GetDiscountsByProductIdAsync(productId);
            return discounts.Select(MapToDTO);
        }

        public async Task<DiscountDTO> CreateDiscountAsync(CreateDiscountDTO discountDto)
        {
            ValidateDiscount(discountDto.DiscountPercentage, discountDto.StartDate, discountDto.EndDate);

            var discount = new Discount
            {
                ProductId = discountDto.ProductId,
                DiscountPercentage = discountDto.DiscountPercentage,
                StartDate = discountDto.StartDate,
                EndDate = discountDto.EndDate,
                IsActive = true
            };

            var createdDiscount = await _discountRepository.CreateDiscountAsync(discount);
            return MapToDTO(createdDiscount);
        }

        public async Task<DiscountDTO> UpdateDiscountAsync(int id, UpdateDiscountDTO discountDto)
        {
            ValidateDiscount(discountDto.DiscountPercentage, discountDto.StartDate, discountDto.EndDate);

            var discount = await _discountRepository.GetByIdAsync(id);
            if (discount == null)
                return null;

            discount.DiscountPercentage = discountDto.DiscountPercentage;
            discount.StartDate = discountDto.StartDate;
            discount.EndDate = discountDto.EndDate;
            discount.IsActive = discountDto.IsActive;

            var updatedDiscount = await _discountRepository.UpdateDiscountAsync(discount);
            return MapToDTO(updatedDiscount);
        }

        public async Task<bool> DeleteDiscountAsync(int id)
        {
            return await _discountRepository.DeleteDiscountAsync(id);
        }

        public async Task<decimal> CalculateDiscountedPriceAsync(int productId, decimal originalPrice)
        {
            var now = DateTime.UtcNow;

            // Chuyển đổi sang múi giờ Việt Nam (+7)
            var vietnamTimeZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time"); // Hoặc "Asia/Ho_Chi_Minh"
            var vietnamNow = TimeZoneInfo.ConvertTimeFromUtc(now, vietnamTimeZone);
            var productDiscounts = await _discountRepository.GetDiscountsByProductIdAsync(productId);
            // Get the active discount with the highest percentage
            var activeDiscount = productDiscounts
        .Where(d => d.IsActive && d.StartDate <= vietnamNow && d.EndDate >= vietnamNow)
        .OrderByDescending(d => d.DiscountPercentage)
        .FirstOrDefault();

            if (activeDiscount == null)
                return originalPrice;

            var discountAmount = originalPrice * (activeDiscount.DiscountPercentage / 100m);
            return Math.Round(originalPrice - discountAmount, 2);
        }

        private void ValidateDiscount(decimal percentage, DateTime startDate, DateTime endDate)
        {
            if (percentage <= 0 || percentage > 100)
                throw new ArgumentException("Discount percentage must be between 0 and 100");

            if (startDate >= endDate)
                throw new ArgumentException("End date must be after start date");
        }

        private DiscountDTO MapToDTO(Discount discount)
        {
            return new DiscountDTO
            {
                Id = discount.Id,
                ProductId = discount.ProductId,
                ProductName = discount.Product?.Name,
                DiscountPercentage = discount.DiscountPercentage,
                StartDate = discount.StartDate,
                EndDate = discount.EndDate,
                IsActive = discount.IsActive,
                CreatedAt = discount.CreatedAt,
                UpdatedAt = discount.UpdatedAt
            };
        }
    }
}