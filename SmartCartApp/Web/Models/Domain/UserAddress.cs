using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Web.Models.Domain
{
    public class UserAddress
    {
        [Key]
        public int AddressId { get; set; }
        public int UserId { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
        public bool IsDefault { get; set; } = false;

        [JsonIgnore] // 🚀 Tránh vòng lặp
        public User User { get; set; }
    }
}
