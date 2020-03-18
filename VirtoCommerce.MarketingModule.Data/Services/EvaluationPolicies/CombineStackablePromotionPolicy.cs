using System;
using System.Collections.Generic;
using System.Linq;
using VirtoCommerce.Domain.Common;
using VirtoCommerce.Domain.Marketing.Model;
using VirtoCommerce.Domain.Marketing.Model.Promotions.Search;
using VirtoCommerce.Domain.Marketing.Services;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.MarketingModule.Data.Services
{
    public class CombineStackablePromotionPolicy : IMarketingPromoEvaluator
    {
        private readonly IPromotionSearchService _promotionSearchService;
        private readonly IPromotionRewardEvaluator _promotionRewardEvaluator;

        public CombineStackablePromotionPolicy(IPromotionSearchService promotionSearchService, IPromotionRewardEvaluator promotionRewardEvaluator)
        {
            _promotionSearchService = promotionSearchService;
            _promotionRewardEvaluator = promotionRewardEvaluator;
        }

        public PromotionResult EvaluatePromotion(IEvaluationContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var promoContext = context as PromotionEvaluationContext;
            if (promoContext == null)
            {
                throw new ArgumentException($"{nameof(context)} type {context.GetType()} must be derived from PromotionEvaluationContext");
            }

            var promotionSearchCriteria = new PromotionSearchCriteria
            {
                OnlyActive = true,
                StoreIds = string.IsNullOrEmpty(promoContext.StoreId) ? null : new[] { promoContext.StoreId },
                Take = int.MaxValue
            };

            var promotions = _promotionSearchService.SearchPromotions(promotionSearchCriteria).Results;

            var result = new PromotionResult();

            EvalAndCombineRewardsRecursively(promoContext, promotions, result.Rewards, new List<PromotionReward>());
            return result;
        }

        protected virtual void EvalAndCombineRewardsRecursively(PromotionEvaluationContext context, IEnumerable<Promotion> promotions, ICollection<PromotionReward> resultRewards, ICollection<PromotionReward> skippedRewards)
        {
            //Evaluate rewards with passed context and exclude already applied rewards from result
            var rewards = _promotionRewardEvaluator.GetOrderedValidRewards(promotions, context)
                .Except(resultRewards)
                .Except(skippedRewards)
                .ToList();

            if (rewards.IsNullOrEmpty())
            {
                return;
            }

            var firstOrderExlusiveReward = rewards.FirstOrDefault(x => x.Promotion.IsExclusive);
            if (firstOrderExlusiveReward != null)
            {
                //Leave only exclusive promotion rewards even if they appeared as a result of other promotion with highest priority
                resultRewards.Clear();
                //Add only rewards from exclusive promotion
                resultRewards.AddRange(rewards.Where(x => x.Promotion == firstOrderExlusiveReward.Promotion));
            }
            else
            {
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

                //Gifts
                resultRewards.AddRange(highestPriorityPromotions.OfType<GiftReward>());
                //Special offer
                resultRewards.AddRange(highestPriorityPromotions.OfType<SpecialOfferReward>());

                //Apply new rewards to the evaluation context to influent for conditions in the  next evaluation iteration
                ApplyRewardsToContext(context, newRewards, skippedRewards);
                resultRewards.AddRange(newRewards.Except(skippedRewards));
                //If there any other rewards left need to cycle new iteration
                if (rewards.Any())
                {
                    //Call recursively
                    EvalAndCombineRewardsRecursively(context, promotions, resultRewards, skippedRewards);
                }
            }
        }

        protected virtual void ApplyRewardsToContext(PromotionEvaluationContext context, IEnumerable<PromotionReward> rewards, ICollection<PromotionReward> skippedRewards)
        {
            var activeShipmentReward = rewards.OfType<ShipmentReward>().Where(x => string.IsNullOrEmpty(x.ShippingMethod) || x.ShippingMethod.EqualsInvariant(context.ShipmentMethodCode))
                                                                       .FirstOrDefault(x => string.IsNullOrEmpty(x.ShippingMethodOption) || x.ShippingMethodOption.EqualsInvariant(context.ShipmentMethodOption));
            if (activeShipmentReward != null)
            {
                var discountAmount = activeShipmentReward.GetRewardAmount(context.ShipmentMethodPrice, 1);
                context.ShipmentMethodPrice -= discountAmount;
                //Do not allow to make negative shipment price
                if (context.ShipmentMethodPrice < 0 || discountAmount == 0)
                {
                    skippedRewards.Add(activeShipmentReward);
                    //restore back shipment price
                    context.ShipmentMethodPrice += discountAmount;
                }
            }

            foreach (var productReward in rewards.OfType<CatalogItemAmountReward>())
            {
                var promoEntry = context.PromoEntries.FirstOrDefault(x => string.IsNullOrEmpty(x.ProductId) || x.ProductId.EqualsInvariant(productReward.ProductId));
                if (promoEntry != null)
                {
                    var perUnitDiscountAmount = productReward.GetRewardAmount(promoEntry.Price, Math.Max(1, promoEntry.Quantity));
                    promoEntry.Price -= perUnitDiscountAmount;
                    //Do not allow to make negative the product price and exclude not affected rewards
                    if (promoEntry.Price < 0 || perUnitDiscountAmount == 0)
                    {
                        skippedRewards.Add(productReward);
                        //restore back prices and totals
                        promoEntry.Price += perUnitDiscountAmount;
                    }
                    else
                    {
                        //Need to do the same for cart promo entries because there is may be conditions which check these entries prices
                        var cartPromoEntry = context.CartPromoEntries.FirstOrDefault(x => x.ProductId.EqualsInvariant(productReward.ProductId));
                        if (cartPromoEntry != null)
                        {
                            perUnitDiscountAmount = productReward.GetRewardAmount(cartPromoEntry.Price, Math.Max(1, cartPromoEntry.Quantity));
                            cartPromoEntry.Price -= perUnitDiscountAmount;
                        }
                    }
                }
            }
        }
    }

}
