using Web.Models.Domain;
using Web.Models.DTO;
using Web.Models.DTO.ProductDTOs;
using Web.Models.DTO.UserDTOs;
using Web.Repositories.Interfaces;

namespace Web.Repositories.Contracts
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User?> GetByEmailAsync(string email);

        Task<bool> IsEmailExistsAsync(string email, int? excludeUserId = null);

        Task<User> GetByIdWithDetailsAsync(int userId);

        Task<PagedResultDto<User>> GetFilteredUsersAsync(UserFilterDto filter);

        Task<IEnumerable<UserAddress>> GetUserAddressesAsync(int userId);

        Task<UserAddress?> GetUserAddressByIdAsync(int addressId);

        Task<UserAddress> AddUserAddressAsync(UserAddress address);

        Task UpdateUserAddressAsync(UserAddress address);

        Task DeleteUserAddressAsync(int addressId);

        Task<bool> SetDefaultAddressAsync(int userId, int addressId);
        //regester and login
        Task<bool> RegisterAsync(User userRegister);
        Task<User?> LoginAsync(UserLoginDto userLogin);

        Task<decimal>  GetTotalSpendingAsync(int userId);
    }
}
