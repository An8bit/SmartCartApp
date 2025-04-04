using Web.Models.Domain;

namespace Web.UserStates
{
    public class GoldMemberState : IUserMembershipState
    {
        public string TierName => "Gold";

        public string TierCode => "GLD";

        public string BadgeColor => "#FFD700"; // Gold color

        public string BadgeIcon => "gold-icon.png";

        public decimal DiscountPercentage => 0.10m; // 10% discount

        public int FreeShippingMinOrder => 100; // Free shipping for orders over $100

        public bool HasPrioritySupport => true;

        public bool HasExclusiveAccess => false;

        public decimal CalculateDiscountedPrice(decimal originalPrice)
        {
            return originalPrice * (1 - DiscountPercentage);
        }

        public bool CanDemote(User user)
        {
            return user.TotalSpending < GetDemotionThreshold();
        }

        public bool CanPromote(User user)
        {
            return user.TotalSpending >= GetPromotionThreshold();
        }

        public decimal GetDemotionThreshold()
        {
            return 1000m; // Demote if total spending falls below $1000
        }

        public IUserMembershipState GetNextTier()
        {
            return new DiamondMemberState(); // Assuming Platinum is the tier above Gold
        }

        public IUserMembershipState GetPreviousTier()
        {
            return new SilverMemberState(); // Assuming Silver is the tier below Gold
        }

        public decimal GetPromotionThreshold()
        {
            return 5000m; // Promote if total spending reaches $5000
        }

        public string GetUIThemePath()
        {
            return "themes/gold-theme.css";
        }
    }
}
