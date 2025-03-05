using Microsoft.EntityFrameworkCore;
using Web.Data;
using Web.Models.Domain;
using Web.Models.DTO;
using Web.Repositories.Contracts;

namespace Web.Repositories.Implementations
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> CreateUser(AddUserRequestDto requestDto)
        {

            if (await _context.Users.AnyAsync(u => u.Email == requestDto.Email))
            {
                throw new Exception("Email đã tồn tại.");
            }


            var user = new User
            {
                FullName = requestDto.FullName,
                Email = requestDto.Email,
                PasswordHash = requestDto.Password,
                Phone = requestDto.Phone,
                Role = requestDto.Role,
                CreatedAt = DateTime.UtcNow
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return true;

        }

        public async Task<bool> DeleteUserById(int id)
        {

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return false;
            }
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<User>> GetAllUser()
        {
            return await _context.Users
        .Include(u => u.UserAddresses)
        .Include(u => u.Orders)
        .Include(u => u.ProductReviews)
        .ToListAsync();
        }

        public Task<User?> GetOrdersByUserId(int orderId)
        {
            return _context.Users.Include(u => u.Orders).FirstOrDefaultAsync(u => u.UserId == orderId);
        }

        public async Task<User?> GetUserByEmail(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
           

        }

        public async Task<User> GetUserById(int id)
        {
            return await _context.Users.
                 Include(u => u.UserAddresses)
                 .Include(u => u.Orders)
                 .Include(u => u.ProductReviews)
                 .FirstAsync(u => u.UserId == id);
        }

        public async Task<bool> UpdateUserById(int id, UpdateUserRequestDto updateUserRequestDto)
        {
            var userInDb = await _context.Users.FindAsync(id);
            if (userInDb == null)
            {
                return false;
            }
            //convert dto to domain
            userInDb.FullName = updateUserRequestDto.FullName;
            userInDb.Email = updateUserRequestDto.Email;
            userInDb.Phone = updateUserRequestDto.Phone;
            userInDb.Role = updateUserRequestDto.Role;
            userInDb.UpdatedAt = DateTime.UtcNow;

            _context.Update(userInDb);
            await _context.SaveChangesAsync();
            return true;


        }
    }
}
