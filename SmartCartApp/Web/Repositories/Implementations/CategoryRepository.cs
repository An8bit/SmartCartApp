using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using Web.Data;
using Web.Models.Domain;
using Web.Models.DTO.CategoryDTOs;
using Web.Models.DTO.ProductDTOs;

using Web.Repositories.Contracts;

namespace Web.Repositories.Implementations
{
    public class CategoryRepository : BaseRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<Category> GetByIdWithRelatedDataAsync(int id)
        {
            var category = await _context.Categories
                
                
               
                .FirstOrDefaultAsync(c => c.CategoryId == id);
            if (category == null)
            {
                throw new KeyNotFoundException($"Category with id {id} not found.");
            }
            return category;
        }

       
    }
}
