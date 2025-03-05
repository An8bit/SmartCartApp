using Web.Models.Domain;
using Web.Models.DTO;
using Web.Repositories.Contracts;

namespace Web.Services
{
    public class UserService
    {
        private readonly IUserRepository _userRepository;
        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public Task<bool> CreateUser(AddUserRequestDto requestDto)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteUserById(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<User>?> GetAllUser()
        {
            var usersDto = await _userRepository.GetAllUser();
            if (usersDto == null)
                return null;
            return usersDto;

        }

        public Task<User?> GetOrdersByUserId(int orderId)
        {
            throw new NotImplementedException();
        }

        public async Task<User?> GetUserByEmail(string email)
        {
            if (!email.Contains("@"))
            {
                return null;
            }
            return await _userRepository.GetUserByEmail(email);
        }

        public Task<User> GetUserById(int id)
        {
           var user = _userRepository.GetUserById(id);
            return user;
        }

        public async Task<bool> UpdateUserById(int id, UpdateUserRequestDto updateUserRequestDto)
        {
            var user = await _userRepository.GetUserById(id);
            if (user == null)
            {
                return false;
            }          
            await _userRepository.UpdateUserById(id, updateUserRequestDto);
            return true;

        }
    }
}
