using AutoMapper;
using Web.Models.Domain;
using Web.Models.DTO.CategoryDTOs;


namespace Web.MappingProfile
{
    public class CategoryMappingProfile : Profile
    {
        public CategoryMappingProfile()
        {
            // Category → CategoryDto (Full details with navigation properties)
            CreateMap<Category, CategoryDto>()
                .ForMember(dest => dest.ParentCategoryName, opt =>
                    opt.MapFrom(src => src.ParentCategory != null ? src.ParentCategory.CategoryName : null))
                .ForMember(dest => dest.ProductCount, opt =>
                    opt.MapFrom(src => src.Products != null ? src.Products.Count(p => !p.IsDeleted) : 0))
                .ForMember(dest => dest.SubCategories, opt =>
                    opt.MapFrom(src => src.SubCategories));

            // Category → CategoryBasicDto (Simplified version for lists/dropdowns)
            CreateMap<Category, CategoryBasicDto>()
                .ForMember(dest => dest.ProductCount, opt =>
                    opt.MapFrom(src => src.Products != null ? src.Products.Count(p => !p.IsDeleted) : 0));

            // CategoryCreateDto → Category
            CreateMap<CategoryCreateDto, Category>()
                .ForMember(dest => dest.Products, opt => opt.Ignore())
                .ForMember(dest => dest.SubCategories, opt => opt.Ignore())
                .ForMember(dest => dest.ParentCategory, opt => opt.Ignore());

            // CategoryUpdateDto → Category (with conditional mapping for partial updates)
            CreateMap<CategoryUpdateDto, Category>()
                .ForMember(dest => dest.CategoryName, opt =>
                    opt.Condition(src => !string.IsNullOrEmpty(src.CategoryName)))
                .ForMember(dest => dest.Description, opt =>
                    opt.Condition(src => src.Description != null))
                .ForMember(dest => dest.ParentCategoryId, opt =>
                    opt.Condition(src => src.ParentCategoryId != null))
                .ForMember(dest => dest.Products, opt => opt.Ignore())
                .ForMember(dest => dest.SubCategories, opt => opt.Ignore())
                .ForMember(dest => dest.ParentCategory, opt => opt.Ignore());
        }
    }
}