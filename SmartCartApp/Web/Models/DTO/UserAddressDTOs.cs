using System.ComponentModel.DataAnnotations;

namespace Web.Models.DTO.UserAddressDTOs
{
    public class UserAddressDto
    {
        public int AddressId { get; set; }
        public int UserId { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
        public bool IsDefault { get; set; }
    }

    public class UserAddressCreateDto
    {
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
        [StringLength(255, MinimumLength = 5, ErrorMessage = "Địa chỉ phải từ 5 đến 255 ký tự")]
        public string AddressLine1 { get; set; }

        [StringLength(255, MinimumLength = 5, ErrorMessage = "Địa chỉ phải từ 5 đến 255 ký tự")]
        public string AddressLine2 { get; set; }

        [StringLength(100, ErrorMessage = "Thành phố không được vượt quá 100 ký tự")]
        public string City { get; set; }

        [StringLength(100, ErrorMessage = "Tỉnh/Bang không được vượt quá 100 ký tự")]
        public string State { get; set; }

        [StringLength(20, ErrorMessage = "Mã bưu điện không được vượt quá 20 ký tự")]
        public string PostalCode { get; set; }

        public bool? IsDefault { get; set; }
    }
}