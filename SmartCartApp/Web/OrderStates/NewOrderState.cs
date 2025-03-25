using Web.Models.Domain;

namespace Web.OrderStates
{
    public class NewOrderState : IOrderState
    {
        //đơn hàng mới chưa được giải quyết
        public string StateName => "Pending";


        public bool CanCancel(Order order)
        {
            return true;
        }

        public bool CanDeliver(Order order)
        {
            return false;
        }

        public bool CanProcess(Order order)
        {
            return true;
        }

        public bool CanRefund(Order order)
        {
            return false;
        }

        public bool CanShip(Order order)
        {
           return false;
        }

        public OrderHistory CreateHistoryEntry(Order order, int userId, string note)
        {
            return new OrderHistory
            {
                OrderId = order.OrderId,
                OldStatus = order.Status,
                NewStatus = StateName,
                ChangedAt = DateTime.UtcNow,
                ChangedBy = userId,
                Note = note ?? "Order created"
            };
        }
    }
}
