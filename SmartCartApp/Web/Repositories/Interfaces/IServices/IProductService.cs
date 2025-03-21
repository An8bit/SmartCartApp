using SmartCartApp.Core.DTOs;

namespace Web.Repositories.Interfaces.Service
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDto>> GetAllProductsAsync();
        Task<ProductDto> GetProductByIdAsync(int id);
        Task<IEnumerable<ProductDto>> GetProductsByCategoryAsync(int categoryId);
        Task<PagedResultDto<ProductDto>> GetFilteredProductsAsync(ProductFilterOptions options);
        Task<PagedResultDto<ProductDto>> SearchProductsAsync(string keyword, int page, int pageSize);
        Task<ProductDto> CreateProductAsync(CreateProductDto productDto);
        Task UpdateProductAsync(int id, UpdateProductDto productDto);
        Task DeleteProductAsync(int id);
    }
}
