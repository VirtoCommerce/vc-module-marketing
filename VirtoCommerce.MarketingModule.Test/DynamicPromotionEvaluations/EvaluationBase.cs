using System.Linq;
using Moq;
using Newtonsoft.Json;
using VirtoCommerce.Domain.Common;
using VirtoCommerce.Domain.Marketing.Model;
using VirtoCommerce.Domain.Marketing.Services;
using VirtoCommerce.MarketingModule.Data.Promotions;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Serialization;
using VirtoCommerce.Platform.Data.Serialization;

namespace VirtoCommerce.MarketingModule.Test.DynamicPromotionEvaluations
{
    public abstract class EvaluationBase
    {
        protected IExpressionSerializer expressionSerializer = new XmlExpressionSerializer();
        private ICouponService couponService = new Mock<ICouponService>().Object;
        private IPromotionUsageService promotionUsageService = new Mock<IPromotionUsageService>().Object;

        protected DynamicPromotion GetDynamicPromotion(IConditionExpression[] conditions, IRewardExpression[] rewards)
        {
            var dynamicPromotion = new DynamicPromotion(expressionSerializer, couponService, promotionUsageService);
            dynamicPromotion.PredicateSerialized = GetPredicateSerialized(conditions);
            dynamicPromotion.RewardsSerialized = GetRewardsSerialized(rewards);

            return dynamicPromotion;
        }

        private string GetPredicateSerialized(IConditionExpression[] conditions)
        {
            var predicate = PredicateBuilder.False<IEvaluationContext>();
            foreach (var expression in conditions.Select(x => x.GetConditionExpression()))
            {
                predicate = predicate.Or(expression);
            }

            return expressionSerializer.SerializeExpression(predicate);
        }

        private string GetRewardsSerialized(IRewardExpression[] rewards)
        {
            var promotionRewards = rewards.SelectMany(r => r.GetRewards()).ToArray();
            return JsonConvert.SerializeObject(promotionRewards, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All });
        }
    }

    public class EvaluationResult
    {
        public int ValidCount { get; set; }
        public int InvalidCount { get; set; }
    }
}
