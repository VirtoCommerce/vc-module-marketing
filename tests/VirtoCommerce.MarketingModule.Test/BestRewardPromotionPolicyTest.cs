using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using VirtoCommerce.MarketingModule.Core.Model.Promotions;
using VirtoCommerce.MarketingModule.Core.Model.Promotions.Search;
using VirtoCommerce.MarketingModule.Core.Promotions;
using VirtoCommerce.MarketingModule.Core.Search;
using VirtoCommerce.MarketingModule.Core.Services;
using VirtoCommerce.MarketingModule.Data.Services;
using VirtoCommerce.Platform.Caching;
using VirtoCommerce.Platform.Core.Common;
using Xunit;

namespace VirtoCommerce.MarketingModule.Test
{
    [Trait("Category", "CI")]
    public class BestRewardPromotionPolicyTest
    {
        private const string promoGet_10_off = "Get 10% off on ProductA";
        private const string promoGet_15_offFor1of2 = "Get 15% off for every 1 of 2 on ProductA";
        private const string promoGet_25_offFor1of2 = "Get 25% off for every 1 of 2 on ProductA";

        [Fact]
        public void EvaluateRewards_ShippingMethodNotSpecified_Counted()
        {
            //Arrange            
            var evalPolicy = GetPromotionEvaluationPolicy(GetPromotions("FedEx Get 30% Off", "Any shipment 70% Off"));
            var productA = new ProductPromoEntry { ProductId = "ProductA", Price = 100, Quantity = 1 };
            var context = new PromotionEvaluationContext
            {
                ShipmentMethodCode = "FedEx",
                ShipmentMethodPrice = 100,
                PromoEntries = new[] { productA }
            };
            //Act
            var rewards = evalPolicy.EvaluatePromotionAsync(context).GetAwaiter().GetResult().Rewards.OfType<ShipmentReward>().ToList();

            //Assert
            Assert.Equal(2, rewards.Count);
            Assert.Equal(30m, rewards.FirstOrDefault(x => x.Promotion.Id.EqualsInvariant("FedEx Get 30% Off")).Amount);
            Assert.Equal(70m, rewards.FirstOrDefault(x => x.Promotion.Id.EqualsInvariant("Any shipment 70% Off")).Amount);
        }

        [Theory]
        [InlineData(1, 10, promoGet_10_off, promoGet_15_offFor1of2)]
        [InlineData(2, 10, promoGet_10_off, promoGet_15_offFor1of2)]
        [InlineData(2, 25, promoGet_10_off, promoGet_15_offFor1of2, promoGet_25_offFor1of2)]
        [InlineData(2, 15, promoGet_15_offFor1of2)]
        [InlineData(2, 25, promoGet_15_offFor1of2, promoGet_25_offFor1of2)]
        [InlineData(3, 10, promoGet_10_off, promoGet_25_offFor1of2)]
        [InlineData(4, 25, promoGet_10_off, promoGet_25_offFor1of2)]
        [InlineData(7, 25, promoGet_10_off, promoGet_25_offFor1of2)]
        public void EvaluateRewards_PickBestRelativeDiscount(int quantity, decimal expectedReward, params string[] promotions)
        {
            //Arrange            
            var evalPolicy = GetPromotionEvaluationPolicy(GetPromotions(promotions));
            var productA = new ProductPromoEntry { ProductId = "ProductA", Price = 100, Quantity = quantity };
            var context = new PromotionEvaluationContext
            {
                PromoEntries = new[] { productA }
            };
            //Act
            var rewards = evalPolicy.EvaluatePromotionAsync(context).GetAwaiter().GetResult().Rewards.OfType<CatalogItemAmountReward>().ToList();

            //Assert
            Assert.Single(rewards);
            Assert.Equal(expectedReward, rewards.First().Amount);
        }

        [Fact]
        public void EvaluatePromotion_GetBestPaymentReward()
        {
            //Arrange
            var blockReward = new BlockReward().WithChildrens(new RewardPaymentGetOfAbs() { Amount = 10m, PaymentMethod = "PayTest" });
            var dynamicPromotion = new DynamicPromotion
            {
                DynamicExpression = AbstractTypeFactory<PromotionConditionAndRewardTree>.TryCreateInstance()
            };
            dynamicPromotion.DynamicExpression.WithChildrens(blockReward);

            var evalPolicy = GetPromotionEvaluationPolicy(new[] { dynamicPromotion });
            var productA = new ProductPromoEntry { ProductId = "ProductA", Price = 100, Quantity = 1 };
            var context = new PromotionEvaluationContext
            {
                PaymentMethodCode = "PayTest",
                PaymentMethodPrice = 5m,
                PromoEntries = new[] { productA }
            };

            //Act
            var rewards = (await evalPolicy.EvaluatePromotionAsync(context)).Rewards.OfType<PaymentReward>().ToList();

            //Assert
            Assert.Equal(10m, rewards.First().Amount);
            Assert.True(rewards.First().IsValid);
        }

        private static IMarketingPromoEvaluator GetPromotionEvaluationPolicy(IEnumerable<Promotion> promotions)
        {
            var result = new PromotionSearchResult
            {
                Results = promotions.ToList()
            };

            var promoSearchServiceMock = new Mock<IPromotionSearchService>();
            promoSearchServiceMock.Setup(x => x.SearchPromotionsAsync(It.IsAny<PromotionSearchCriteria>())).ReturnsAsync(result);

            var memoryCache = new MemoryCache(Options.Create(new MemoryCacheOptions()));
            var platformMemoryCache = new PlatformMemoryCache(memoryCache, Options.Create(new CachingOptions()), new Mock<ILogger<PlatformMemoryCache>>().Object);

            return new BestRewardPromotionPolicy(promoSearchServiceMock.Object, platformMemoryCache);
        }

        private static IEnumerable<Promotion> TestPromotions
        {
            get
            {
                yield return new MockPromotion
                {
                    Id = "FedEx Get 30% Off",
                    Rewards = new[]
                   {
                        new ShipmentReward { ShippingMethod = "FedEx", Amount = 30, AmountType = RewardAmountType.Relative, IsValid = true  }
                    },
                    Priority = 2,
                    IsExclusive = false
                };
                yield return new MockPromotion
                {
                    Id = "Any shipment 70% Off",
                    Rewards = new[]
                   {
                        new ShipmentReward { ShippingMethod = null, Amount = 70, AmountType = RewardAmountType.Relative, IsValid = true  }
                    },
                    Priority = 2,
                    IsExclusive = false
                };
                yield return new MockPromotion
                {
                    Id = promoGet_10_off,
                    Rewards = new[]
                   {
                        new CatalogItemAmountReward { Amount = 10, ProductId = "ProductA"}
                    }
                };
                yield return new MockPromotion
                {
                    Id = promoGet_15_offFor1of2,
                    Rewards = new[]
                   {
                        new CatalogItemAmountReward { Amount = 15, ForNthQuantity = 1, InEveryNthQuantity = 2, ProductId = "ProductA"  }
                    },
                    Priority = 2
                };
                yield return new MockPromotion
                {
                    Id = promoGet_25_offFor1of2,
                    Rewards = new[]
                   {
                        new CatalogItemAmountReward { Amount = 25, ForNthQuantity = 1, InEveryNthQuantity = 2, ProductId = "ProductA"  }
                    },
                    Priority = 2
                };
            }
        }

        private static IEnumerable<Promotion> GetPromotions(params string[] ids)
        {
            return TestPromotions.Where(x => ids.Contains(x.Id));
        }
    }
}
