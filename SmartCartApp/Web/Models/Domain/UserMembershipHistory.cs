using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Web.Models.Domain
{
    public class UserMembershipHistory
    {
        [Key]
        public int HistoryId { get; set; }

        public int UserId { get; set; }

        [Required]
        [StringLength(3)]
        public string OldTier { get; set; }

        [Required]
        [StringLength(3)]
        public string NewTier { get; set; }

        public DateTime ChangedAt { get; set; } = DateTime.UtcNow;

        [StringLength(200)]
        public string? Reason { get; set; }

        [ForeignKey("UserId")]
        [JsonIgnore]
        public User User { get; set; }
    }
}
