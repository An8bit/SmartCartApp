using AutoMapper;
using Web.Models.Domain;
using Web.Models.DTO.MembershipDTOs;
using Web.UserStates;

namespace Web.MappingProfile
{
    public class UserMembershipProfile : Profile
    {
        public UserMembershipProfile()
        {
            // Map từ User và State sang MembershipSummaryDto
            CreateMap<(User User, IUserMembershipState State), MembershipSummaryDto>()
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.User.UserId))
                .ForMember(dest => dest.TierName, opt => opt.MapFrom(src => src.State.TierName))
                .ForMember(dest => dest.TierCode, opt => opt.MapFrom(src => src.State.TierCode))
                .ForMember(dest => dest.BadgeColor, opt => opt.MapFrom(src => src.State.BadgeColor))
                .ForMember(dest => dest.BadgeIcon, opt => opt.MapFrom(src => src.State.BadgeIcon))
                .ForMember(dest => dest.DiscountPercentage, opt => opt.MapFrom(src => src.State.DiscountPercentage))
                .ForMember(dest => dest.TotalSpending, opt => opt.MapFrom(src => src.User.TotalSpending))
                .ForMember(dest => dest.NextTierThreshold, opt => opt.MapFrom(src => src.State.GetPromotionThreshold()))
                .ForMember(dest => dest.SpendingToNextTier, opt => opt.MapFrom(src =>
                    Math.Max(0, src.State.GetPromotionThreshold() - src.User.TotalSpending)))
                .ForMember(dest => dest.HasPrioritySupport, opt => opt.MapFrom(src => src.State.HasPrioritySupport))
                .ForMember(dest => dest.HasExclusiveAccess, opt => opt.MapFrom(src => src.State.HasExclusiveAccess))
                .ForMember(dest => dest.FreeShippingMinOrder, opt => opt.MapFrom(src => src.State.FreeShippingMinOrder));

            // Map từ UserMembershipHistory sang UserMembershipHistoryDto
            CreateMap<UserMembershipHistory, UserMembershipHistoryDto>()
                .ForMember(dest => dest.OldTier, opt =>
                    opt.MapFrom(src => GetTierNameFromCode(src.OldTier)))
                .ForMember(dest => dest.NewTier, opt =>
                    opt.MapFrom(src => GetTierNameFromCode(src.NewTier)));
        }

        // Helper method để lấy tên tier từ mã
        private string GetTierNameFromCode(string tierCode)
        {
            var context = new UserStateContext();
            return context.GetState(tierCode).TierName;
        }
    }
}