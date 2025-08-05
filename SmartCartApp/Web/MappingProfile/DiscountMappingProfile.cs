using AutoMapper;
using Web.Models.Domain;
using Web.Models.DTO;

namespace Web.MappingProfile
{
    public class DiscountMappingProfile : Profile
    {       
        public DiscountMappingProfile()
        {
            // Map Domain Model to DTO
            CreateMap<Discount, DiscountDTO>()
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.Name))
                .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.Product.ProductId));
            // Map Create DTO to Domain Model
            CreateMap<CreateDiscountDTO, Discount>()
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => true));
            // Map Update DTO to Domain Model
            CreateMap<UpdateDiscountDTO, Discount>()
                .ForMember(dest => dest.IsActive, opt => opt.Ignore());
        }
    }
}
