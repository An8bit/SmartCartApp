using System.ComponentModel.DataAnnotations;
using Web.Models.DTO.UserAddressDTOs;

namespace Web.Models.DTO.OrderDTOs
{
    public class OrderDto
    {
        public int OrderId { get; set; }
        public int UserId { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public required string Status { get; set; }
        public required string PaymentMethod { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public required UserAddressDto ShippingAddress { get; set; }
        public required ICollection<OrderItemDto> OrderItems { get; set; }
    }



    public class CreateOrderDto
    {
        [Required]
        public int ShippingAddressId { get; set; }

        [Required]
        [StringLength(50)]
        public string PaymentMethod { get; set; }

        // Optional - only needed if cart items are selectively chosen
        public List<int>? CartItemIds { get; set; }

        // Optional notes for the order
        [StringLength(500)]
        public string? OrderNotes { get; set; }
    }
    public class OrderItemDto
    {
        public int OrderItemId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductImage { get; set; }
        public int? ProductVariantId { get; set; }
        public string? VariantInfo { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal TotalPrice => Quantity * Price;
    }
    public class OrderStatusUpdateDto
    {
        [Required]
        public string Action { get; set; } // "Process", "Ship", "Deliver", "Cancel", "Refund"

        [StringLength(500)]
        public string? Note { get; set; }
    }
    public class OrderHistoryDto
    {
        public int HistoryId { get; set; }
        public string OldStatus { get; set; }
        public string NewStatus { get; set; }
        public DateTime ChangedAt { get; set; }
        public string ChangedBy { get; set; } // Name of the user who made the change
        public string? Note { get; set; }
    }
    public class OrderStatisticsDto
    {
        public int TotalOrders { get; set; }
        public decimal TotalRevenue { get; set; }
        public int PendingOrders { get; set; }
        public int ProcessingOrders { get; set; }
        public int ShippingOrders { get; set; }
        public int DeliveredOrders { get; set; }
        public int CanceledOrders { get; set; }
        public int RefundedOrders { get; set; }
    }
}
