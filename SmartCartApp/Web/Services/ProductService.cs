using AutoMapper;
using Web.Models.Domain;
using Web.Models.DTO.ProductDTOs;
using Web.Repositories.Contracts;
using Web.Repositories.Implementations;
using Web.Repositories.Interfaces.Service;

namespace Web.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IDiscountRepository _discountRepository;
        private readonly IMapper _mapper;
        public ProductService(IProductRepository productRepository, IMapper mapper, IDiscountRepository dis)
        {
            _productRepository = productRepository;
            _mapper = mapper;
            _discountRepository = dis;
        }

        public async Task<ProductDto> CreateProductAsync(CreateProductDto productDto)
        {
            var product = _mapper.Map<Product>(productDto);
            product.CreatedAt = DateTime.UtcNow;
            product.UpdatedAt = DateTime.UtcNow;
            if (productDto.Variants != null && productDto.Variants.Any())
            {
                product.Variants = _mapper.Map<List<ProductVariant>>(productDto.Variants);
            }
            var createdProduct = await _productRepository.AddAsync(product);
            return _mapper.Map<ProductDto>(createdProduct);
        }

        public  async Task DeleteProductAsync(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null)
                throw new KeyNotFoundException($"Product with ID {id} not found");

            await _productRepository.DeleteAsync(id);
        }

      

        public async Task<IEnumerable<ProductDto>> GetAllProductsAsync()
        {
            var products = await _productRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<ProductDto>>(products);
        }
        //chưa code
        public async Task<PagedResultDto<ProductDto>> GetFilteredProductsAsync(ProductFilterOptions options)
        {
            var products = await _productRepository.GetFilteredProductsAsync(options);
            if (products == null)
            {
                throw new KeyNotFoundException($"Products with the given filter options not found.");
            }

            var productDtos = _mapper.Map<IEnumerable<ProductDto>>(products.Items);
            return new PagedResultDto<ProductDto>
            {
                Items = productDtos.ToList(),
                TotalCount = products.TotalCount,
                Page = products.Page,
                PageSize = products.PageSize
            };
        }

        public async Task<ProductDto> GetProductByIdAsync(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null)
                throw new KeyNotFoundException($"Product with ID {id} not found");

            return _mapper.Map<ProductDto>(product);
        }

        public async Task<IEnumerable<ProductDto>> GetProductsByCategoryAsync(int categoryId)
        {
            var products = await _productRepository.GetByCategoryAsync(categoryId);
            return _mapper.Map<IEnumerable<ProductDto>>(products);
        }

        //chưa code
        public Task<PagedResultDto<ProductDto>> SearchProductsAsync(string keyword, int page, int pageSize)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateProductAsync(int id, UpdateProductDto productDto)
        {
            var existingProduct = await _productRepository.GetByIdAsync(id);
            if (existingProduct == null)
                throw new KeyNotFoundException($"Product with ID {id} not found");
            if (productDto.Variants != null && productDto.Variants.Any())
            {
                existingProduct.Variants = _mapper.Map<List<ProductVariant>>(productDto.Variants);
            }
            _mapper.Map(productDto, existingProduct);
            existingProduct.UpdatedAt = DateTime.UtcNow;

            await _productRepository.UpdateAsync(existingProduct);
        }
        public async Task<IEnumerable<ProductWithDiscountDTO>> GetAllDiscountedProductsAsync()
        {
            var now = DateTime.UtcNow;

            // Chuyển đổi sang múi giờ Việt Nam (+7)
            var vietnamTimeZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time"); // Hoặc "Asia/Ho_Chi_Minh"
            var vietnamNow = TimeZoneInfo.ConvertTimeFromUtc(now, vietnamTimeZone);

            // Lấy tất cả giảm giá đang hoạt động
            var activeDiscounts = await _discountRepository.GetActiveDiscountsAsync();

            // Lọc các giảm giá đang hoạt động theo thời gian Việt Nam
            var filteredDiscounts = activeDiscounts
                .Where(d => d.IsActive && d.StartDate <= vietnamNow && d.EndDate >= vietnamNow)
                .GroupBy(d => d.ProductId)
                .Select(g => g.OrderByDescending(d => d.DiscountPercentage).First()); // Lấy giảm giá cao nhất cho mỗi sản phẩm

            var result = new List<ProductWithDiscountDTO>();

            foreach (var discount in filteredDiscounts)
            {
                var product = await _productRepository.GetByIdAsync(discount.ProductId);
                if (product != null)
                {
                    decimal originalPrice = product.Price;
                    decimal discountAmount = originalPrice * (discount.DiscountPercentage / 100m);
                    decimal discountedPrice = Math.Round(originalPrice - discountAmount, 2);

                    result.Add(new ProductWithDiscountDTO
                    {
                        ProductId = product.ProductId,
                        ProductName = product.Name,
                        Description = product.Description,
                        ImageUrl = product.ImageUrl,
                        OriginalPrice = originalPrice,
                        DiscountPercentage = discount.DiscountPercentage,
                        DiscountedPrice = discountedPrice,
                        DiscountStartDate = discount.StartDate,
                        DiscountEndDate = discount.EndDate
                    });
                }
            }

            return result;
        }
    }
}
