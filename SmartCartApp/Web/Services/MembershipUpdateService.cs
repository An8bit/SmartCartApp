//using Web.Models.DTO.UserDTOs;
//using Web.Repositories.Interfaces.IServices;
//using Web.UserStates;

//namespace Web.Services
//{
//    public class MembershipUpdateService
//    {
       
//            private Timer _timer;
//            private readonly UserStateContext _userStateContext;
//        private readonly IUserService _userService;

//        public MembershipUpdateService(IUserService userService)
//            {
//            _userService = userService;
//            _userStateContext = new UserStateContext();
//            }

//            public Task StartAsync(CancellationToken cancellationToken)
//            {
//                // Chạy dịch vụ mỗi 24 giờ
//                _timer = new Timer(UpdateMembershipTiers, null, TimeSpan.Zero, TimeSpan.FromHours(24));
//                return Task.CompletedTask;
//            }

//            private void UpdateMembershipTiers(object state)
//            {
//                // Lấy danh sách người dùng từ database
//                var users = GetAllUsers();

//                foreach (var user in users)
//                {
//                    _userStateContext.CheckAndUpdateUserTier(user);

//                    // Lưu thay đổi vào database
//                    SaveUserChanges(user);
//                }
//            }

//            private UserDtos GetAllUsers()
//            {
                
//            }
//            }

//            private void SaveUserChanges(UserMemberDto user)
//            {
//                // Thay thế bằng logic thực tế để lưu thay đổi vào database
//                Console.WriteLine($"Cập nhật người dùng {user.Id} với hạng {user.MembershipTier}");
//            }

//            public Task StopAsync(CancellationToken cancellationToken)
//            {
//                _timer?.Change(Timeout.Infinite, 0);
//                return Task.CompletedTask;
//            }

//            public void Dispose()
//            {
//                _timer?.Dispose();
//            }
//        }
//    }
