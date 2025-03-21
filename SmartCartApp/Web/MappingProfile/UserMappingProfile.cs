using AutoMapper;
using Web.Models.Domain;
using Web.Models.DTO.UrserDTOs;
using Web.Models.DTO.UserAddressDTOs;

namespace Web.MappingProfile
{
    public class UserMappingProfile : Profile
    {
        public UserMappingProfile()
        {
            // User -> UserDto (Basic safe info)
            CreateMap<User, UserDto>();

            // User -> UserDetailsDto (With related entity counts)
            CreateMap<User, UserDetailsDto>()
                .ForMember(dest => dest.Addresses, opt =>
                    opt.MapFrom(src => src.UserAddresses))
                .ForMember(dest => dest.OrderCount, opt =>
                    opt.MapFrom(src => src.Orders != null ? src.Orders.Count : 0))
                .ForMember(dest => dest.ReviewCount, opt =>
                    opt.MapFrom(src => src.ProductReviews != null ? src.ProductReviews.Count : 0));

            // UserRegisterDto -> User
            // UserRegisterDto -> User
            CreateMap<UserRegisterDto, User>()
                .ForMember(dest => dest.PasswordHash, opt => opt.MapFrom(src => src.Password)) // Fixed missing parenthesis
                .ForMember(dest => dest.UserAddresses, opt => opt.Ignore())
                .ForMember(dest => dest.Orders, opt => opt.Ignore())
                .ForMember(dest => dest.ProductReviews, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow));

            // UserCreateDto -> User
            CreateMap<UserCreateDto, User>()
                .ForMember(dest => dest.PasswordHash, opt => opt.MapFrom(src => src.Password)) // Set in service
                .ForMember(dest => dest.UserAddresses, opt => opt.Ignore())
                .ForMember(dest => dest.Orders, opt => opt.Ignore())
                .ForMember(dest => dest.ProductReviews, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow));

            // UserUpdateDto -> User
            CreateMap<UserUpdateDto, User>()
                .ForMember(dest => dest.FullName, opt =>
                    opt.Condition(src => !string.IsNullOrEmpty(src.FullName)))
                .ForMember(dest => dest.Phone, opt =>
                    opt.Condition(src => !string.IsNullOrEmpty(src.Phone)))
                .ForMember(dest => dest.UpdatedAt, opt =>
                    opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.UserAddresses, opt => opt.Ignore())
                .ForMember(dest => dest.Orders, opt => opt.Ignore())
                .ForMember(dest => dest.ProductReviews, opt => opt.Ignore())
                .ForMember(dest => dest.Email, opt => opt.Ignore())
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
                .ForMember(dest => dest.Role, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore());

            // UserAdminUpdateDto -> User (admin can change more fields)
            CreateMap<UserAdminUpdateDto, User>()
                .ForMember(dest => dest.FullName, opt =>
                    opt.Condition(src => !string.IsNullOrEmpty(src.FullName)))
                .ForMember(dest => dest.Email, opt =>
                    opt.Condition(src => !string.IsNullOrEmpty(src.Email)))
                .ForMember(dest => dest.Phone, opt =>
                    opt.Condition(src => !string.IsNullOrEmpty(src.Phone)))
                .ForMember(dest => dest.Role, opt =>
                    opt.Condition(src => !string.IsNullOrEmpty(src.Role)))
                .ForMember(dest => dest.UpdatedAt, opt =>
                    opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.UserAddresses, opt => opt.Ignore())
                .ForMember(dest => dest.Orders, opt => opt.Ignore())
                .ForMember(dest => dest.ProductReviews, opt => opt.Ignore())
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore());

            // UserAddress -> UserAddressDto
            CreateMap<UserAddress, UserAddressDto>();

            // UserAddressCreateDto -> UserAddress
            CreateMap<UserAddressCreateDto, UserAddress>()
                .ForMember(dest => dest.User, opt => opt.Ignore());

            // UserAddressUpdateDto -> UserAddress
            CreateMap<UserAddressUpdateDto, UserAddress>()             
                .ForMember(dest => dest.AddressLine1, opt =>
                    opt.Condition(src => !string.IsNullOrEmpty(src.AddressLine)))
                .ForMember(dest => dest.City, opt =>
                    opt.Condition(src => !string.IsNullOrEmpty(src.City)))
                .ForMember(dest => dest.State, opt =>
                    opt.Condition(src => !string.IsNullOrEmpty(src.State)))
                .ForMember(dest => dest.PostalCode, opt =>
                    opt.Condition(src => !string.IsNullOrEmpty(src.PostalCode)))
                .ForMember(dest => dest.AddressLine1, opt =>
                    opt.Condition(src => !string.IsNullOrEmpty(src.AddressLine)))
                .ForMember(dest => dest.IsDefault, opt =>
                    opt.Condition(src => src.IsDefault.HasValue))
                .ForMember(dest => dest.User, opt => opt.Ignore())
                .ForMember(dest => dest.UserId, opt => opt.Ignore());
        }
    }
}