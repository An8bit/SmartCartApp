using AutoMapper;
using SmartCartApp.Core.DTOs;
using System.Linq;
using Web.Models.Domain;

namespace Web.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Product mappings
            CreateMap<Product, ProductDto>()
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category != null ? src.Category.CategoryName : null))
                .ForMember(dest => dest.Variants, opt => opt.MapFrom(src => src.Variants))
                .ForMember(dest => dest.AverageRating, opt =>
                    opt.MapFrom(src => src.Reviews != null && src.Reviews.Any() ?
                        src.Reviews.Average(r => r.Rating) : (double?)null))
                .ForMember(dest => dest.ReviewsCount, opt =>
                    opt.MapFrom(src => src.Reviews != null ? src.Reviews.Count : 0));

            // ProductVariant mapping
            CreateMap<ProductVariant, ProductVariantDto>();
            CreateMap<CreateProductVariantInlineDto, ProductVariant>();
            CreateMap<UpdateProductVariantDto, ProductVariant>();

            // Create mapping
            CreateMap<CreateProductDto, Product>()
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.Variants, opt => opt.Ignore());

            // Update mapping
            CreateMap<UpdateProductDto, Product>()
                .ForMember(dest => dest.Name, opt => opt.Condition(src => !string.IsNullOrEmpty(src.Name)))
                .ForMember(dest => dest.Description, opt => opt.Condition(src => !string.IsNullOrEmpty(src.Description)))
                .ForMember(dest => dest.Price, opt => opt.Condition(src => src.Price.HasValue))
                .ForMember(dest => dest.CategoryId, opt => opt.Condition(src => src.CategoryId.HasValue))
                .ForMember(dest => dest.ImageUrl, opt => opt.Condition(src => !string.IsNullOrEmpty(src.ImageUrl)))
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.UtcNow));

            // Category mappings
            CreateMap<Category, CategoryDto>()
                .ForMember(dest => dest.ProductCount, opt =>
                    opt.MapFrom(src => src.Products != null ?
                        src.Products.Count(p => !p.IsDeleted) : 0));

            CreateMap<CreateCategoryDto, Category>();
        }


    }
}
