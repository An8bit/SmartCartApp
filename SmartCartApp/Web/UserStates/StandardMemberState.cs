using Web.Models.Domain;

namespace Web.UserStates
{
    public class StandardMemberState : IUserMembershipState
    {
        public string TierName => "Standard";

        public string TierCode => "STD";

        public string BadgeColor => "#808080";

        public string BadgeIcon => "standard-icon.png";

        public decimal DiscountPercentage => 0.00m;

        public int FreeShippingMinOrder => 200;

        public bool HasPrioritySupport => false;

        public bool HasExclusiveAccess => false;

        public decimal CalculateDiscountedPrice(decimal originalPrice)
        {
            return originalPrice;
        }

        public bool CanDemote(User user)
        {
            return false;
        }

        public bool CanPromote(User user)
        {
            return user.TotalSpending >= GetPromotionThreshold();
        }

        public decimal GetDemotionThreshold()
        {
            return decimal.MinValue;
        }

        public IUserMembershipState GetNextTier()
        {
            return new SilverMemberState();
        }

        public IUserMembershipState GetPreviousTier()
        {
            return null;
        }

        public decimal GetPromotionThreshold()
        {
            return 500m;
        }

        public string GetUIThemePath()
        {
            return "themes/standard-theme.css";
        }
    }
}
