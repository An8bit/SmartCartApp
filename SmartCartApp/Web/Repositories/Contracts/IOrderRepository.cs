using Web.Models.Domain;

namespace Web.Repositories.Contracts
{
    public interface IOrderRepository
    {// Order management
        Task<Order?> GetOrderByIdAsync(int orderId);
        Task<IEnumerable<Order>> GetOrdersByUserIdAsync(int userId);
        Task<Order> CreateOrderAsync(Order order);
        Task UpdateOrderAsync(Order order);
        Task<bool> DeleteOrderAsync(int orderId);

        // Order items
        Task<OrderItem?> GetOrderItemByIdAsync(int orderItemId);
        Task<IEnumerable<OrderItem>> GetOrderItemsByOrderIdAsync(int orderId);
        Task AddOrderItemAsync(OrderItem orderItem);
        Task UpdateOrderItemAsync(OrderItem orderItem);
        Task RemoveOrderItemAsync(int orderItemId);

        // Order history
        Task<IEnumerable<OrderHistory>> GetOrderHistoryAsync(int orderId);
        Task AddOrderHistoryAsync(OrderHistory history);

        // Statistics
        Task<int> GetTotalOrderCountAsync();
        Task<decimal> GetTotalRevenueAsync(DateTime? startDate = null, DateTime? endDate = null);
        // Add these methods to the existing interface
        Task<IEnumerable<Order>> GetAllOrdersAsync(int page = 1, int pageSize = 10);
        Task<int> GetOrderCountByStatusAsync(string status);
        Task<decimal> GetTotalAmountPaidByUserAsync(int userId); // Tính tổng tiền đã thanh toán của user
    }
}