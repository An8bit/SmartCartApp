using Web.Models.Domain;

namespace Web.OrderStates
{
    public class ProcessingOrderState : IOrderState
    {
        public string StateName => "Processing";

        public bool CanProcess(Order order) => false;
        public bool CanShip(Order order) => true;
        public bool CanDeliver(Order order) => false;
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
                Note = note ?? "Order processing started"
            };
        }
    }
}
