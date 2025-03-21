using Web.Models.Domain;
using Web.Repositories.Contracts;

namespace Web.Services
{
    public class ProductService
    {
        private readonly IProductRepository _productRepository;
        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<List<Product>?> GetAllProduct()
        {
            var productsDto = await _productRepository.GetAllProduct();
            if (productsDto == null)
                return null;
            return productsDto;
        }

    }
}
