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

namespace VirtoCommerce.MarketingModule.Web.Authorization;

public sealed class MarketingAuthorizationHandler(
    IOptions<MvcNewtonsoftJsonOptions> jsonOptions,
    IPromotionService promotionService,
    ICouponService couponService)
    : PermissionAuthorizationHandlerBase<MarketingAuthorizationRequirement>
{
    private readonly MvcNewtonsoftJsonOptions _jsonOptions = jsonOptions.Value;

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

        var allowedStoreIds = userPermission.AssignedScopes
            .OfType<MarketingStoreSelectedScope>()
            .Select(x => x.StoreId)
            .Distinct()
            .ToArray();

        switch (context.Resource)
        {
            case PromotionSearchCriteria criteria:
                criteria.StoreIds = allowedStoreIds;
                context.Succeed(requirement);
                break;
            case DynamicPromotion promotion:
                if (StoreInScope(promotion.StoreIds.ToArray(), allowedStoreIds, requirement.CheckAllScopes))
                {
                    context.Succeed(requirement);
                }
                break;
            case IEnumerable<Coupon> coupons:
                if (await CouponsInScope(coupons, allowedStoreIds, requirement.CheckAllScopes))
                {
                    context.Succeed(requirement);
                }
                break;
            case PermissionResourceModel resource:
                if (await CouponsInScope(resource.CouponIds, allowedStoreIds, requirement.CheckAllScopes)
                    || await PromotionsInScope(resource.PromotionIds, allowedStoreIds, requirement.CheckAllScopes))
                {
                    context.Succeed(requirement);
                }
                break;
        }
    }

    private async Task<bool> CouponsInScope(string[] couponIds, string[] allowedStoreIds, bool checkAllScopes)
    {
        if (couponIds.IsNullOrEmpty())
        {
            return false;
        }

        var coupons = await couponService.GetAsync(couponIds);

        return await CouponsInScope(coupons, allowedStoreIds, checkAllScopes);
    }

    private async Task<bool> CouponsInScope(IEnumerable<Coupon> coupons, string[] allowedStoreIds, bool checkAllScopes)
    {
        var promotionIds = coupons.Select(x => x.PromotionId).Distinct().ToArray();
        var promotions = await promotionService.GetAsync(promotionIds);
        var storesIds = promotions.SelectMany(x => x.StoreIds).ToArray();

        return StoreInScope(storesIds, allowedStoreIds, checkAllScopes);
    }

    private async Task<bool> PromotionsInScope(string[] promotionIds, string[] allowedStoreIds, bool checkAllScopes)
    {
        if (promotionIds.IsNullOrEmpty())
        {
            return false;
        }
        var promotions = await promotionService.GetAsync(promotionIds);
        var storeIds = promotions.SelectMany(x => x.StoreIds).ToArray();

        return StoreInScope(storeIds, allowedStoreIds, checkAllScopes);
    }

    private static bool StoreInScope(string[] currentStoreIds, string[] allowedStoreIds, bool checkAllScopes)
    {
        return currentStoreIds.IsNullOrEmpty() || CheckAll() || CheckAny();

        bool CheckAll() => checkAllScopes && currentStoreIds.All(allowedStoreIds.Contains);
        bool CheckAny() => !checkAllScopes && currentStoreIds.Any(allowedStoreIds.Contains);
    }
}
