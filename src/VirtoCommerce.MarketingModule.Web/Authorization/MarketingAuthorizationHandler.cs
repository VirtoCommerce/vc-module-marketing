using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using VirtoCommerce.MarketingModule.Core.Model.Promotions;
using VirtoCommerce.MarketingModule.Core.Model.Promotions.Search;
using VirtoCommerce.MarketingModule.Core.Promotions;
using VirtoCommerce.MarketingModule.Core.Services;
using VirtoCommerce.MarketingModule.Data.Authorization;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Security.Authorization;

namespace VirtoCommerce.MarketingModule.Web.Authorization
{
    public sealed class MarketingAuthorizationHandler : PermissionAuthorizationHandlerBase<MarketingAuthorizationRequirement>
    {
        private readonly IPromotionService _promotionService;
        private readonly ICouponService _couponService;
        private readonly MvcNewtonsoftJsonOptions _jsonOptions;
        public MarketingAuthorizationHandler(IOptions<MvcNewtonsoftJsonOptions> jsonOptions, IPromotionService promotionService, ICouponService couponService)
        {
            _promotionService = promotionService;
            _couponService = couponService;
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
            switch (context.Resource)
            {
                case PromotionSearchCriteria criteria:
                    criteria.StoreIds = allowedStoreIds;
                    context.Succeed(requirement);
                    break;
                case DynamicPromotion promotion:
                    if ((IsStoreInScope(promotion.StoreIds.ToArray(), allowedStoreIds, requirement.CheckAllScopes)))
                    {
                        context.Succeed(requirement);
                    }
                    break;
                case IEnumerable<Coupon> coupons:
                    if (await IsCouponsInScope(coupons, allowedStoreIds, requirement.CheckAllScopes))
                    {
                        context.Succeed(requirement);
                    }

                    break;
                case string[] couponIds:
                    {
                        var coupons = await _couponService.GetByIdsAsync(couponIds);
                        if (await IsCouponsInScope(coupons, allowedStoreIds, requirement.CheckAllScopes))
                        {
                            context.Succeed(requirement);
                        }
                    }
                    break;
            }
        }

        private async Task<bool> IsCouponsInScope(IEnumerable<Coupon> coupons, string[] allowedStoreIds, bool checkAllScopes)
        {
            var promotionIds = coupons.Select(x => x.PromotionId).Distinct().ToArray();
            var promotions = await _promotionService.GetPromotionsByIdsAsync(promotionIds);
            var storesIds = promotions.SelectMany(x => x.StoreIds).ToArray();
            return IsStoreInScope(storesIds, allowedStoreIds, checkAllScopes);
        }

        private static bool IsStoreInScope(string[] currentStoreIds, string[] allowedStoreIds, bool checkAllScopes)
        {
            return currentStoreIds.IsNullOrEmpty() || CheckAll() || CheckAny();
            bool CheckAll() => checkAllScopes && currentStoreIds.All(allowedStoreIds.Contains);
            bool CheckAny() => !checkAllScopes && currentStoreIds.Any(allowedStoreIds.Contains);
        }
    }
}
