using Web.Models.Domain;

namespace Web.Models.DTO
{
    public class UserDto
    {
        public int UserId { get; set; }
        public required string FullName { get; set; }
        public required string Email { get; set; }
        public required string Phone { get; set; }
        public required string Role { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public required ICollection<UserAddressDto> UserAddresses { get; set; }
        public required ICollection<OrderDto> Orders { get; set; }
        

        public static  UserDto  ConvertToDto(User user)
        {
            return new UserDto
            {
                UserId = user.UserId,
                FullName = user.FullName,
                Email = user.Email,
                Phone = user.Phone,
                Role = user.Role,
                CreatedAt = user.CreatedAt,
                UpdatedAt = user.UpdatedAt,
                UserAddresses =  user.UserAddresses!.Select(a => new UserAddressDto
                {
                    AddressId = a.AddressId,
                    AddressLine1 = a.AddressLine1,
                    AddressLine2 = a.AddressLine2,
                    City = a.City,
                    State = a.State,
                    PostalCode = a.PostalCode,
                    IsDefault = a.IsDefault
                }).ToList(),
                Orders = user.Orders!.Select(o => new OrderDto
                {
                    UserId = o.UserId,
                    OrderId = o.OrderId,
                    OrderDate = o.OrderDate,
                    TotalAmount = o.TotalAmount,
                    Status = o.Status,
                    PaymentMethod = o.PaymentMethod!,
                    ShippingAddress= new UserAddressDto
                    {
                        AddressId = o.ShippingAddress!.AddressId,
                        AddressLine1 = o.ShippingAddress.AddressLine1,
                        AddressLine2 = o.ShippingAddress.AddressLine2,
                        City = o.ShippingAddress.City,
                        State = o.ShippingAddress.State,
                        PostalCode = o.ShippingAddress.PostalCode,
                        IsDefault = o.ShippingAddress.IsDefault
                    },
                    OrderItems = o.OrderItems!.Select(i => new OrderItemDto
                    {
                        OrderItemId = i.OrderItemId,
                        ProductId = i.ProductId,
                        Quantity = i.Quantity,
                        Price = i.Price
                    }).ToList(),
                    CreatedAt = o.CreatedAt,
                    UpdatedAt = o.UpdatedAt
                }).ToList()
               
            };
        }
        public static List<UserDto> ConvertToDtoList(List<User> users)
        {
            return users.Select(ConvertToDto).ToList();
        }
    }
}
