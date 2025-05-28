using Web.Models.Domain;
using Web.Models.DTO.UserDTOs;

namespace Web.UserStates
{
    public class DiamondMemberState : IUserMembershipState
    {
        public string TierName => "Diamond";

        public string TierCode => "DMD";

        public string BadgeColor => "#b9f2ff"; // Light blue color

        public string BadgeIcon => "diamond-icon.png";

        public decimal DiscountPercentage => 0.20m; // 20% discount

        public int FreeShippingMinOrder => 50; // Free shipping for orders over $50

        public bool HasPrioritySupport => true;

        public bool HasExclusiveAccess => true;

        public decimal CalculateDiscountedPrice(decimal originalPrice)
        {
            return originalPrice * (1 - DiscountPercentage);
        }

      
        public bool CanDemote(UserDtos user)
        {
            return user.TotalSpending < GetDemotionThreshold();
        }

        public bool CanPromote(User user)
        {
            return false; // Diamond is the highest tier, so promotion is not possible
        }

        public bool CanPromote(UserDtos user)
        {
            throw new NotImplementedException();
        }

        public decimal GetDemotionThreshold()
        {
            return 5000m; // Demote if total spending falls below $5000
        }

        public IUserMembershipState GetNextTier()
        {
            return null; // Diamond is the highest tier, so no next tier
        }

        public IUserMembershipState GetPreviousTier()
        {
            throw new NotImplementedException();
        }

        //public IUserMembershipState GetPreviousTier()
        //{
        //    return new PlatinumMemberState(); // Assuming Platinum is the tier below Diamond
        //}

        public decimal GetPromotionThreshold()
        {
            return decimal.MaxValue; // No promotion threshold as Diamond is the highest tier
        }

        public string GetUIThemePath()
        {
            return "themes/diamond-theme.css";
        }
    }
}
