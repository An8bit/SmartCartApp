using AutoMapper;
using Web.Models.Domain;
using Web.Models.DTO.ShoppingCartDTOs;

namespace Web.MappingProfile
{
    public class ShoppingCartMappingProfile : Profile
    {
        public ShoppingCartMappingProfile()
        {
            // Map từ ShoppingCart đến ShoppingCartDto
            CreateMap<ShoppingCart, ShoppingCartDto>()
             .ForMember(dest => dest.Items, opt =>
                 opt.MapFrom(src => src.CartItems));

            // Map từ CartItem đến CartItemDto
            CreateMap<CartItem, CartItemDto>()
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.Name))
                .ForMember(dest => dest.ProductImage, opt => opt.MapFrom(src => src.Product.ImageUrl))
                .ForMember(dest => dest.TotalPrice, opt => opt.MapFrom(src => src.Quantity * src.UnitPrice))
                .ForMember(dest => dest.VariantInfo, opt => opt.MapFrom(src => FormatVariantInfo(src.ProductVariant)));
        }

        private string FormatVariantInfo(ProductVariant variant)
        {
            if (variant == null)
                return null;

            var info = new List<string>();

            if (!string.IsNullOrEmpty(variant.Color))
                info.Add($"Màu: {variant.Color}");

            if (!string.IsNullOrEmpty(variant.Size))
                info.Add($"Kích thước: {variant.Size}");

            return string.Join(", ", info);
        }
    }
}