using Web.Models.Domain;
using Web.Models.DTO.UserDTOs;

namespace Web.UserStates
{
    public class SilverMemberState : IUserMembershipState
    {
        public string TierName => "Silver";

        public string TierCode => "SLV";

        public string BadgeColor => "#C0C0C0";

        public string BadgeIcon => "silver-icon.png";

        public decimal DiscountPercentage => 0.05m;

        public int FreeShippingMinOrder => 150;

        public bool HasPrioritySupport => false;

        public bool HasExclusiveAccess => false;

        public decimal CalculateDiscountedPrice(decimal originalPrice)
        {
            return originalPrice * (1 - DiscountPercentage);
        }

        public bool CanDemote(UserDtos user)
        {
            return user.TotalSpending < GetDemotionThreshold();
        }

        public bool CanPromote(UserDtos user)
        {
            return user.TotalSpending >= GetPromotionThreshold();
        }

        public decimal GetDemotionThreshold()
        {
            return 500m;
        }

        public IUserMembershipState GetNextTier()
        {
            return new GoldMemberState();
        }

        public IUserMembershipState GetPreviousTier()
        {
            return new SilverMemberState();
        }

        public decimal GetPromotionThreshold()
        {
            return 1000m;
        }

        public string GetUIThemePath()
        {
            return "themes/silver-theme.css";
        }
    }
}
