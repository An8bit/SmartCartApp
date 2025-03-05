using Web.Models.Domain;
using Web.Models.DTO;

namespace Web.Repositories.Contracts
{
    public interface IUserRepository
    {
        Task<List<User>> GetAllUser();
        Task<User> GetUserById(int id);
        Task<bool> UpdateUserById(int id, UpdateUserRequestDto updateUserRequestDto);
        Task<bool> DeleteUserById(int id);
        Task<bool> CreateUser(AddUserRequestDto requestDto);
        Task<User?> GetUserByEmail(string email);
        Task<User?> GetOrdersByUserId(int orderId);
    }
}
