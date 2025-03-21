using SmartCartApp.Core.DTOs;

namespace Web.Repositories.Interfaces.Service
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync();
        Task<CategoryDto> GetCategoryByIdAsync(int id);
        Task<CategoryDto> CreateCategoryAsync(CreateCategoryDto categoryDto);
        Task UpdateCategoryAsync(int id, CreateCategoryDto categoryDto);
        Task DeleteCategoryAsync(int id);
    }
}
