using Microsoft.EntityFrameworkCore;
using Web.Data;
using Web.Models.Domain;
using Web.Repositories.Contracts;
namespace Web.Repositories.Implementations
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _context;

        public ProductRepository(ApplicationDbContext context)
        {
            _context = context;
        }


        public async Task<List<Product>> GetAllProduct()
        {
            var products = await _context.Products.Include(p => p.Category).ToListAsync();
            return await _context.Products
               
                .ToListAsync();
        }

        public Task<Product> GetProductById(int id)
        {
            throw new NotImplementedException();
        }
    }
}
