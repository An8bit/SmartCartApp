using Web.Models.Domain;
using Web.Models.DTO.UserDTOs;

namespace Web.UserStates
{
    public interface IUserMembershipState
    {
        string TierName { get; }
        string TierCode { get; }
        string BadgeColor { get; }
        string BadgeIcon { get; }

        // Xác định mức giảm giá và các quyền lợi
        decimal DiscountPercentage { get; }
        int FreeShippingMinOrder { get; }
        bool HasPrioritySupport { get; }
        bool HasExclusiveAccess { get; }

        // Kiểm tra có thể nâng cấp hay giảm cấp
        bool CanPromote(UserDtos user);
        bool CanDemote(UserDtos user);

        // Tính toán giá sản phẩm sau khi áp dụng ưu đãi
        decimal CalculateDiscountedPrice(decimal originalPrice);

        // Lấy đường dẫn đến giao diện UI theme
        string GetUIThemePath();

        // Xác định hạng tiếp theo và hạng trước đó
        IUserMembershipState GetNextTier();
        IUserMembershipState GetPreviousTier();

        // Kiểm tra ngưỡng chi tiêu
        decimal GetPromotionThreshold();
        decimal GetDemotionThreshold();
    }
}
