using AutoMapper;
using Web.Models.Domain;
using Web.Models.DTO.OrderDTOs;

namespace Web.MappingProfile
{
    public class OrderMappingProfile : Profile
    {
        public OrderMappingProfile()
        {
            // Map Order to OrderDto
            CreateMap<Order, OrderDto>()
                .ForMember(dest => dest.ShippingAddress, opt =>
                    opt.MapFrom(src => src.ShippingAddress))
                .ForMember(dest => dest.OrderItems, opt =>
                    opt.MapFrom(src => src.OrderItems));

            // Map OrderItem to OrderItemDto
            CreateMap<OrderItem, OrderItemDto>()
                .ForMember(dest => dest.ProductName, opt =>
                    opt.MapFrom(src => src.Product.Name))
                .ForMember(dest => dest.ProductImage, opt =>
                    opt.MapFrom(src => src.Product.ImageUrl))
                .ForMember(dest => dest.VariantInfo, opt =>
                    opt.MapFrom(src => FormatVariantInfo(src.ProductVariant)));

            // Map OrderHistory to OrderHistoryDto
            CreateMap<OrderHistory, OrderHistoryDto>()
                .ForMember(dest => dest.ChangedBy, opt =>
                    opt.MapFrom(src => GetUserName(src.ChangedBy)));
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

        // In a real application, you would look up the user name from a repository
        private string GetUserName(int userId)
        {
            // This is just a placeholder - you'd inject your user repository here
            return $"User {userId}";
        }
    }
}