using Web.Models.Domain;

namespace Web.OrderStates
{
    public class ShippingOrderState : IOrderState
    {
        public string StateName => "Shipping";

        public bool CanProcess(Order order) => false;
        public bool CanShip(Order order) => false;
        public bool CanDeliver(Order order) => true;
        public bool CanCancel(Order order) => true;
        public bool CanRefund(Order order) => false;

        public OrderHistory CreateHistoryEntry(Order order, int userId, string note)
        {
            return new OrderHistory
            {
                OrderId = order.OrderId,
                OldStatus = order.Status,
                NewStatus = StateName,
                ChangedAt = DateTime.UtcNow,
                ChangedBy = userId,
                Note = note ?? "Order shipped"
            };
        }
    }
}