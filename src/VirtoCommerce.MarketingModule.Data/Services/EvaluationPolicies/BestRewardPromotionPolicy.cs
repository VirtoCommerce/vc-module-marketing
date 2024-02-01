using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VirtoCommerce.MarketingModule.Core.Model.Promotions;
using VirtoCommerce.MarketingModule.Core.Model.Promotions.Search;
using VirtoCommerce.MarketingModule.Core.Search;
using VirtoCommerce.MarketingModule.Data.Services.EvaluationPolicies;
using VirtoCommerce.Platform.Core.Caching;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.MarketingModule.Data.Services
{
    public class BestRewardPromotionPolicy : PromotionPolicyBase
    {
        private readonly IPromotionSearchService _promotionSearchService;

        public BestRewardPromotionPolicy(IPromotionSearchService promotionSearchService, IPlatformMemoryCache platformMemoryCache)
            : base(platformMemoryCache)
        {
            _promotionSearchService = promotionSearchService;
        }

        protected override async Task<PromotionResult> EvaluatePromotionWithoutCache(PromotionEvaluationContext promoContext)
        {
            var promotionSearchCriteria = AbstractTypeFactory<PromotionSearchCriteria>.TryCreateInstance();
            promotionSearchCriteria.PopulateFromEvalContext(promoContext);

            promotionSearchCriteria.OnlyActive = true;
            promotionSearchCriteria.Take = int.MaxValue;
            promotionSearchCriteria.StoreIds = string.IsNullOrEmpty(promoContext.StoreId) ? null : new[] { promoContext.StoreId };

            var promotions = await _promotionSearchService.SearchPromotionsAsync(promotionSearchCriteria);

            var result = new PromotionResult();
            var evalPromotionTasks = promotions.Results.Select(x => x.EvaluatePromotionAsync(promoContext)).ToArray();
            await Task.WhenAll(evalPromotionTasks);
            var rewards = evalPromotionTasks.SelectMany(x => x.Result).Where(x => x.IsValid).ToArray();

            var firstOrderExclusiveReward = rewards.FirstOrDefault(x => x.Promotion.IsExclusive);
            if (firstOrderExclusiveReward != null)
            {
                //Add only rewards from exclusive promotion
                rewards = rewards.Where(x => x.Promotion == firstOrderExclusiveReward.Promotion).ToArray();
            }
            //best shipment promotion
            var curShipmentAmount = promoContext.ShipmentMethodCode != null ? promoContext.ShipmentMethodPrice : 0m;
            var allShipmentRewards = rewards.OfType<ShipmentReward>().ToArray();
            var groupedByShippingMethodRewards = allShipmentRewards.GroupBy(x => x.ShippingMethod);
            foreach (var shipmentRewards in groupedByShippingMethodRewards)
            {
                var bestShipmentReward = GetBestAmountReward(curShipmentAmount, shipmentRewards);
                if (bestShipmentReward != null)
                {
                    result.Rewards.Add(bestShipmentReward);
                }
            }

            //best payment promotion
            var currentPaymentAmount = promoContext.PaymentMethodCode != null ? promoContext.PaymentMethodPrice : 0m;
            var allPaymentRewards = rewards.OfType<PaymentReward>().ToArray();
            var groupedByPaymentMethodRewards = allPaymentRewards.GroupBy(x => x.PaymentMethod);
            foreach (var paymentRewards in groupedByPaymentMethodRewards)
            {
                var bestPaymentReward = GetBestAmountReward(currentPaymentAmount, paymentRewards);
                if (bestPaymentReward != null)
                {
                    result.Rewards.Add(bestPaymentReward);
                }
            }

            //best catalog item promotion
            var allItemsRewards = rewards.OfType<CatalogItemAmountReward>().ToArray();
            var groupRewards = allItemsRewards.GroupBy(x => x.ProductId).Where(x => x.Key != null);
            foreach (var groupReward in groupRewards)
            {
                var item = promoContext.PromoEntries.FirstOrDefault(x => x.ProductId == groupReward.Key);
                if (item != null)
                {
                    var bestItemReward = GetBestAmountReward(item.Price, item.Quantity, groupReward);
                    if (bestItemReward != null)
                    {
                        result.Rewards.Add(bestItemReward);
                    }
                }
            }

            //best order promotion 
            var cartSubtotalRewards = rewards.OfType<CartSubtotalReward>().Where(x => x.IsValid).OrderByDescending(x => x.GetRewardAmount(promoContext.CartTotal, 1));
            var cartSubtotalReward = cartSubtotalRewards.FirstOrDefault(x => !string.IsNullOrEmpty(x.Coupon)) ?? cartSubtotalRewards.FirstOrDefault();
            if (cartSubtotalReward != null)
            {
                result.Rewards.Add(cartSubtotalReward);
            }

            //Gifts
            rewards.OfType<GiftReward>().ToList().ForEach(x => result.Rewards.Add(x));

            //Special offer
            rewards.OfType<SpecialOfferReward>().ToList().ForEach(x => result.Rewards.Add(x));

            return result;
        }

        protected virtual AmountBasedReward GetBestAmountReward(decimal currentAmount, IEnumerable<AmountBasedReward> reward)
        {
            return GetBestAmountReward(currentAmount, 1, reward);
        }

        protected virtual AmountBasedReward GetBestAmountReward(decimal currentAmount, int quantity, IEnumerable<AmountBasedReward> reward)
        {
            AmountBasedReward retVal = null;
            var maxAbsoluteReward = reward
                .Where(y => y.AmountType == RewardAmountType.Absolute)
                .OrderByDescending(y => y.GetRewardAmount(currentAmount, quantity)).FirstOrDefault();

            var maxRelativeReward = reward
                .Where(y => y.AmountType == RewardAmountType.Relative)
                .OrderByDescending(y => y.GetRewardAmount(currentAmount, quantity)).FirstOrDefault();

            var absDiscountAmount = maxAbsoluteReward != null ? maxAbsoluteReward.GetRewardAmount(currentAmount, quantity) : 0;
            var relDiscountAmount = maxRelativeReward != null ? currentAmount * maxRelativeReward.GetRewardAmount(currentAmount, quantity) : 0;

            if (maxAbsoluteReward != null && maxRelativeReward != null)
            {
                retVal = absDiscountAmount > relDiscountAmount ? maxAbsoluteReward : maxRelativeReward;
            }
            else if (maxAbsoluteReward != null)
            {
                retVal = maxAbsoluteReward;
            }
            else if (maxRelativeReward != null)
            {
                retVal = maxRelativeReward;
            }

            return retVal;
        }
    }
}
