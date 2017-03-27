using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using VirtoCommerce.Domain.Common;
using VirtoCommerce.Domain.Marketing.Model;
using VirtoCommerce.Domain.Marketing.Model.Promotions.Search;
using VirtoCommerce.Domain.Marketing.Services;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Serialization;

namespace VirtoCommerce.MarketingModule.Data.Promotions
{
    public class DynamicPromotion : Promotion
    {
        private readonly ICouponService _couponService;
        private readonly IPromotionUsageService _usageService;
        public DynamicPromotion(IExpressionSerializer expressionSerializer, ICouponService couponService, IPromotionUsageService usageService)
        {
            this.ExpressionSerializer = expressionSerializer;
            _couponService  = couponService;
            _usageService = usageService;
        }      
        private Func<IEvaluationContext, bool> _condition;
        private PromotionReward[] _rewards;

        protected IExpressionSerializer ExpressionSerializer { get; set; }

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
            var couponIsValid = !HasCoupons || CheckCouponIsValid(promoContext.Coupon);

            //Evaluate reward for all promoEntry in context
            foreach (var promoEntry in promoContext.PromoEntries)
            {
                //Set current context promo entry for evaluation
                promoContext.PromoEntry = promoEntry;

                foreach (var reward in Rewards)
                {
                    var promoReward = reward.Clone();
                    EvaluateReward(promoContext, couponIsValid, promoReward);
                    result.Add(promoReward);
                }
            }

            return result.ToArray();
        }

        protected virtual void EvaluateReward(PromotionEvaluationContext promoContext, bool couponIsValid, PromotionReward reward)
        {
            reward.Promotion = this;
            reward.IsValid = couponIsValid && Condition(promoContext);
            //Add coupon to reward only for case when promotion contains associated coupons
            if (HasCoupons)
            {
                reward.Coupon = promoContext.Coupon;
            }
            //Set productId for catalog item reward
            var catalogItemReward = reward as CatalogItemAmountReward;
            if (catalogItemReward != null && catalogItemReward.ProductId == null)
            {
                catalogItemReward.ProductId = promoContext.PromoEntry.ProductId;
            }
        }  
        
        protected virtual bool CheckCouponIsValid(string couponCode)
        {
            Coupon coupon = null;
            var retVal = !string.IsNullOrEmpty(couponCode);
            if (retVal)
            {
                coupon = _couponService.SearchCoupons(new CouponSearchCriteria { Code = couponCode, PromotionId = Id }).Results.FirstOrDefault();
                retVal = coupon != null;
            }
            if (retVal && coupon.ExpirationDate != null)
            {
                retVal = coupon.ExpirationDate <= DateTime.UtcNow;
            }
            if (retVal && coupon.MaxUsesNumber > 0)
            {
                retVal = _usageService.SearchUsages(new PromotionUsageSearchCriteria { PromotionId = Id, CouponCode = couponCode, Take = 0 }).TotalCount <= coupon.MaxUsesNumber;
            }
            return retVal;
        } 
     
    }
}
