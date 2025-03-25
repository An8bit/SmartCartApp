using Web.Models.Domain;

namespace Web.OrderStates
{
    public interface IOrderState
    {
        string StateName { get; }
        bool CanProcess(Order order);
        bool CanShip(Order order);
        bool CanDeliver(Order order);
        bool CanCancel(Order order);
        bool CanRefund(Order order);

        OrderHistory CreateHistoryEntry(Order order, int userId, string note);
    }
}
