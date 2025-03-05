using Web.Models.Domain;

namespace Web.Repositories.Contracts
{
    public interface IProductRepository
    {
        Task<List<Product>> GetAllProduct();
        Task<Product> GetProductById(int id);

    }
}
