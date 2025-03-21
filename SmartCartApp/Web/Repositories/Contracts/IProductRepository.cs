
using Web.Models.Domain;
using Web.Models.DTO.ProductDTOs;
using Web.Repositories.Interfaces;

namespace Web.Repositories.Contracts
{
    public interface IProductRepository : IRepository<Product>
    {
        Task<IEnumerable<Product>> GetByCategoryAsync(int categoryId);
        Task<PagedResultDto<Product>> GetFilteredProductsAsync(ProductFilterOptions options);
        //Task<PagedResultDto<Product>> SearchProductsAsync(string keyword, int page, int pageSize);
    }
}
