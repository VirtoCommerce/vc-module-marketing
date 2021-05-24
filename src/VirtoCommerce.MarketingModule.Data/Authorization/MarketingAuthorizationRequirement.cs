using VirtoCommerce.Platform.Security.Authorization;

namespace VirtoCommerce.MarketingModule.Data.Authorization
{
    public sealed class MarketingAuthorizationRequirement : PermissionAuthorizationRequirement
    {
        public MarketingAuthorizationRequirement(string permission)
            : base(permission)
        {
        }
    }
}
