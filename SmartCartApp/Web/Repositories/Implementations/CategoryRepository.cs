using Microsoft.EntityFrameworkCore;
using System.Collections;
using Web.Data;
using Web.Models.Domain;
using Web.Repositories.Contracts;

namespace Web.Repositories.Implementations
{
    public class CategoryRepository : BaseRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(ApplicationDbContext context) : base(context)
        {
        }

        public override async Task<Category> GetByIdAsync(int id)
        {

            var result = await _context.Categories
                .Include(c => c.Products)
                .FirstOrDefaultAsync(c => c.CategoryId == id);
            if (result == null)
            {
                throw new KeyNotFoundException($"Category with id {id} not found.");

            }
            return result;
        }
        public IEnumerator<Category> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
   
}
