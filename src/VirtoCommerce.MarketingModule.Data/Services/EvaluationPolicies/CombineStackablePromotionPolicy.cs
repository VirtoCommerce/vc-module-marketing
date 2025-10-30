using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VirtoCommerce.CoreModule.Core.Currency;
using VirtoCommerce.MarketingModule.Core.Model.Promotions;
using VirtoCommerce.MarketingModule.Core.Model.Promotions.Search;
using VirtoCommerce.MarketingModule.Core.Search;
using VirtoCommerce.MarketingModule.Core.Services;
using VirtoCommerce.MarketingModule.Data.Services.EvaluationPolicies;
using VirtoCommerce.Platform.Core.Caching;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.MarketingModule.Data.Services;

public class CombineStackablePromotionPolicy(
    ICurrencyService currencyService,
    IPlatformMemoryCache platformMemoryCache,
    IPromotionSearchService promotionSearchService,
    IPromotionRewardEvaluator promotionRewardEvaluator)
    : PromotionPolicyBase(currencyService, platformMemoryCache)
{
    protected override async Task<PromotionResult> EvaluatePromotionWithoutCache(PromotionEvaluationContext promoContext)
    {
        var searchCriteria = AbstractTypeFactory<PromotionSearchCriteria>.TryCreateInstance();
        searchCriteria.PopulateFromEvalContext(promoContext);
        searchCriteria.OnlyActive = true;
        searchCriteria.StoreIds = promoContext.StoreId.IsNullOrEmpty() ? null : [promoContext.StoreId];

        var promotions = await promotionSearchService.SearchAllAsync(searchCriteria);

        var result = new PromotionResult();

        await EvalAndCombineRewardsRecursivelyAsync(promoContext, promotions, result.Rewards, new List<PromotionReward>());

        return result;
    }

    protected virtual async Task EvalAndCombineRewardsRecursivelyAsync(
        PromotionEvaluationContext context,
        IList<Promotion> promotions,
        ICollection<PromotionReward> resultRewards,
        ICollection<PromotionReward> skippedRewards)
    {
        List<PromotionReward> rewards;

        do
        {
            // Evaluate rewards with passed context and exclude already applied rewards from result
            rewards = (await promotionRewardEvaluator.GetOrderedValidRewardsAsync(promotions, context))
                .Except(resultRewards)
                .Except(skippedRewards)
                .ToList();

            if (rewards.IsNullOrEmpty())
            {
                return;
            }

            var firstOrderExclusiveReward = rewards.FirstOrDefault(x => x.Promotion.IsExclusive);
            if (firstOrderExclusiveReward != null)
            {
                // Keep only exclusive promotion rewards, even if they appeared as a result of another promotion with a higher priority
                resultRewards.Clear();
                // Add only rewards from exclusive promotion
                rewards = rewards.Where(x => x.Promotion == firstOrderExclusiveReward.Promotion).ToList();
            }

            var highestPriorityRewards = rewards
                .GroupBy(x => x.Promotion.Priority)
                .OrderByDescending(x => x.Key)
                .First()
                .ToList();

            var newRewards = GetNewRewards(highestPriorityRewards, rewards);

            // Apply new rewards to the evaluation context to influence conditions in the next evaluation iteration
            ApplyNewRewardsToContext(context, newRewards, skippedRewards);

            resultRewards.AddRange(newRewards.Except(skippedRewards));

            // Throw exception if there are non-handled rewards. They could cause cycling otherwise.
            if (rewards.Count > 0 && newRewards.Count == 0)
            {
                var notSupportedRewards = highestPriorityRewards.Select(x => x.GetType().Name);
                throw new NotSupportedException($"Some reward types are not handled by the promotion policy: {string.Join(",", notSupportedRewards)}");
            }
        }
        while (rewards.Count > 0);
    }

    protected virtual IList<PromotionReward> GetNewRewards(IList<PromotionReward> highestPriorityRewards, IList<PromotionReward> rewards)
    {
        var newRewards = new List<PromotionReward>();

        AddProductRewards(newRewards, highestPriorityRewards, rewards);
        AddCartRewards(newRewards, highestPriorityRewards, rewards);
        AddShipmentRewards(newRewards, highestPriorityRewards, rewards);
        AddPaymentRewards(newRewards, highestPriorityRewards, rewards);
        AddGiftRewards(newRewards, highestPriorityRewards, rewards);
        AddSpecialOfferRewards(newRewards, highestPriorityRewards, rewards);

        return newRewards;
    }

    protected virtual void AddProductRewards(IList<PromotionReward> newRewards, IList<PromotionReward> highestPriorityRewards, IList<PromotionReward> rewards)
    {
        var groupedByProductRewards = highestPriorityRewards.OfType<CatalogItemAmountReward>().GroupBy(x => x.ProductId);

        foreach (var productRewards in groupedByProductRewards)
        {
            // Need to take one reward from first prioritized promotion for each product rewards group
            var productPriorityReward = productRewards.FirstOrDefault();

            if (productPriorityReward != null)
            {
                newRewards.Add(productPriorityReward);
                rewards.Remove(productPriorityReward);
            }
        }
    }

    protected virtual void AddCartRewards(IList<PromotionReward> newRewards, IList<PromotionReward> highestPriorityRewards, IList<PromotionReward> rewards)
    {
        var cartPriorityReward = highestPriorityRewards.OfType<CartSubtotalReward>().FirstOrDefault();

        if (cartPriorityReward != null)
        {
            newRewards.Add(cartPriorityReward);
            rewards.Remove(cartPriorityReward);
        }
    }

    protected virtual void AddShipmentRewards(IList<PromotionReward> newRewards, IList<PromotionReward> highestPriorityRewards, IList<PromotionReward> rewards)
    {
        var groupedByShipmentMethodRewards = highestPriorityRewards.OfType<ShipmentReward>().GroupBy(x => x.ShippingMethod);

        foreach (var shipmentMethodRewards in groupedByShipmentMethodRewards)
        {
            // Need to take one reward from first prioritized promotion for each shipment method group
            var shipmentPriorityReward = shipmentMethodRewards.FirstOrDefault();

            if (shipmentPriorityReward != null)
            {
                newRewards.Add(shipmentPriorityReward);
                rewards.Remove(shipmentPriorityReward);
            }
        }
    }

    protected virtual void AddPaymentRewards(IList<PromotionReward> newRewards, IList<PromotionReward> highestPriorityRewards, IList<PromotionReward> rewards)
    {
        var groupedByPaymentMethodRewards = highestPriorityRewards.OfType<PaymentReward>().GroupBy(x => x.PaymentMethod);

        foreach (var paymentMethodRewards in groupedByPaymentMethodRewards)
        {
            // Need to take one reward from first prioritized promotion for each payment method group
            var paymentPriorityReward = paymentMethodRewards.FirstOrDefault();

            if (paymentPriorityReward != null)
            {
                newRewards.Add(paymentPriorityReward);
                rewards.Remove(paymentPriorityReward);
            }
        }
    }

    protected virtual void AddGiftRewards(IList<PromotionReward> newRewards, IList<PromotionReward> highestPriorityRewards, IList<PromotionReward> rewards)
    {
        var giftRewards = highestPriorityRewards.OfType<GiftReward>();

        foreach (var giftReward in giftRewards)
        {
            newRewards.Add(giftReward);
            rewards.Remove(giftReward);
        }
    }

    protected virtual void AddSpecialOfferRewards(IList<PromotionReward> newRewards, IList<PromotionReward> highestPriorityRewards, IList<PromotionReward> rewards)
    {
        var specialOfferRewards = highestPriorityRewards.OfType<SpecialOfferReward>();

        foreach (var specialOfferReward in specialOfferRewards)
        {
            newRewards.Add(specialOfferReward);
            rewards.Remove(specialOfferReward);
        }
    }

    protected virtual void ApplyNewRewardsToContext(PromotionEvaluationContext context, IList<PromotionReward> newRewards, ICollection<PromotionReward> skippedRewards)
    {
        ApplyShipmentRewards(context, newRewards, skippedRewards);
        ApplyPaymentRewards(context, newRewards, skippedRewards);
        ApplyProductRewards(context, newRewards, skippedRewards);
    }

    protected virtual void ApplyShipmentRewards(PromotionEvaluationContext context, IList<PromotionReward> newRewards, ICollection<PromotionReward> skippedRewards)
    {
        var activeShipmentReward = newRewards.OfType<ShipmentReward>()
            .Where(x => x.ShippingMethod.IsNullOrEmpty() || x.ShippingMethod.EqualsIgnoreCase(context.ShipmentMethodCode))
            .FirstOrDefault(x => x.ShippingMethodOption.IsNullOrEmpty() || x.ShippingMethodOption.EqualsIgnoreCase(context.ShipmentMethodOption));

        if (activeShipmentReward is null)
        {
            return;
        }

        var discountAmount = activeShipmentReward.GetTotalAmount(context.ShipmentMethodPrice, 1, context.CurrencyObject);

        // Do not allow to make negative shipment price
        if (discountAmount <= 0 || discountAmount > context.ShipmentMethodPrice)
        {
            skippedRewards.Add(activeShipmentReward);
        }
        else
        {
            context.ShipmentMethodPrice -= discountAmount;
        }
    }

    protected virtual void ApplyPaymentRewards(PromotionEvaluationContext context, IList<PromotionReward> newRewards, ICollection<PromotionReward> skippedRewards)
    {
        var activePaymentReward = newRewards.OfType<PaymentReward>()
            .FirstOrDefault(x => x.PaymentMethod.IsNullOrEmpty() || x.PaymentMethod.EqualsIgnoreCase(context.PaymentMethodCode));

        if (activePaymentReward is null)
        {
            return;
        }

        var discountAmount = activePaymentReward.GetTotalAmount(context.PaymentMethodPrice, 1, context.CurrencyObject);

        // Do not allow to make negative payment price
        if (discountAmount <= 0 || discountAmount > context.PaymentMethodPrice)
        {
            skippedRewards.Add(activePaymentReward);
        }
        else
        {
            context.PaymentMethodPrice -= discountAmount;
        }
    }

    protected virtual void ApplyProductRewards(PromotionEvaluationContext context, IList<PromotionReward> newRewards, ICollection<PromotionReward> skippedRewards)
    {
        var currency = context.CurrencyObject;

        foreach (var productReward in newRewards.OfType<CatalogItemAmountReward>())
        {
            var promoEntry = context.PromoEntries.FirstOrDefault(x => x.ProductId.IsNullOrEmpty() || x.ProductId.EqualsIgnoreCase(productReward.ProductId));

            if (promoEntry is null)
            {
                continue;
            }

            var discountAmountPerItem = productReward.GetAmountPerItem(promoEntry.Price, Math.Max(1, promoEntry.Quantity), currency);

            // Do not allow to make negative product price
            if (discountAmountPerItem <= 0 || discountAmountPerItem > promoEntry.Price)
            {
                skippedRewards.Add(productReward);
            }
            else
            {
                promoEntry.Price -= discountAmountPerItem;

                // Need to do the same for cart promo entries because there may be conditions which check these entries prices
                var cartPromoEntry = context.CartPromoEntries.FirstOrDefault(x => x.ProductId.EqualsIgnoreCase(productReward.ProductId));

                if (cartPromoEntry != null)
                {
                    discountAmountPerItem = productReward.GetAmountPerItem(cartPromoEntry.Price, Math.Max(1, cartPromoEntry.Quantity), currency);
                    cartPromoEntry.Price -= discountAmountPerItem;
                }
            }
        }
    }
}
