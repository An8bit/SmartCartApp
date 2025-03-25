using Web.Models.Domain;

namespace Web.OrderStates
{
    public class DeliveredOrderState : IOrderState
    {
        public string StateName => "Delivered";

        public bool CanProcess(Order order) => false;
        public bool CanShip(Order order) => false;
        public bool CanDeliver(Order order) => false;
        public bool CanCancel(Order order) => false;
        public bool CanRefund(Order order) => true;

        public OrderHistory CreateHistoryEntry(Order order, int userId, string note)
        {
            return new OrderHistory
            {
                OrderId = order.OrderId,
                OldStatus = order.Status,
                NewStatus = StateName,
                ChangedAt = DateTime.UtcNow,
                ChangedBy = userId,
                Note = note ?? "Order delivered successfully"
            };
        }
    }
}