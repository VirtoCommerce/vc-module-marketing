using System.Collections.Generic;
using System.Linq;
using VirtoCommerce.Domain.Common;
using VirtoCommerce.Domain.Marketing.Model;
using VirtoCommerce.DynamicExpressionsModule.Data.Promotion;
using VirtoCommerce.MarketingModule.Data.Promotions;
using Xunit;

namespace VirtoCommerce.MarketingModule.Test.DynamicPromotionEvaluations.BlockCatalogCondition
{
    public class InStockQuantityConditionTests : EvaluationBase
    {
        [Theory]
        [MemberData(nameof(TestConditionDataGenerator.GetConditions), MemberType = typeof(TestConditionDataGenerator))]
        public void CheckPromotionValid(IConditionExpression[] conditions, IRewardExpression[] rewards, IEvaluationContext context, EvaluationResult evaluationResult)
        {
            DynamicPromotion dynamicPromotion = GetDynamicPromotion(conditions, rewards);

            var result = dynamicPromotion.EvaluatePromotion(context);

            Assert.Equal(evaluationResult.ValidCount, result.Count(r => r.IsValid));
            Assert.Equal(evaluationResult.InvalidCount, result.Count(r => !r.IsValid));
        }
    }

    public class TestConditionDataGenerator
    {
        public static IEnumerable<object[]> GetConditions()
        {
            yield return new object[]
            {
                new IConditionExpression[] { new ConditionInStockQuantity { Quantity = 10 } },
                new IRewardExpression[] { new RewardItemGetOfRel() },
                context,
                new EvaluationResult
                {
                    ValidCount = 2,
                    InvalidCount = 1
                }
            };

            yield return new object[]
            {
                new IConditionExpression[] { new ConditionInStockQuantity { Quantity = 10, Exactly = true } },
                new IRewardExpression[] { new RewardItemGetOfRel() },
                context,
                new EvaluationResult
                {
                    ValidCount = 1,
                    InvalidCount = 2
                }
            };

            yield return new object[]
            {
                new IConditionExpression[] 
                {
                    new ConditionInStockQuantity { Quantity = 12 },
                    new ConditionInStockQuantity { Quantity = 10, Exactly = true },
                    new ConditionInStockQuantity { Quantity = 7, Exactly = true }
                },
                new IRewardExpression[] { new RewardItemGetOfRel() },
                context,
                new EvaluationResult
                {
                    ValidCount = 2,
                    InvalidCount = 1
                }
            };
        }

        private static IEvaluationContext context = new PromotionEvaluationContext
        {
            PromoEntries = new List<ProductPromoEntry>
            {
                new ProductPromoEntry { InStockQuantity = 12 },
                new ProductPromoEntry { InStockQuantity = 10 },
                new ProductPromoEntry { InStockQuantity = 8 }
            }
        };
    }
}
