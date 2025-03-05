namespace Web.Models.DTO
{
    public class UserAddressDto
    {
        public int AddressId { get; set; }
        public required string AddressLine1 { get; set; }
        public required string AddressLine2 { get; set; }
        public required string City { get; set; }
        public required string State { get; set; }
        public required string PostalCode { get; set; }
        public  bool IsDefault { get; set; }
    }
}
