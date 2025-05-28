using Microsoft.EntityFrameworkCore;
using Web.Data;
using Web.Models.Domain;
using Web.Models.DTO;
using Web.Models.DTO.ProductDTOs;
using Web.Models.DTO.UserDTOs;
using Web.Repositories.Contracts;

namespace Web.Repositories.Implementations
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {


        public UserRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<UserAddress> AddUserAddressAsync(UserAddress address)
        {
            await _context.UserAddresses.AddAsync(address);
            await _context.SaveChangesAsync();
            return address;
        }

        public Task DeleteUserAddressAsync(int addressId)
        {
            throw new NotImplementedException();
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == email);
            return user!;
        }

        public Task<User> GetByIdWithDetailsAsync(int userId)
        {
            throw new NotImplementedException();
        }

        public Task<PagedResultDto<User>> GetFilteredUsersAsync(UserFilterDto filter)
        {
            throw new NotImplementedException();
        }

        public async Task<decimal> GetTotalSpendingAsync(int userId)
        {
            return await _context.Orders
                .Where(o => o.UserId == userId)
                .SumAsync(o => o.TotalAmount);
        }

        public async Task<UserAddress?> GetUserAddressByIdAsync(int addressId)
        {
            return await _context.UserAddresses.FirstOrDefaultAsync(a => a.AddressId == addressId);
        }

        public async Task<IEnumerable<UserAddress>> GetUserAddressesAsync(int userId)
        {
            return await _context.UserAddresses.Where(a => a.UserId == userId).ToListAsync();
        }

        public async Task<bool> IsEmailExistsAsync(string email, int? excludeUserId = null)
        {
            if (excludeUserId.HasValue)
                return await _context.Users.AnyAsync(u => u.Email == email && u.UserId != excludeUserId);
            return await _context.Users.AnyAsync(u => u.Email == email);
        }

        public async Task<User?> LoginAsync(UserLoginDto userLogin)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == userLogin.Email && x.PasswordHash == userLogin.Password);
            return user!;
        }

        public async Task<bool> RegisterAsync(User userRegister)
        {
            var user = await GetByEmailAsync(userRegister.Email);
            if (user != null)
                return false;
            await AddAsync(userRegister);
            return true;
        }

        public Task<bool> SetDefaultAddressAsync(int userId, int addressId)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateUserAddressAsync(UserAddress address)
        {
             _context.UserAddresses.Update(address);
            await _context.SaveChangesAsync();
        }
    }
}
