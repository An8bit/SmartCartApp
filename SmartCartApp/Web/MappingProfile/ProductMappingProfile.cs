using AutoMapper;

using System.Linq;
using Web.Models.Domain;
using Web.Models.DTO.CategoryDTOs;
using Web.Models.DTO.ProductDTOs;

namespace Web.Mapping
{
    public class ProductMappingProfile : Profile
    {
        public ProductMappingProfile()
        {
            // Product mappings
            CreateMap<Product, ProductDto>()
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category != null ? src.Category.CategoryName : null))
                .ForMember(dest => dest.Variants, opt => opt.MapFrom(src => src.Variants))

                .ForMember(dest => dest.AverageRating, opt =>
                    opt.MapFrom(src => src.Reviews != null && src.Reviews.Any() ?
                        src.Reviews.Average(r => r.Rating) : (double?)null))
                .ForMember(dest => dest.ReviewsCount, opt =>
                    opt.MapFrom(src => src.Reviews != null ? src.Reviews.Count : 0))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))
                .ForMember(dest => dest.OriginalPrice, opt => opt.MapFrom(src => src.Price))
                .ForMember(dest => dest.DiscountedPrice, opt => opt.Ignore())
                .ForMember(dest => dest.DiscountPercentage, opt => opt.Ignore())
                .ForMember(dest => dest.MembershipTierName, opt => opt.Ignore());
            ;

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

        }


    }
}
