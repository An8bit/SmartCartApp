using AutoMapper;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Web.Models.Domain;
using Web.Models.DTO.ProductDTOs;
using Web.Models.DTO.UrserDTOs;
using Web.Models.DTO.UserAddressDTOs;
using Web.Repositories.Contracts;
using Web.Repositories.Interfaces.IServices;

namespace Web.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        public UserService(IUserRepository userRepository , IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }
        public Task<UserAddressDto> AddUserAddressAsync(int userId, UserAddressCreateDto addressDto)
        {
            throw new NotImplementedException();
        }

        public Task<UserDto> AdminUpdateUserAsync(int userId, UserAdminUpdateDto updateDto)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ChangePasswordAsync(int userId, ChangePasswordDto changePasswordDto)
        {
            throw new NotImplementedException();
        }

        public Task<UserDto> CreateUserAsync(UserCreateDto createDto)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteUserAddressAsync(int addressId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteUserAsync(int userId)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
        {
            var user = await _userRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<UserDto>>(user);
        }

       

        public Task<UserAddressDto> GetUserAddressByIdAsync(int addressId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<UserAddressDto>> GetUserAddressesAsync(int userId)
        {
            throw new NotImplementedException();
        }

        public async Task<UserDto> GetUserByIdAsync(int userId)
        {
           var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
                 throw new KeyNotFoundException($"Product with ID {userId} not found");
            return _mapper.Map<UserDto>(user);
        }

        public Task<UserDetailsDto> GetUserDetailsAsync(int userId)
        {
            throw new NotImplementedException();
        }

        public async Task<string> LoginAsync(UserLoginDto loginDto)
        {
           var checkemail = await _userRepository.GetByEmailAsync(loginDto.Email);
            if (checkemail == null)
                return "Không tìm thấy user";
            var login = await _userRepository.LoginAsync(loginDto);
            if (login == false)
                return "Password Sai";
            return "Login successful";

        }

        public async Task<string> RegisterAsync(UserRegisterDto registerDto)
        {
           var user =  _mapper.Map<User>(registerDto);
           var checkemail = await _userRepository.GetByEmailAsync(registerDto.Email);
            if (checkemail != null)
                return "Email đã được đăng ký rồi";
            await _userRepository.RegisterAsync(user);
            return "Đăng ký thành công";
        }

        public Task<bool> RequestPasswordResetAsync(ForgotPasswordDto forgotPasswordDto)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ResetPasswordAsync(ResetPasswordDto resetPasswordDto)
        {
            throw new NotImplementedException();
        }

        public Task<bool> SetDefaultAddressAsync(int userId, int addressId)
        {
            throw new NotImplementedException();
        }

        public Task<UserAddressDto> UpdateUserAddressAsync(int addressId, UserAddressUpdateDto addressDto)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateUserAsync(int userId, UserUpdateDto updateDto)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
                throw new KeyNotFoundException($"Product with ID {userId} not found");
            _mapper.Map(updateDto, user);
            await _userRepository.UpdateAsync(user);
        }
    }
}
