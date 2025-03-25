using Microsoft.EntityFrameworkCore;
using Web.Data;
using Web.Models.Domain;
using Web.Repositories.Contracts;

namespace Web.Repositories.Implementations
{
    public class OrderRepository : BaseRepository<Order>, IOrderRepository
    {
        public OrderRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<Order?> GetOrderByIdAsync(int orderId)
        {
            var order = await _context.Orders
                .Include(o => o.User)
                .Include(o => o.ShippingAddress)
                .Include(o => o.OrderItems)
                .Include(o => o.OrderHistories)
                .FirstOrDefaultAsync(o => o.OrderId == orderId);

            // If order exists, load nested relationships separately to avoid nullability issues
            if (order?.OrderItems != null)
            {
                // Load product and variant information for each order item
                foreach (var item in order.OrderItems)
                {
                    await _context.Entry(item)
                        .Reference(i => i.Product)
                        .LoadAsync();

                    if (item.ProductVariantId.HasValue)
                    {
                        await _context.Entry(item)
                            .Reference(i => i.ProductVariant)
                            .LoadAsync();
                    }
                }
            }

            return order;
        }

        public async Task<IEnumerable<Order>> GetOrdersByUserIdAsync(int userId)
        {
            return await _context.Orders
                .Include(o => o.OrderItems)
                .Include(o => o.ShippingAddress)
                .Where(o => o.UserId == userId)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();
        }

        public async Task<Order> CreateOrderAsync(Order order)
        {
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
            return order;
        }

        public async Task UpdateOrderAsync(Order order)
        {
            order.UpdatedAt = DateTime.UtcNow;
            _context.Orders.Update(order);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> DeleteOrderAsync(int orderId)
        {
            var order = await _context.Orders.FindAsync(orderId);
            if (order != null)
            {
                _context.Orders.Remove(order);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<OrderItem?> GetOrderItemByIdAsync(int orderItemId)
        {
            return await _context.OrderItems
                .Include(oi => oi.Product)
                .Include(oi => oi.ProductVariant)
                .FirstOrDefaultAsync(oi => oi.OrderItemId == orderItemId);
        }

        public async Task<IEnumerable<OrderItem>> GetOrderItemsByOrderIdAsync(int orderId)
        {
            return await _context.OrderItems
                .Include(oi => oi.Product)
                .Include(oi => oi.ProductVariant)
                .Where(oi => oi.OrderId == orderId)
                .ToListAsync();
        }

        public async Task AddOrderItemAsync(OrderItem orderItem)
        {
            _context.OrderItems.Add(orderItem);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateOrderItemAsync(OrderItem orderItem)
        {
            _context.OrderItems.Update(orderItem);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveOrderItemAsync(int orderItemId)
        {
            var orderItem = await _context.OrderItems.FindAsync(orderItemId);
            if (orderItem != null)
            {
                _context.OrderItems.Remove(orderItem);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<OrderHistory>> GetOrderHistoryAsync(int orderId)
        {
            return await _context.OrderHistories
                .Where(h => h.OrderId == orderId)
                .OrderByDescending(h => h.ChangedAt)
                .ToListAsync();
        }

        public async Task AddOrderHistoryAsync(OrderHistory history)
        {
            _context.OrderHistories.Add(history);
            await _context.SaveChangesAsync();
        }

        public async Task<int> GetTotalOrderCountAsync()
        {
            return await _context.Orders.CountAsync();
        }

        public async Task<decimal> GetTotalRevenueAsync(DateTime? startDate = null, DateTime? endDate = null)
        {
            var query = _context.Orders.AsQueryable();

            if (startDate.HasValue)
                query = query.Where(o => o.OrderDate >= startDate.Value);

            if (endDate.HasValue)
                query = query.Where(o => o.OrderDate <= endDate.Value);

            return await query
                .Where(o => o.Status != "Canceled" && o.Status != "Refunded")
                .SumAsync(o => o.TotalAmount);
        }

        public async Task<IEnumerable<Order>> GetAllOrdersAsync(int page = 1, int pageSize = 10)
        {
            return await _context.Orders
                .Include(o => o.User)
                .Include(o => o.OrderItems)
                .OrderByDescending(o => o.OrderDate)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<int> GetOrderCountByStatusAsync(string status)
        {
            return await _context.Orders
                .Where(o => o.Status == status)
                .CountAsync();
        }
    }
}
