using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Web.Models.Domain
{
    public class OrderHistory
    {
        [Key]
        public int HistoryId { get; set; }

        public int OrderId { get; set; }

        [Required]
        [StringLength(50)]
        public string OldStatus { get; set; }

        [Required]
        [StringLength(50)]
        public string NewStatus { get; set; }

        public DateTime ChangedAt { get; set; } = DateTime.UtcNow;

        public int ChangedBy { get; set; }

        [StringLength(500)]
        public string? Note { get; set; }

        [ForeignKey("OrderId")]
        [JsonIgnore]
        public Order Order { get; set; }
    }
}