using SmartCartApp.Core.DTOs;
using Web.Repositories.Interfaces.Service;

namespace Web.Services
{
    public class CategoryService : ICategoryService
    {

        private readonly ICategoryService _categoryService;

        public Task<CategoryDto> CreateCategoryAsync(CreateCategoryDto categoryDto)
        {
            throw new NotImplementedException();
        }

        public Task DeleteCategoryAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync()
        {
            throw new NotImplementedException();
        }

        public Task<CategoryDto> GetCategoryByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateCategoryAsync(int id, CreateCategoryDto categoryDto)
        {
            throw new NotImplementedException();
        }
    }
}
