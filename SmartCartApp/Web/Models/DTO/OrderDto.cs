namespace Web.Models.DTO
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
}
