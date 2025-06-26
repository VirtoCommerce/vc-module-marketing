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

namespace VirtoCommerce.MarketingModule.Data.Services
{
    public class CombineStackablePromotionPolicy : PromotionPolicyBase
    {
        private readonly IPromotionSearchService _promotionSearchService;
        private readonly IPromotionRewardEvaluator _promotionRewardEvaluator;

        public CombineStackablePromotionPolicy(
            ICurrencyService currencyService,
            IPlatformMemoryCache platformMemoryCache,
            IPromotionSearchService promotionSearchService,
            IPromotionRewardEvaluator promotionRewardEvaluator)
            : base(currencyService, platformMemoryCache)
        {
            _promotionSearchService = promotionSearchService;
            _promotionRewardEvaluator = promotionRewardEvaluator;
        }

        protected override async Task<PromotionResult> EvaluatePromotionWithoutCache(PromotionEvaluationContext promoContext)
        {
            var promotionSearchCriteria = AbstractTypeFactory<PromotionSearchCriteria>.TryCreateInstance();
            promotionSearchCriteria.PopulateFromEvalContext(promoContext);

            promotionSearchCriteria.OnlyActive = true;
            promotionSearchCriteria.Take = int.MaxValue;
            promotionSearchCriteria.StoreIds = string.IsNullOrEmpty(promoContext.StoreId) ? null : [promoContext.StoreId];

            var promotions = await _promotionSearchService.SearchPromotionsAsync(promotionSearchCriteria);

            var result = new PromotionResult();

            await EvalAndCombineRewardsRecursivelyAsync(promoContext, promotions.Results, result.Rewards, new List<PromotionReward>());

            return result;
        }

        protected virtual async Task EvalAndCombineRewardsRecursivelyAsync(PromotionEvaluationContext context, IEnumerable<Promotion> promotions, ICollection<PromotionReward> resultRewards, ICollection<PromotionReward> skippedRewards)
        {
            //Evaluate rewards with passed context and exclude already applied rewards from result
            var rewards = (await _promotionRewardEvaluator.GetOrderedValidRewardsAsync(promotions, context))
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
                //Add only rewards from exclusive promotion
                rewards = rewards.Where(x => x.Promotion == firstOrderExclusiveReward.Promotion).ToList();
            }
            var promotionsByRewards = rewards
                .GroupBy(x => x.Promotion.Priority)
                .OrderByDescending(x => x.Key);
            var highestPriorityPromotions = promotionsByRewards.First().ToList();
            var newRewards = new List<PromotionReward>();

            //catalog item rewards
            var groupedByProductRewards = highestPriorityPromotions.OfType<CatalogItemAmountReward>()
                                                 .GroupBy(x => x.ProductId);
            foreach (var productRewards in groupedByProductRewards)
            {
                //Need to take one reward from first prioritized promotion for each product rewards group
                var productPriorityReward = productRewards.FirstOrDefault();
                if (productPriorityReward != null)
                {
                    newRewards.Add(productPriorityReward);
                    rewards.Remove(productPriorityReward);
                }
            }

            //cart subtotal rewards
            var cartPriorityReward = highestPriorityPromotions.OfType<CartSubtotalReward>().FirstOrDefault();
            if (cartPriorityReward != null)
            {
                newRewards.Add(cartPriorityReward);
                rewards.Remove(cartPriorityReward);
            }

            //shipment rewards
            var groupedByShipmentMethodRewards = highestPriorityPromotions.OfType<ShipmentReward>()
                                                        .GroupBy(x => x.ShippingMethod);
            foreach (var shipmentMethodRewards in groupedByShipmentMethodRewards)
            {
                //Need to take one reward from first prioritized promotion for each shipment method group
                var shipmentPriorityReward = shipmentMethodRewards.FirstOrDefault();
                if (shipmentPriorityReward != null)
                {
                    newRewards.Add(shipmentPriorityReward);
                    rewards.Remove(shipmentPriorityReward);
                }
            }

            //payment method rewards
            var groupedByPaymentMethodRewards = highestPriorityPromotions.OfType<PaymentReward>()
                                                        .GroupBy(x => x.PaymentMethod);
            foreach (var paymentMethodRewards in groupedByPaymentMethodRewards)
            {
                //Need to take one reward from first prioritized promotion for each payment method group
                var paymentPriorityReward = paymentMethodRewards.FirstOrDefault();
                if (paymentPriorityReward != null)
                {
                    newRewards.Add(paymentPriorityReward);
                    rewards.Remove(paymentPriorityReward);
                }
            }

            //Gifts
            var giftRewards = highestPriorityPromotions.OfType<GiftReward>();
            foreach (var giftReward in giftRewards)
            {
                newRewards.Add(giftReward);
                rewards.Remove(giftReward);
            }

            //Special offer
            var specialOfferRewards = highestPriorityPromotions.OfType<SpecialOfferReward>();
            foreach (var specialOfferReward in specialOfferRewards)
            {
                newRewards.Add(specialOfferReward);
                rewards.Remove(specialOfferReward);
            }

            // Apply new rewards to the evaluation context to influence conditions in the next evaluation iteration
            ApplyRewardsToContext(context, newRewards, skippedRewards);
            resultRewards.AddRange(newRewards.Except(skippedRewards));
            // If there are other rewards left, run a new iteration
            if (rewards.Count > 0)
            {
                // Throw exception if there are non-handled rewards. They could cause cycling otherwise.
                if (!newRewards.Any())
                {
                    var notSupportedRewards = highestPriorityPromotions.Select(x => x.GetType().Name).ToArray();

                    throw new NotSupportedException($"Some reward types are not handled by the promotion policy: {string.Join(",", notSupportedRewards)}");
                }

                //Call recursively
                await EvalAndCombineRewardsRecursivelyAsync(context, promotions, resultRewards, skippedRewards);
            }
        }

        protected virtual void ApplyRewardsToContext(PromotionEvaluationContext context, IEnumerable<PromotionReward> rewards, ICollection<PromotionReward> skippedRewards)
        {
            var currency = context.CurrencyObject;

            var activeShipmentReward = rewards.OfType<ShipmentReward>()
                .Where(x => string.IsNullOrEmpty(x.ShippingMethod) || x.ShippingMethod.EqualsIgnoreCase(context.ShipmentMethodCode))
                .FirstOrDefault(x => string.IsNullOrEmpty(x.ShippingMethodOption) || x.ShippingMethodOption.EqualsIgnoreCase(context.ShipmentMethodOption));

            if (activeShipmentReward != null)
            {
                var discountAmount = activeShipmentReward.GetTotalAmount(context.ShipmentMethodPrice, 1, currency);

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

            var activePaymentReward = rewards.OfType<PaymentReward>()
                .FirstOrDefault(x => string.IsNullOrEmpty(x.PaymentMethod) || x.PaymentMethod.EqualsIgnoreCase(context.PaymentMethodCode));

            if (activePaymentReward != null)
            {
                var discountAmount = activePaymentReward.GetTotalAmount(context.PaymentMethodPrice, 1, currency);

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

            foreach (var productReward in rewards.OfType<CatalogItemAmountReward>())
            {
                var promoEntry = context.PromoEntries.FirstOrDefault(x => string.IsNullOrEmpty(x.ProductId) || x.ProductId.EqualsIgnoreCase(productReward.ProductId));

                if (promoEntry != null)
                {
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
    }
}
