using System;
using System.Collections.Generic;
using System.Linq;
using VirtoCommerce.Domain.Common;
using VirtoCommerce.Domain.Marketing.Model;
using VirtoCommerce.DynamicExpressionsModule.Data.Promotion;
using VirtoCommerce.MarketingModule.Data.Promotions;
using Xunit;

namespace VirtoCommerce.MarketingModule.Test.DynamicPromotionEvaluations.BlockCatalogCondition
{
    public class ConditionCategoryIsTests : EvaluationBase
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

        public class TestConditionDataGenerator
        {
            private static string CategoryId = Guid.NewGuid().ToString();
            private static string ProductId = Guid.NewGuid().ToString();

            public static IEnumerable<object[]> GetConditions()
            {
                yield return new object[]
                {
                    new IConditionExpression[] { new ConditionCategoryIs { CategoryId = CategoryId, ExcludingProductIds = new[] { ProductId } } },
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
                    new IConditionExpression[] { new ConditionCategoryIs { CategoryId = CategoryId } },
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
                    new IConditionExpression[] { new ConditionCategoryIs { CategoryId = Guid.NewGuid().ToString() } },
                    new IRewardExpression[] { new RewardItemGetOfRel() },
                    context,
                    new EvaluationResult
                    {
                        ValidCount = 0,
                        InvalidCount = 3
                    }
                };
            }

            private static IEvaluationContext context = new PromotionEvaluationContext
            {
                PromoEntries = new List<ProductPromoEntry>
                {
                    new ProductPromoEntry { CategoryId = CategoryId, ProductId = Guid.NewGuid().ToString() },
                    new ProductPromoEntry { CategoryId = CategoryId, ProductId = ProductId },
                    new ProductPromoEntry { CategoryId = Guid.NewGuid().ToString(), ProductId = Guid.NewGuid().ToString() }
                }
            };
        }
    }
}
