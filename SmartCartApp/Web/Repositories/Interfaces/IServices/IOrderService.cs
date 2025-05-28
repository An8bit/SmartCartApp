using Web.Models.DTO.OrderDTOs;

namespace Web.Repositories.Interfaces.IServices
{
    public interface IOrderService
    {// Order retrieval
        Task<OrderDto> GetOrderByIdAsync(int orderId, int userId);
        Task<IEnumerable<OrderDto>> GetUserOrdersAsync(int userId);

        // Order creation
        Task<OrderDto> CreateOrderFromCartAsync(int userId, CreateOrderDto createOrderDto);

        // Order state management
        Task<OrderDto> UpdateOrderStatusAsync(int orderId, int userId, OrderStatusUpdateDto updateDto);

        // Order history
        Task<IEnumerable<OrderHistoryDto>> GetOrderHistoryAsync(int orderId, int userId);

        // Admin operations
        Task<IEnumerable<OrderDto>> GetAllOrdersAsync(int page = 1, int pageSize = 10);
        Task<OrderStatisticsDto> GetOrderStatisticsAsync(DateTime? startDate = null, DateTime? endDate = null);

        Task<decimal> GetTotalAmount(int orderId);
    }
}