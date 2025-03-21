using AutoMapper;
using Web.Models.DTO.ProductDTOs;
using Web.Models.DTO.CategoryDTOs;
using Web.Repositories.Contracts;
using Web.Repositories.Interfaces.Service;
using Web.Models.Domain;

namespace Web.Services
{
    public class CategoryService : ICategoryService
    {

        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public CategoryService(ICategoryRepository categoryRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        public async Task<CategoryDto> CreateCategoryAsync(CategoryCreateDto categoryDto)
        {
           var category = _mapper.Map<Category>(categoryDto);
            var createdCategory = await _categoryRepository.AddAsync(category);
            return _mapper.Map<CategoryDto>(createdCategory);
        }

        public async Task DeleteCategoryAsync(int id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category == null)
                throw new KeyNotFoundException($"Category with ID {id} not found");
            await _categoryRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<CategoryBasicDto>> GetAllCategoriesAsync()
        {

            var categories = await _categoryRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<CategoryBasicDto>>(categories);
        }

        public async Task<CategoryDto> GetCategoryByIdAsync(int id)
        {
           var category = await _categoryRepository.GetByIdAsync(id);
            return _mapper.Map<CategoryDto>(category);
        }

        public async Task UpdateCategoryAsync(int id, CategoryUpdateDto categoryDto)
        {
            var category = await _categoryRepository.GetByIdAsync(id);

            if (category == null)
                throw new KeyNotFoundException($"Category with ID {id} not found");
            _mapper.Map(categoryDto, category);
            await _categoryRepository.UpdateAsync(category);
        }
    }
}
