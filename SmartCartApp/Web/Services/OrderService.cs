using AutoMapper;
using Web.Models.Domain;
using Web.Models.DTO.OrderDTOs;
using Web.OrderStates;
using Web.Repositories.Contracts;
using Web.Repositories.Interfaces.IServices;

namespace Web.Services
{
    public class OrderService : IOrderService
    {

        private readonly IOrderRepository _orderRepository;
        private readonly IShoppingCartRepository _cartRepository;
        private readonly IProductRepository _productRepository;
        private readonly IUserRepository _userRepository;
        private readonly OrderStateContext _orderStateContext;
        private readonly IMapper _mapper;
        public OrderService(IOrderRepository orderRepository, IShoppingCartRepository cartRepository,IProductRepository productRepository,IUserRepository userRepository,IMapper mapper)
        {
            _orderRepository = orderRepository;
            _cartRepository = cartRepository;
            _productRepository = productRepository;
            _userRepository = userRepository;
            _orderStateContext = new OrderStateContext();
            _mapper = mapper;
        }
        public async Task<OrderDto> CreateOrderFromCartAsync(int userId, CreateOrderDto createOrderDto)
        {
            // Get user's cart
            var cart = await _cartRepository.GetCartByUserIdAsync(userId);
            if (cart == null || !cart.CartItems.Any())
            {
                throw new InvalidOperationException("Your cart is empty");
            }

            // Validate shipping address
            var userAddresses = await _userRepository.GetUserAddressesAsync(userId);
            var shippingAddress = userAddresses.FirstOrDefault(a => a.AddressId == createOrderDto.ShippingAddressId);
            if (shippingAddress == null)
            {
                throw new InvalidOperationException("Invalid shipping address");
            }

            // Get cart items to include in the order
            var cartItems = cart.CartItems;
            if (createOrderDto.CartItemIds != null && createOrderDto.CartItemIds.Any())
            {
                cartItems = cartItems.Where(ci => createOrderDto.CartItemIds.Contains(ci.CartItemId)).ToList();

                if (!cartItems.Any())
                {
                    throw new InvalidOperationException("No valid items selected for ordering");
                }
            }

            // Validate products stock
            foreach (var item in cartItems)
            {
                var product = await _productRepository.GetByIdAsync(item.ProductId);
                if (product == null)
                {
                    throw new InvalidOperationException($"Product with ID {item.ProductId} not found");
                }

                int stockQuantity;
                if (item.ProductVariantId.HasValue)
                {
                    var variant = product.Variants.FirstOrDefault(v => v.ProductId == item.ProductId);
                    if (variant == null)
                    {
                        throw new InvalidOperationException($"Product variant with ID {item.ProductVariantId} not found");
                    }
                    stockQuantity = variant.StockQuantity;
                }
                else
                {
                    stockQuantity = product.ProductId;
                }

                if (stockQuantity < item.Quantity)
                {
                    throw new InvalidOperationException($"Not enough stock for product {product.Name}. Available: {stockQuantity}");
                }
            }

            // Create new order
            var order = new Order
            {
                UserId = userId,
                OrderDate = DateTime.UtcNow,
                TotalAmount = cartItems.Sum(item => item.Quantity * item.UnitPrice),
                Status = "Pending",
                ShippingAddressId = createOrderDto.ShippingAddressId,
                PaymentMethod = createOrderDto.PaymentMethod,
                CreatedAt = DateTime.UtcNow,
                OrderItems = new List<OrderItem>()
            };

            // Create order items from cart items
            foreach (var cartItem in cartItems)
            {
                var orderItem = new OrderItem
                {
                    ProductId = cartItem.ProductId,
                    ProductVariantId = cartItem.ProductVariantId,
                    Quantity = cartItem.Quantity,
                    Price = cartItem.UnitPrice
                };

                order.OrderItems.Add(orderItem);

                // Update product stock
                var product = await _productRepository.GetByIdAsync(cartItem.ProductId);
                if (cartItem.ProductVariantId.HasValue)
                {
                    var variant = product.Variants.FirstOrDefault(v => v.ProductId == cartItem.ProductId);
                    if (variant != null)
                    {
                        variant.StockQuantity -= cartItem.Quantity;
                        
                       // await _productRepository.UpdateAsync(variant);
                    }
                }
                else
                {
                    product.ProductId -= cartItem.Quantity;
                    await _productRepository.UpdateAsync(product);
                }
            }

            // Save the order
            var createdOrder = await _orderRepository.CreateOrderAsync(order);

            // Create order history entry
            var newState = _orderStateContext.GetState("Pending");
            var historyEntry = newState.CreateHistoryEntry(createdOrder, userId, createOrderDto.OrderNotes);
            await _orderRepository.AddOrderHistoryAsync(historyEntry);

            // Remove items from cart
            foreach (var cartItem in cartItems)
            {
                await _cartRepository.DeleteCartItemAsync(cartItem.CartItemId);
            }

            // Update cart timestamp
            cart.UpdatedAt = DateTime.UtcNow;
            await _cartRepository.UpdateCartAsync(cart);

            return _mapper.Map<OrderDto>(createdOrder);
        }

        public async Task<IEnumerable<OrderDto>> GetAllOrdersAsync(int page = 1, int pageSize = 10)
        {
            var orders = await _orderRepository.GetAllOrdersAsync(page, pageSize);
            return _mapper.Map<IEnumerable<OrderDto>>(orders);
        }

        public async Task<OrderDto> GetOrderByIdAsync(int orderId, int userId)
        {
            var order = await _orderRepository.GetOrderByIdAsync(orderId);

            // Check if order belongs to user or user is admin
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                throw new UnauthorizedAccessException("User not found");
            }

            if (order.UserId != userId && user.Role != "Admin")
            {
                throw new UnauthorizedAccessException("You don't have permission to view this order");
            }

            return _mapper.Map<OrderDto>(order);
        }

        public async Task<IEnumerable<OrderHistoryDto>> GetOrderHistoryAsync(int orderId, int userId)
        {
            var order = await _orderRepository.GetOrderByIdAsync(orderId);
            if (order == null)
            {
                throw new KeyNotFoundException($"Order with ID {orderId} not found");
            }

            // Check if user has permission
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                throw new UnauthorizedAccessException("User not found");
            }

            if (order.UserId != userId && user.Role != "Admin")
            {
                throw new UnauthorizedAccessException("You don't have permission to view this order history");
            }

            var history = await _orderRepository.GetOrderHistoryAsync(orderId);
            return _mapper.Map<IEnumerable<OrderHistoryDto>>(history);
        }

        public async Task<OrderStatisticsDto> GetOrderStatisticsAsync(DateTime? startDate = null, DateTime? endDate = null)
        {
            var statistics = new OrderStatisticsDto
            {
                TotalOrders = await _orderRepository.GetTotalOrderCountAsync(),
                TotalRevenue = await _orderRepository.GetTotalRevenueAsync(startDate, endDate),
                PendingOrders = await _orderRepository.GetOrderCountByStatusAsync("Pending"),
                ProcessingOrders = await _orderRepository.GetOrderCountByStatusAsync("Processing"),
                ShippingOrders = await _orderRepository.GetOrderCountByStatusAsync("Shipping"),
                DeliveredOrders = await _orderRepository.GetOrderCountByStatusAsync("Delivered"),
                CanceledOrders = await _orderRepository.GetOrderCountByStatusAsync("Canceled"),
                RefundedOrders = await _orderRepository.GetOrderCountByStatusAsync("Refunded")
            };

            return statistics;
        }

        public async Task<IEnumerable<OrderDto>> GetUserOrdersAsync(int userId)
        {
            var orders = await _orderRepository.GetOrdersByUserIdAsync(userId);
            return _mapper.Map<IEnumerable<OrderDto>>(orders);
        }

        public async Task<OrderDto> UpdateOrderStatusAsync(int orderId, int userId, OrderStatusUpdateDto updateDto)
        {
            var order = await _orderRepository.GetOrderByIdAsync(orderId);
            if (order == null)
            {
                throw new KeyNotFoundException($"Order with ID {orderId} not found");
            }

            // Check if user has permission
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                throw new UnauthorizedAccessException("User not found");
            }

            bool hasPermission = user.Role == "Admin" || (order.UserId == userId && updateDto.Action == "Cancel");
            if (!hasPermission)
            {
                throw new UnauthorizedAccessException("You don't have permission to update this order");
            }

            // Get current state
            var currentState = _orderStateContext.GetState(order.Status);

            // Determine the next state based on action
            string newStatus;
            bool isValidTransition = false;

            switch (updateDto.Action.ToLower())
            {
                case "process":
                    isValidTransition = currentState.CanProcess(order);
                    newStatus = "Processing";
                    break;
                case "ship":
                    isValidTransition = currentState.CanShip(order);
                    newStatus = "Shipping";
                    break;
                case "deliver":
                    isValidTransition = currentState.CanDeliver(order);
                    newStatus = "Delivered";
                    break;
                case "cancel":
                    isValidTransition = currentState.CanCancel(order);
                    newStatus = "Canceled";
                    break;
                case "refund":
                    isValidTransition = currentState.CanRefund(order);
                    newStatus = "Refunded";
                    break;
                default:
                    throw new InvalidOperationException($"Invalid action: {updateDto.Action}");
            }

            if (!isValidTransition)
            {
                throw new InvalidOperationException($"Cannot {updateDto.Action} an order in {order.Status} state");
            }

            // Update order status
            var oldStatus = order.Status;
            order.Status = newStatus;
            order.UpdatedAt = DateTime.UtcNow;
            await _orderRepository.UpdateOrderAsync(order);

            // Create order history entry
            var newState = _orderStateContext.GetState(newStatus);
            var historyEntry = new OrderHistory
            {
                OrderId = order.OrderId,
                OldStatus = oldStatus,
                NewStatus = newStatus,
                ChangedAt = DateTime.UtcNow,
                ChangedBy = userId,
                Note = updateDto.Note
            };
            await _orderRepository.AddOrderHistoryAsync(historyEntry);

            // Handle inventory for canceled orders
            if (newStatus == "Canceled" || newStatus == "Refunded")
            {
                await ReturnItemsToInventoryAsync(order);
            }

            return _mapper.Map<OrderDto>(order);
        }

        // Helper method to return items to inventory when order is canceled
        private async Task ReturnItemsToInventoryAsync(Order order)
        {
            foreach (var item in order.OrderItems)
            {
                var product = await _productRepository.GetByIdAsync(item.ProductId);
                if (product != null)
                {
                    if (item.ProductVariantId.HasValue)
                    {
                        var variant = product.Variants.FirstOrDefault(v => v.ProductId == item.ProductId);
                        if (variant != null)
                        {
                            variant.StockQuantity += item.Quantity;
                           // await _productRepository.UpdateProductVariantAsync(variant);
                        }
                    }
                    else
                    {
                        product.ProductId += item.Quantity;
                        await _productRepository.UpdateAsync(product);
                    }
                }
            }
        }
    }
}
