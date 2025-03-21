using Web.Models.DTO;
using Web.Models.DTO.CategoryDTOs;


namespace Web.Repositories.Interfaces.Service
{
    public interface ICategoryService
    {


        
        Task<IEnumerable<CategoryBasicDto>> GetAllCategoriesAsync();
        Task<CategoryDto> GetCategoryByIdAsync(int id);
        Task<CategoryDto> CreateCategoryAsync(CategoryCreateDto categoryDto);

        Task UpdateCategoryAsync(int id, CategoryUpdateDto categoryDto);

        Task DeleteCategoryAsync(int id);


    }
}
