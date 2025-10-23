using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using VirtoCommerce.CoreModule.Core.Common;
using VirtoCommerce.MarketingModule.Core.Model.Promotions;
using VirtoCommerce.MarketingModule.Core.Model.Promotions.Search;
using VirtoCommerce.MarketingModule.Core.Search;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.MarketingModule.Core.Promotions;

public class DynamicPromotion : Promotion
{
    public DynamicPromotion()
    {
        Type = nameof(DynamicPromotion);
    }

    /// <summary>
    /// If this flag is set to true, it allows this promotion to combine with itself.
    /// Special for case when need to return same promotion rewards for multiple coupons
    /// </summary>
    public bool IsAllowCombiningWithSelf { get; set; }

    [JsonIgnore]
    public ICouponSearchService CouponSearchService { get; set; }

    [JsonIgnore]
    public IPromotionUsageSearchService PromotionUsageSearchService { get; set; }

    public PromotionConditionAndRewardTree DynamicExpression { get; set; }

    public override async Task<IList<PromotionReward>> EvaluatePromotionAsync(IEvaluationContext context)
    {
        var rewards = new List<PromotionReward>();

        if (context is not PromotionEvaluationContext promoContext)
        {
            throw new ArgumentException($"context should be {nameof(PromotionEvaluationContext)}");
        }

        var validCoupons = HasCoupons
            ? await FindValidCouponsAsync(promoContext)
            : [];

        // Check coupon
        var couponIsValid = validCoupons.Count > 0 || !HasCoupons;

        promoContext = promoContext.Clone();

        // Evaluate reward for all promoEntry in context
        foreach (var promoEntry in promoContext.PromoEntries)
        {
            // Set current context promo entry for evaluation
            promoContext.PromoEntry = promoEntry;

            foreach (var reward in DynamicExpression?.GetRewards() ?? [])
            {
                var clonedReward = reward.CloneTyped();
                EvaluateReward(promoContext, couponIsValid, clonedReward);

                // Add coupon to reward only when promotion contains associated coupons
                if (validCoupons.Count > 0)
                {
                    // Need to return promotion rewards for each valid coupon if promotion IsAllowCombiningWithSelf flag set
                    foreach (var validCoupon in IsAllowCombiningWithSelf ? validCoupons : validCoupons.Take(1))
                    {
                        clonedReward.Promotion = this;
                        clonedReward.Coupon = validCoupon.Code;
                        rewards.Add(clonedReward);

                        // Clone reward for next iteration
                        clonedReward = clonedReward.CloneTyped();
                    }
                }
                else
                {
                    rewards.Add(clonedReward);
                }
            }
        }

        return rewards;
    }

    protected virtual void EvaluateReward(PromotionEvaluationContext promoContext, bool couponIsValid, PromotionReward reward)
    {
        reward.Promotion = this;
        reward.IsValid = couponIsValid && (DynamicExpression?.IsSatisfiedBy(promoContext) ?? false);

        // Set productId for catalog item reward
        if (reward is CatalogItemAmountReward catalogItemReward && catalogItemReward.ProductId == null)
        {
            catalogItemReward.ProductId = promoContext.PromoEntry.ProductId;
        }
    }

    protected virtual async Task<IList<Coupon>> FindValidCouponsAsync(PromotionEvaluationContext promoContext)
    {
        if (promoContext.Coupons.IsNullOrEmpty())
        {
            return [];
        }

        // Remove empty codes from input list
        var couponCodes = promoContext.Coupons
            .Where(x => !x.IsNullOrEmpty())
            .ToList();

        if (couponCodes.IsNullOrEmpty())
        {
            return [];
        }

        var searchCriteria = AbstractTypeFactory<CouponSearchCriteria>.TryCreateInstance();
        searchCriteria.PromotionId = Id;
        searchCriteria.Codes = couponCodes;

        var coupons = await CouponSearchService.SearchAllAsync(searchCriteria);

        var validCoupons = new List<Coupon>();

        foreach (var coupon in coupons.OrderBy(x => x.TotalUsesCount))
        {
            if (await CouponIsValid(coupon, promoContext))
            {
                validCoupons.Add(coupon);
            }
        }

        return validCoupons;
    }

    private async Task<bool> CouponIsValid(Coupon coupon, PromotionEvaluationContext promoContext)
    {
        var couponIsValid = true;

        if (coupon.ExpirationDate != null)
        {
            couponIsValid = coupon.ExpirationDate > DateTime.UtcNow;
        }

        if (couponIsValid && !coupon.MemberId.IsNullOrWhiteSpace())
        {
            couponIsValid = coupon.MemberId == promoContext.ContactId || coupon.MemberId == promoContext.OrganizationId;
        }

        if (couponIsValid && coupon.MaxUsesNumber > 0)
        {
            couponIsValid = await GetUsageCount(Id, coupon.Code) < coupon.MaxUsesNumber;
        }

        if (couponIsValid && coupon.MaxUsesPerUser > 0 && !promoContext.UserId.IsNullOrWhiteSpace())
        {
            couponIsValid = await GetUsageCount(Id, coupon.Code, promoContext.UserId) < coupon.MaxUsesPerUser;
        }

        return couponIsValid;
    }

    public override object Clone()
    {
        var result = (DynamicPromotion)base.Clone();

        if (DynamicExpression != null)
        {
            result.DynamicExpression = DynamicExpression.CloneTyped();
        }

        return result;
    }


    private async Task<int> GetUsageCount(string promotionId, string couponCode, string userId = null)
    {
        var searchCriteria = AbstractTypeFactory<PromotionUsageSearchCriteria>.TryCreateInstance();
        searchCriteria.PromotionId = promotionId;
        searchCriteria.CouponCode = couponCode;
        searchCriteria.UserId = userId;
        searchCriteria.Take = 0;

        var searchResult = await PromotionUsageSearchService.SearchAsync(searchCriteria);

        return searchResult.TotalCount;
    }
}
