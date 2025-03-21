using Microsoft.EntityFrameworkCore;
using Web.Models.DTO.ProductDTOs;
using Web.Data;
using Web.Models.Domain;
using Web.Repositories.Contracts;
namespace Web.Repositories.Implementations
{
    public class ProductRepository : BaseRepository<Product>, IProductRepository
    {
        public ProductRepository(ApplicationDbContext context) : base(context)
        {
        }

        public override async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await _context.Products
                .Include(p => p.Category)  
                .Include(p => p.Variants)
                .Include(p => p.Reviews)
                .ToListAsync();
        }

        public override async Task<Product> GetByIdAsync(int id)
        {
            var result = await _context.Products
                .Include(p => p.Category)
                .Include(p => p.Variants)
                .FirstOrDefaultAsync(p => p.ProductId == id);
            if (result == null)
            {
                throw new KeyNotFoundException($"Product with id {id} not found.");
            }
            return result;
        }

        public async Task<IEnumerable<Product>> GetByCategoryAsync(int categoryId)
        {
            var result = await _context.Products
                .Include(p => p.Category)
                .Include(p => p.Variants)
                .Where(p => p.CategoryId == categoryId)
                .ToListAsync();
            return result;
        }

        public async Task<PagedResultDto<Product>> GetFilteredProductsAsync(ProductFilterOptions options)
        {
            var query = _context.Products
                .Include(p => p.Category)
                .Include(p => p.Variants)
                .Include(p => p.Reviews)
                .AsQueryable();

            // Apply filters
            if (options.CategoryId.HasValue)
            {
                query = query.Where(p => p.CategoryId == options.CategoryId.Value);
            }

            if (options.MinPrice.HasValue)
            {
                query = query.Where(p => p.Price >= options.MinPrice.Value);
            }

            if (options.MaxPrice.HasValue)
            {
                query = query.Where(p => p.Price <= options.MaxPrice.Value);
            }

            if (!string.IsNullOrEmpty(options.Color))
            {
                query = query.Where(p => p.Variants.Any(a =>
                    a.Color.ToLower() == "color" &&
                    a.Color.ToLower() == options.Color.ToLower()));
            }

            if (!string.IsNullOrEmpty(options.Size))
            {
                query = query.Where(p => p.Variants.Any(a =>
                    a.Size.ToLower() == "size" &&
                    a.Size.ToLower() == options.Size.ToLower()));
            }

            var totalCount = await query.CountAsync();

            var products = await query
                .Skip((options.Page - 1) * options.PageSize)
                .Take(options.PageSize)
                .ToListAsync();

            return new PagedResultDto<Product>
            {
                Items = products,
                TotalCount = totalCount,
                Page = options.Page,
                PageSize = options.PageSize
            };
        }
    }
}
