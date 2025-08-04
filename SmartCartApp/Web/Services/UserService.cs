using AutoMapper;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Data.SqlTypes;
using Web.Models.Domain;
using Web.Models.DTO.ProductDTOs;
using Web.Models.DTO.UserDTOs;
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
        public async Task<UserAddressDto> AddUserAddressAsync(int userId, UserAddressCreateDto addressDto)
        {
            var userAddress = _mapper.Map<UserAddress>(addressDto);
            userAddress.UserId = userId; // Set the UserId from the parameter

            var addedAddress = await _userRepository.AddUserAddressAsync(userAddress);

            return _mapper.Map<UserAddressDto>(addedAddress);
        }

        public async Task<UserDtos> AdminUpdateUserAsync(int userId, UserAdminUpdateDto updateDto)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
                throw new KeyNotFoundException($"Không tìm thấy người dùng với ID: {userId}");

            // Check if email is being changed and already exists
            if (!string.IsNullOrEmpty(updateDto.Email) &&
                updateDto.Email != user.Email &&
                await _userRepository.IsEmailExistsAsync(updateDto.Email, userId))
                throw new InvalidOperationException($"Email {updateDto.Email} đã được sử dụng");

            _mapper.Map(updateDto, user);

            await _userRepository.UpdateAsync(user);

            return _mapper.Map<UserDtos>(user);
        }

        public Task<bool> ChangePasswordAsync(int userId, ChangePasswordDto changePasswordDto)
        {
            throw new NotImplementedException();
        }

        public async Task<UserDtos> CreateUserAsync(UserCreateDto createDto)
        {
            // Check if email already exists
            if (await _userRepository.IsEmailExistsAsync(createDto.Email))
                throw new InvalidOperationException($"Email {createDto.Email} đã được sử dụng");

            // Map DTO to entity
            var newUser = _mapper.Map<User>(createDto);

            // Save user
            var createdUser = await _userRepository.AddAsync(newUser);

            // Return user DTO
            return _mapper.Map<UserDtos>(createdUser);
        }

        public Task<bool> DeleteUserAddressAsync(int addressId)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> DeleteUserAsync(int userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
                throw new KeyNotFoundException($"Không tìm thấy người dùng với ID: {userId}");
            await _userRepository.DeleteAsync(userId);
            return true;
        }

        public async Task<IEnumerable<UserDtos>> GetAllUsersAsync()
        {
            var user = await _userRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<UserDtos>>(user);
        }

       

        public Task<UserAddressDto> GetUserAddressByIdAsync(int addressId)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<UserAddressDto>> GetUserAddressesAsync(int userId)
        {
           var  userAddress = await _userRepository.GetUserAddressesAsync(userId);
            if (userAddress == null)
                throw new KeyNotFoundException($"Không tìm thấy địa chỉ của người dùng với ID: {userId}");
            return _mapper.Map<IEnumerable<UserAddressDto>>(userAddress);
        }

        public async Task<UserDtos> GetUserByEmailAsync(string email)
        {
            var user = await _userRepository.GetByEmailAsync(email);
            if (user == null)
                throw new KeyNotFoundException($"Không tìm thấy người dùng với email: {email}");
            return _mapper.Map<UserDtos>(user);
        }

        public async Task<UserDtos> GetUserByIdAsync(int userId)
        {
           var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
                 throw new KeyNotFoundException($"Product with ID {userId} not found");
            return _mapper.Map<UserDtos>(user);
        }

        public Task<UserDetailsDto> GetUserDetailsAsync(int userId)
        {
            throw new NotImplementedException();
        }

        public async Task<UserMemberDto?> GetUserMemberShip(int userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
                throw new KeyNotFoundException($"Product with ID {userId} not found");
            return _mapper.Map<UserMemberDto>(user);
        }

        public async Task<string> LoginAsync(UserLoginDto loginDto)
        {
            var checkemail = await _userRepository.GetByEmailAsync(loginDto.Email);
            if (checkemail == null)
                return "Không tìm thấy user";

            var login = await _userRepository.LoginAsync(loginDto);
            if (login == null)
                return "Password Sai";

            // Convert the object to a string representation with email included
            return $"{{\"role\":\"{login.Role}\",\"authToken\":\"{login.Email}\",\"message\":\"Đăng nhập thành công\"}}";
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

        public async Task<UserAddressDto> UpdateUserAddressAsync(int addressId, UserAddressUpdateDto addressDto)
        {
            // Get the existing address
            var existingAddress = await _userRepository.GetUserAddressByIdAsync(addressId);
            if (existingAddress == null)
                throw new KeyNotFoundException($"Không tìm thấy địa chỉ với ID: {addressId}");

            // Map the update data to the existing address
            _mapper.Map(addressDto, existingAddress);

            // Update the address in the repository
            await _userRepository.UpdateUserAddressAsync(existingAddress);

            // Return the updated address as DTO
            return _mapper.Map<UserAddressDto>(existingAddress);
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
