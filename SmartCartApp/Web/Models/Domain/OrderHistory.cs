using System.ComponentModel.DataAnnotations;

namespace Web.Models.Domain
{
    public class OrderHistory
    {
        [Key]
        public int HistoryId { get; set; }
        public int OrderId { get; set; }
        public string OldStatus { get; set; }
        public string NewStatus { get; set; }
        public DateTime ChangedAt { get; set; } = DateTime.UtcNow;
        public int ChangedBy { get; set; }  // Ai thay đổi (Admin hoặc User)
        public string Note { get; set; }  // Lý do thay đổi

        public Order Order { get; set; }
    }
}
