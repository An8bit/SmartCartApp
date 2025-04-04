using Web.Models.Domain;

namespace Web.UserStates
{
    public class UserStateContext
    {

        private readonly Dictionary<string, IUserMembershipState> _states;

        public UserStateContext()
        {
            _states = new Dictionary<string, IUserMembershipState>
            {
                { "STD", new StandardMemberState() },
                { "SLV", new SilverMemberState() },
                { "GLD", new GoldMemberState() },
                { "DMD", new DiamondMemberState() }
            };
        }

        public IUserMembershipState GetState(string tierCode)
        {
            if (_states.TryGetValue(tierCode, out var state))
            {
                return state;
            }

            // Mặc định trả về Standard nếu không tìm thấy
            return _states["STD"];
        }

        public IUserMembershipState GetUserState(User user)
        {
            return GetState(user.MembershipTier);
        }

        public bool CheckAndUpdateUserTier(User user)
        {
            var currentState = GetUserState(user);

            // Kiểm tra nâng cấp
            if (currentState.CanPromote(user))
            {
                var nextTier = currentState.GetNextTier();
                user.MembershipTier = nextTier.TierCode;
                user.UpdatedAt = DateTime.UtcNow;
                return true;
            }

            // Kiểm tra giảm cấp
            if (currentState.CanDemote(user))
            {
                var previousTier = currentState.GetPreviousTier();
                user.MembershipTier = previousTier.TierCode;
                user.UpdatedAt = DateTime.UtcNow;
                return true;
            }

            return false;
        }
    }
}