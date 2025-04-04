using Web.Models.Domain;
using Web.Models.DTO.ProductDTOs;
using Web.Models.DTO.UrserDTOs;
using Web.Models.DTO.UserAddressDTOs;

namespace Web.Repositories.Interfaces.IServices
{
    public interface IUserService
    {
        // Auth related methods
        Task<string> RegisterAsync(UserRegisterDto registerDto);

        Task<string> LoginAsync(UserLoginDto loginDto);

        Task<bool> ChangePasswordAsync(int userId, ChangePasswordDto changePasswordDto);

        Task<bool> RequestPasswordResetAsync(ForgotPasswordDto forgotPasswordDto);

        Task<bool> ResetPasswordAsync(ResetPasswordDto resetPasswordDto);

        // User profile methods
        Task<UserDto> GetUserByIdAsync(int userId);
        Task<UserDto> GetUserByEmailAsync(string email);

        Task UpdateUserAsync(int userId, UserUpdateDto updateDto);

        // Admin methods
        Task<IEnumerable<UserDto> >GetAllUsersAsync();

        Task<UserDto> CreateUserAsync(UserCreateDto createDto);

        Task<UserDto> AdminUpdateUserAsync(int userId, UserAdminUpdateDto updateDto);

        Task<bool> DeleteUserAsync(int userId);

        // Address methods
        Task<IEnumerable<UserAddressDto>> GetUserAddressesAsync(int userId);

        Task<UserAddressDto> GetUserAddressByIdAsync(int addressId);

        Task<UserAddressDto> AddUserAddressAsync(int userId, UserAddressCreateDto addressDto);

        Task<UserAddressDto> UpdateUserAddressAsync(int addressId, UserAddressUpdateDto addressDto);

        Task<bool> DeleteUserAddressAsync(int addressId);

        Task<bool> SetDefaultAddressAsync(int userId, int addressId);
    }
}