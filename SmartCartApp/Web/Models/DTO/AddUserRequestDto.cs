using System.ComponentModel.DataAnnotations;

namespace Web.Models.DTO
{
    public class AddUserRequestDto
    {
        [Required(ErrorMessage = "Họ và Tên không được để trống")]
        public required string FullName { get; set; }

        [Required(ErrorMessage = "Email không được để trống")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        public required string Email { get; set; }

        [Required(ErrorMessage = "Mật khẩu không được để trống")]
        [MinLength(6, ErrorMessage = "Mật khẩu phải có ít nhất 6 ký tự")]
        public required string Password { get; set; }

        [Required(ErrorMessage = "Số điện thoại không được để trống")]
        public required string Phone { get; set; }

        public string Role { get; set; } = "Customer";
    }
}
