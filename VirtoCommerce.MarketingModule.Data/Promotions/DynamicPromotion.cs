using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using VirtoCommerce.Domain.Common;
using VirtoCommerce.Domain.Marketing.Model;
using VirtoCommerce.Platform.Core.Serialization;

namespace VirtoCommerce.MarketingModule.Data.Promotions
{
    public class DynamicPromotion : Promotion
    {
        private readonly IExpressionSerializer _expressionSerializer;
        private Func<IEvaluationContext, bool> _condition;
        private PromotionReward[] _rewards;

        public DynamicPromotion(IExpressionSerializer expressionSerializer)
        {
            _expressionSerializer = expressionSerializer;
        }

        public string PredicateSerialized { get; set; }
        public string PredicateVisualTreeSerialized { get; set; }
        public string RewardsSerialized { get; set; }

        private Func<IEvaluationContext, bool> Condition
        {
            get { return _condition ?? (_condition = _expressionSerializer.DeserializeExpression<Func<IEvaluationContext, bool>>(PredicateSerialized)); }
        }

        private PromotionReward[] Rewards
        {
            get { return _rewards ?? (_rewards = JsonConvert.DeserializeObject<PromotionReward[]>(RewardsSerialized, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All })); }
        }

        public override PromotionReward[] EvaluatePromotion(IEvaluationContext context)
        {
            var retVal = new List<PromotionReward>();
            var promoContext = context as PromotionEvaluationContext;
            if (promoContext == null)
            {
                throw new ArgumentException("context should be PromotionEvaluationContext");
            }

            //Check coupon
            var couponValid = Coupons == null || !Coupons.Any() || Coupons.Any(x => string.Equals(x, promoContext.Coupon, StringComparison.InvariantCultureIgnoreCase));

            //Evaluate reward for all promoEntry in context
            foreach (var promoEntry in promoContext.PromoEntries)
            {
                //Set current context promo entry for evaluation
                promoContext.PromoEntry = promoEntry;
                foreach (var reward in Rewards.Select(x => x.Clone()))
                {
                    reward.Promotion = this;
                    reward.IsValid = couponValid && Condition(promoContext);
                    var catalogItemReward = reward as CatalogItemAmountReward;
                    //Set productId for catalog item reward
                    if (catalogItemReward != null && catalogItemReward.ProductId == null)
                    {
                        catalogItemReward.ProductId = promoEntry.ProductId;
                    }
                    retVal.Add(reward);
                }
            }
            return retVal.ToArray();
        }

        public override PromotionReward[] ProcessEvent(IMarketingEvent marketingEvent)
        {
            return null;
        }

    }
}
