using System.ComponentModel.DataAnnotations;

namespace Web.Models.DTO.ShoppingCartDTOs
{
    public class ShoppingCartDto
    {
        public int CartId { get; set; }
        public string SessionId { get; set; }
        public int? UserId { get; set; }
        public List<CartItemDto> Items { get; set; } = new List<CartItemDto>();
        public decimal TotalAmount { get; set; }
        public int TotalItems { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    // DTO cho mỗi mục trong giỏ hàng
    public class CartItemDto
    {
        public int CartItemId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductImage { get; set; }
        public int? ProductVariantId { get; set; }
        public string VariantInfo { get; set; }  // Ví dụ: "Màu: Đỏ, Kích thước: XL"
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime AddedAt { get; set; }
    }

    // DTO để thêm sản phẩm vào giỏ hàng
    public class AddToCartDto
    {
        [Required(ErrorMessage = "Cần có ID sản phẩm")]
        public int ProductId { get; set; }

        public int? ProductVariantId { get; set; }

        [Required(ErrorMessage = "Cần có số lượng")]
        [Range(1, 100, ErrorMessage = "Số lượng phải từ 1 đến 100")]
        public int Quantity { get; set; } = 1;
    }

    // DTO để cập nhật mục trong giỏ hàng
    public class UpdateCartItemDto
    {
        [Required(ErrorMessage = "Cần có ID mục giỏ hàng")]
        public int CartItemId { get; set; }

        [Required(ErrorMessage = "Cần có số lượng")]
        [Range(1, 100, ErrorMessage = "Số lượng phải từ 1 đến 100")]
        public int Quantity { get; set; }
    }

    // DTO cho tóm tắt giỏ hàng (hiển thị ở header)
    public class CartSummaryDto
    {
        public int TotalItems { get; set; }
        public decimal TotalAmount { get; set; }
    }
}