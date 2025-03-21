using System.ComponentModel.DataAnnotations;

namespace Web.Models.DTO.UserAddressDTOs
{
    public class UserAddressDto
    {
        public int AddressId { get; set; }

        public int UserId { get; set; }

        public string FullName { get; set; }

        public string Phone { get; set; }

        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string City { get; set; }

        public string State { get; set; }

        public string PostalCode { get; set; }
  

        public bool IsDefault { get; set; }
    }

    public class UserAddressCreateDto
    {
        [Required(ErrorMessage = "Họ tên là bắt buộc")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Họ tên phải từ 3 đến 100 ký tự")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Số điện thoại là bắt buộc")]
        [Phone(ErrorMessage = "Số điện thoại không hợp lệ")]
        [StringLength(15, MinimumLength = 10, ErrorMessage = "Số điện thoại không hợp lệ")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Địa chỉ là bắt buộc")]
        [StringLength(255, MinimumLength = 5, ErrorMessage = "Địa chỉ phải từ 5 đến 255 ký tự")]
        public string AddressLine1 { get; set; }
       
        [StringLength(255, MinimumLength = 5, ErrorMessage = "Địa chỉ phải từ 5 đến 255 ký tự")]
        public string AddressLine2 { get; set; }

        [Required(ErrorMessage = "Thành phố là bắt buộc")]
        [StringLength(100, ErrorMessage = "Thành phố không được vượt quá 100 ký tự")]
        public string City { get; set; }

        [StringLength(100, ErrorMessage = "Tỉnh/Bang không được vượt quá 100 ký tự")]
        public string State { get; set; }

        [StringLength(20, ErrorMessage = "Mã bưu điện không được vượt quá 20 ký tự")]
        public string PostalCode { get; set; }

      

        public bool IsDefault { get; set; }
    }

    public class UserAddressUpdateDto
    {
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Họ tên phải từ 3 đến 100 ký tự")]
        public string FullName { get; set; }

        [Phone(ErrorMessage = "Số điện thoại không hợp lệ")]
        [StringLength(15, MinimumLength = 10, ErrorMessage = "Số điện thoại không hợp lệ")]
        public string Phone { get; set; }

        [StringLength(255, MinimumLength = 5, ErrorMessage = "Địa chỉ phải từ 5 đến 255 ký tự")]
        public string AddressLine { get; set; }

        [StringLength(100, ErrorMessage = "Thành phố không được vượt quá 100 ký tự")]
        public string City { get; set; }

        [StringLength(100, ErrorMessage = "Tỉnh/Bang không được vượt quá 100 ký tự")]
        public string State { get; set; }

        [StringLength(20, ErrorMessage = "Mã bưu điện không được vượt quá 20 ký tự")]
        public string PostalCode { get; set; }

      

        public bool? IsDefault { get; set; }
    }
}