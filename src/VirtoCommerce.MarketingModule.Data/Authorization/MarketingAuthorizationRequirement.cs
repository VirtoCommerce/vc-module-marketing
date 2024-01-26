using VirtoCommerce.Platform.Security.Authorization;

namespace VirtoCommerce.MarketingModule.Data.Authorization
{
    public sealed class MarketingAuthorizationRequirement : PermissionAuthorizationRequirement
    {
        public bool CheckAllScopes { get; }

        public MarketingAuthorizationRequirement(string permission) : this(permission, false)
        {
        }

        public MarketingAuthorizationRequirement(string permission, bool checkAllScopes)
            : base(permission)
        {
            CheckAllScopes = checkAllScopes;
        }
    }
}
