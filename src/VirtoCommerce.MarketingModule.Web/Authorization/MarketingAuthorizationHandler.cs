using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using VirtoCommerce.MarketingModule.Core.Model.Promotions.Search;
using VirtoCommerce.MarketingModule.Core.Promotions;
using VirtoCommerce.MarketingModule.Data.Authorization;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Security.Authorization;

namespace VirtoCommerce.MarketingModule.Web.Authorization
{
    public sealed class MarketingAuthorizationHandler : PermissionAuthorizationHandlerBase<MarketingAuthorizationRequirement>
    {
        private readonly MvcNewtonsoftJsonOptions _jsonOptions;
        public MarketingAuthorizationHandler(IOptions<MvcNewtonsoftJsonOptions> jsonOptions)
        {
            _jsonOptions = jsonOptions.Value;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, MarketingAuthorizationRequirement requirement)
        {
            await base.HandleRequirementAsync(context, requirement);
            if (context.HasSucceeded)
            {
                return;
            }

            var userPermission = context.User.FindPermission(requirement.Permission, _jsonOptions.SerializerSettings);
            if (userPermission == null)
            {
                return;
            }

            var storeSelectedScopes = userPermission.AssignedScopes.OfType<MarketingStoreSelectedScope>();
            var allowedStoreIds = storeSelectedScopes.Select(x => x.StoreId).Distinct().ToArray();
            if (context.Resource is PromotionSearchCriteria criteria)
            {
                criteria.StoreIds = allowedStoreIds;
                context.Succeed(requirement);
            }
            if (context.Resource is DynamicPromotion promotion && (promotion.StoreIds.IsNullOrEmpty() || promotion.StoreIds.All(x => allowedStoreIds.Contains(x))))
            {
                context.Succeed(requirement);
            }
        }
    }
}
