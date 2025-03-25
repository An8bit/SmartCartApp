using Web.Models.Domain;

namespace Web.OrderStates
{
    public class OrderStateContext
    {


        private readonly Dictionary<string, IOrderState> _states;

        public OrderStateContext()
        {
            _states = new Dictionary<string, IOrderState>
            {
                { "Pending", new NewOrderState() },
                { "Processing", new ProcessingOrderState() },
                { "Shipping", new ShippingOrderState() },
                { "Delivered", new DeliveredOrderState() },
                { "Canceled", new CanceledOrderState() },
                { "Refunded", new RefundedOrderState() }
            };
        }

        public IOrderState GetState(string stateName)
        {
            if (_states.TryGetValue(stateName, out var state))
            {
                return state;
            }

            throw new InvalidOperationException($"Unsupported order state: {stateName}");
        }

        public IOrderState GetState(Order order)
        {
            return GetState(order.Status);
        }
    }
}