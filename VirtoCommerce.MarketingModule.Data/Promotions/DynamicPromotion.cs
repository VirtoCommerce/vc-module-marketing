using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using VirtoCommerce.Domain.Common;
using VirtoCommerce.Domain.Marketing.Model;
using VirtoCommerce.Domain.Marketing.Services;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Serialization;

namespace VirtoCommerce.MarketingModule.Data.Promotions
{
    public class DynamicPromotion : Promotion
    {
        public static DynamicPromotion CreateInstance(IExpressionSerializer expressionSerializer, ICouponService couponservice)
        {
            var result = AbstractTypeFactory<DynamicPromotion>.TryCreateInstance();
            result.ExpressionSerializer = expressionSerializer;
            result.CouponService = couponservice;
            return result;
        }

        private Func<IEvaluationContext, bool> _condition;
        private PromotionReward[] _rewards;
        //private ICouponService _couponService;

        protected IExpressionSerializer ExpressionSerializer { get; set; }
        protected ICouponService CouponService { get; set; }

        public string PredicateSerialized { get; set; }
        public string PredicateVisualTreeSerialized { get; set; }
        public string RewardsSerialized { get; set; }

        protected Func<IEvaluationContext, bool> Condition => _condition ?? (_condition = ExpressionSerializer.DeserializeExpression<Func<IEvaluationContext, bool>>(PredicateSerialized));
        protected PromotionReward[] Rewards => _rewards ?? (_rewards = JsonConvert.DeserializeObject<PromotionReward[]>(RewardsSerialized, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All }));

        public override PromotionReward[] EvaluatePromotion(IEvaluationContext context)
        {
            var result = new List<PromotionReward>();

            var promoContext = context as PromotionEvaluationContext;
            if (promoContext == null)
            {
                throw new ArgumentException("context should be PromotionEvaluationContext");
            }

            //Check coupon
            //var couponIsValid = Coupons == null ||
            //                    !Coupons.Any() ||
            //                    Coupons.Any(x => string.Equals(x.Code, promoContext.Coupon, StringComparison.InvariantCultureIgnoreCase));

            //Evaluate reward for all promoEntry in context
            foreach (var promoEntry in promoContext.PromoEntries)
            {
                //Set current context promo entry for evaluation
                promoContext.PromoEntry = promoEntry;

                foreach (var reward in Rewards)
                {
                    var promoReward = reward.Clone();
                    EvaluateReward(promoContext, promoReward);
                    result.Add(promoReward);
                }
            }

            return result.ToArray();
        }

        public override PromotionReward[] ProcessEvent(IMarketingEvent marketingEvent)
        {
            return null;
        }


        protected virtual void EvaluateReward(PromotionEvaluationContext promoContext, PromotionReward reward)
        {
            reward.Promotion = this;
            reward.IsValid = Condition(promoContext);

            if (!string.IsNullOrEmpty(promoContext.Coupon))
            {
                reward.IsValid = CouponService.CheckCoupon(promoContext.Coupon, this.Id);
            }

            //Set productId for catalog item reward
            var catalogItemReward = reward as CatalogItemAmountReward;
            if (catalogItemReward != null && catalogItemReward.ProductId == null)
            {
                catalogItemReward.ProductId = promoContext.PromoEntry.ProductId;
            }
        }
    }
}
