using System;
using System.Collections.Generic;
using System.Linq;
using CacheManager.Core;
using VirtoCommerce.Domain.Common;
using VirtoCommerce.Domain.Marketing.Model;
using VirtoCommerce.Domain.Marketing.Services;
using VirtoCommerce.MarketingModule.Data.Repositories;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Data.Common;

namespace VirtoCommerce.MarketingModule.Data.Services
{
    public class DefaultPromotionEvaluatorImpl : IMarketingPromoEvaluator
    {
        private readonly IPromotionService _promotionService;
        private readonly ICacheManager<object> _cacheManager;
        private readonly Func<IMarketingRepository> _marketingRepositoryFactory;

        public DefaultPromotionEvaluatorImpl(
            IPromotionService promotionService,
            ICacheManager<object> cacheManager,
            Func<IMarketingRepository> marketingRepositoryFactory)
        {
            _promotionService = promotionService;
            _cacheManager = cacheManager;
            _marketingRepositoryFactory = marketingRepositoryFactory;
        }

        #region IMarketingEvaluator Members

        public PromotionResult EvaluatePromotion(IEvaluationContext context)
        {
            var promoContext = (PromotionEvaluationContext)context;

            var promotions = _cacheManager.Get("IPromotionService.GetActivePromotions", "MarketingModuleRegion", () => _promotionService.GetActivePromotions());

            //filter by store
            if (!string.IsNullOrEmpty(promoContext.StoreId))
            {
                promotions = promotions.Where(x => string.Equals(x.Store, promoContext.StoreId, StringComparison.InvariantCultureIgnoreCase)).ToArray();
            }

            var retVal = new PromotionResult();

            var rewards = promotions.SelectMany(x => x.EvaluatePromotion(context)).ToArray();

            //best shipment promotion
            var curShipmentAmount = promoContext.ShipmentMethodCode != null ? promoContext.ShipmentMethodPrice : 0m;
            var allShipmentRewards = rewards.OfType<ShipmentReward>().ToArray();
            var groupedByShippingMethodRewards = allShipmentRewards.GroupBy(x => x.ShippingMethod).Where(x => x.Key != null);
            foreach (var shipmentRewards in groupedByShippingMethodRewards)
            {
                EvaluteBestAmountRewards(curShipmentAmount, shipmentRewards.ToArray()).ToList().ForEach(x => retVal.Rewards.Add(x));
            }

            //best catalog item promotion
            var allItemsRewards = rewards.OfType<CatalogItemAmountReward>().ToArray();
            var groupRewards = allItemsRewards.GroupBy(x => x.ProductId).Where(x => x.Key != null);
            foreach (var groupReward in groupRewards)
            {
                var item = promoContext.PromoEntries.FirstOrDefault(x => x.ProductId == groupReward.Key);
                if (item != null)
                {
                    EvaluteBestAmountRewards(item.Price, groupReward.ToArray()).ToList().ForEach(x => retVal.Rewards.Add(x));
                }
            }

            //best order promotion 
            var cartSubtotalRewards = rewards.OfType<CartSubtotalReward>().Where(x => x.IsValid).OrderByDescending(x => x.Amount);
            var cartSubtotalReward = cartSubtotalRewards.FirstOrDefault(x => !string.IsNullOrEmpty(x.Coupon)) ?? cartSubtotalRewards.FirstOrDefault();
            if (cartSubtotalReward != null)
            {
                retVal.Rewards.Add(cartSubtotalReward);

                //Exlusive offer
                if (cartSubtotalReward.IsExclusive)
                {
                    var itemRewards = retVal.Rewards.OfType<CatalogItemAmountReward>().ToList();
                    for (var i = itemRewards.Count - 1; i >= 0; i--)
                    {
                        retVal.Rewards.Remove(itemRewards[i]);
                    }
                }
            }
            var potentialCartSubtotalRewards = rewards.OfType<CartSubtotalReward>().Where(x => !x.IsValid).OrderByDescending(x => x.Amount);
            var potentialCartSubtotalReward = potentialCartSubtotalRewards.FirstOrDefault(x => !string.IsNullOrEmpty(x.Coupon)) ?? cartSubtotalRewards.FirstOrDefault();
            if (potentialCartSubtotalReward != null && cartSubtotalReward != null)
            {
                if (potentialCartSubtotalReward.Amount > cartSubtotalReward.Amount)
                {
                    retVal.Rewards.Add(potentialCartSubtotalReward);
                }
            }
            else if (potentialCartSubtotalReward != null)
            {
                retVal.Rewards.Add(potentialCartSubtotalReward);
            }

            //Gifts
            rewards.OfType<GiftReward>().ToList().ForEach(x => retVal.Rewards.Add(x));

            //Special offer
            rewards.OfType<SpecialOfferReward>().ToList().ForEach(x => retVal.Rewards.Add(x));

            using (var repository = _marketingRepositoryFactory())
            {
                var couponReward = rewards.FirstOrDefault(r => r.Coupon != null);
                var coupon = repository.Coupons.FirstOrDefault(c => c.Code == couponReward.Coupon);
                var usesCount = repository.PromotionUsages.Where(pu => pu.CouponCode == coupon.Code && pu.PromotionId == couponReward.Promotion.Id).Count();
                if (coupon.ExpirationDate.HasValue)
                {
                    if (coupon.ExpirationDate.Value < DateTime.UtcNow)
                    {
                        var promoReservation = repository.PromotionUsages.FirstOrDefault(pu =>
                            pu.CouponCode == coupon.Code && pu.PromotionId == couponReward.Promotion.Id && string.IsNullOrEmpty(pu.OrderId));
                        if (promoReservation != null)
                        {
                            repository.Remove(promoReservation);
                            repository.UnitOfWork.Commit();
                        }
                    }
                }
                if (coupon.MaxUsesNumber < usesCount)
                {
                    couponReward.IsValid = false;
                }
            }

            return retVal;
        }

        public PromotionResult ProcessEvent(IMarketingEvent markertingEvent)
        {
            var retVal = new PromotionResult();
            var promotions = _promotionService.GetActivePromotions();
            foreach (var promotion in promotions)
            {
                var rewards = promotion.ProcessEvent(markertingEvent).Where(x => x != null);
                foreach (var promotionReward in rewards)
                {
                    retVal.Rewards.Add(promotionReward);
                }
            }

            return retVal;
        }

        #endregion

        private static AmountBasedReward[] EvaluteBestAmountRewards(decimal currentAmount, AmountBasedReward[] promoRewards)
        {
            var retVal = new List<AmountBasedReward>();

            var bestReward = EvaluateBestAmountReward(currentAmount, promoRewards.Where(x => x.IsValid).ToArray());
            if (bestReward != null)
            {
                retVal.Add(bestReward);
            }

            var bestPotentialReward = EvaluateBestAmountReward(currentAmount, promoRewards.Where(x => !x.IsValid).ToArray());

            if (bestPotentialReward != null && bestReward != null)
            {
                var bestRewardFromTwo = EvaluateBestAmountReward(currentAmount, new[] { bestReward, bestPotentialReward });
                if (bestRewardFromTwo == bestPotentialReward)
                {
                    retVal.Add(bestPotentialReward);
                }
            }
            else if (bestPotentialReward != null)
            {
                retVal.Add(bestPotentialReward);
            }

            return retVal.ToArray();
        }

        private static AmountBasedReward EvaluateBestAmountReward(decimal currentAmount, AmountBasedReward[] promoRewards)
        {
            AmountBasedReward retVal = null;
            var maxAbsoluteReward = promoRewards
                .Where(y => y.AmountType == RewardAmountType.Absolute)
                .OrderByDescending(y => y.Amount).FirstOrDefault();

            var maxRelativeReward = promoRewards
                .Where(y => y.AmountType == RewardAmountType.Relative)
                .OrderByDescending(y => y.Amount).FirstOrDefault();


            var absDiscountAmount = maxAbsoluteReward != null ? maxAbsoluteReward.Amount : 0;
            var relDiscountAmount = maxRelativeReward != null ? currentAmount * maxRelativeReward.Amount : 0;

            if (maxAbsoluteReward != null && maxRelativeReward != null)
            {
                if (absDiscountAmount > relDiscountAmount)
                {
                    retVal = maxAbsoluteReward;
                }
                else
                {
                    retVal = maxRelativeReward;
                }
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
