namespace Web.Models.DTO.MembershipDTOs
{
    public class MembershipSummaryDto
    {
        public int UserId { get; set; }
        public string TierName { get; set; }
        public string TierCode { get; set; }
        public string BadgeColor { get; set; }
        public string BadgeIcon { get; set; }
        public decimal DiscountPercentage { get; set; }
        public decimal TotalSpending { get; set; }
        public decimal NextTierThreshold { get; set; }
        public decimal SpendingToNextTier { get; set; }
        public bool HasPrioritySupport { get; set; }
        public bool HasExclusiveAccess { get; set; }
        public int FreeShippingMinOrder { get; set; }
    }
    public class UserMembershipHistoryDto
    {
        public int HistoryId { get; set; }
        public string OldTier { get; set; }
        public string NewTier { get; set; }
        public DateTime ChangedAt { get; set; }
        public string? Reason { get; set; }
    }
}
