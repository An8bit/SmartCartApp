using Web.Models.Domain;
using Web.Models.DTO.ProductDTOs;
using Web.Models.DTO.UserDTOs;
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
        Task<UserDtos> GetUserByIdAsync(int userId);
        Task<UserDtos> GetUserByEmailAsync(string email);

        Task UpdateUserAsync(int userId, UserUpdateDto updateDto);

        // Admin methods
        Task<IEnumerable<UserDtos> >GetAllUsersAsync();

        Task<UserDtos> CreateUserAsync(UserCreateDto createDto);

        Task<UserDtos> AdminUpdateUserAsync(int userId, UserAdminUpdateDto updateDto);

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