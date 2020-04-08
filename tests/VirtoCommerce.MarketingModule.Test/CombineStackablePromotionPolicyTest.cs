using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using Newtonsoft.Json;
using VirtoCommerce.CoreModule.Core.Common;
using VirtoCommerce.CoreModule.Core.Conditions;
using VirtoCommerce.MarketingModule.Core.Model.Promotions;
using VirtoCommerce.MarketingModule.Core.Model.Promotions.Search;
using VirtoCommerce.MarketingModule.Core.Promotions;
using VirtoCommerce.MarketingModule.Core.Search;
using VirtoCommerce.MarketingModule.Core.Services;
using VirtoCommerce.MarketingModule.Data.Promotions;
using VirtoCommerce.MarketingModule.Data.Services;
using VirtoCommerce.MarketingModule.Test.CustomPromotion;
using VirtoCommerce.MarketingModule.Test.CustomReward;
using VirtoCommerce.Platform.Core.Common;
using Xunit;

namespace VirtoCommerce.MarketingModule.Test
{
    [Trait("Category", "CI")]
    public class CombineStackablePromotionPolicyTest
    {
        [Fact]
        public void EvaluateRewards_CombineByPriorityOrder()
        {
            //Arrange            
            var evalPolicy = GetPromotionEvaluationPolicy(GetPromotions("FedEx Get 50% Off", "FedEx Get 30% Off", "ProductA and ProductB Get 2 With 50% Off", "Get ProductA With 25$ Off"));
            var productA = new ProductPromoEntry { ProductId = "ProductA", Price = 100, Quantity = 1 };
            var productB = new ProductPromoEntry { ProductId = "ProductB", Price = 100, Quantity = 3 };
            var context = new PromotionEvaluationContext
            {
                ShipmentMethodCode = "FedEx",
                ShipmentMethodPrice = 100,
                PromoEntries = new[] { productA, productB }
            };
            //Act
            var rewards = evalPolicy.EvaluatePromotionAsync(context).GetAwaiter().GetResult().Rewards;

            //Assert
            Assert.Equal(5, rewards.Count);
            Assert.Equal(35m, context.ShipmentMethodPrice);
            Assert.Equal(37.5m, productA.Price);
            Assert.Equal(66.66m, productB.Price);
        }

        [Fact]
        public void EvaluateRewards_OnlySingleExclusivePromotion()
        {
            //Arrange            
            var evalPolicy = GetPromotionEvaluationPolicy(TestPromotions);
            var productA = new ProductPromoEntry { ProductId = "ProductA", Price = 100, Quantity = 1 };
            var productB = new ProductPromoEntry { ProductId = "ProductB", Price = 100, Quantity = 1 };
            var context = new PromotionEvaluationContext
            {
                ShipmentMethodCode = "FedEx",
                ShipmentMethodPrice = 100,
                PromoEntries = new[] { productA, productB }
            };
            //Act
            var rewards = evalPolicy.EvaluatePromotionAsync(context).GetAwaiter().GetResult().Rewards;

            //Assert
            Assert.Single(rewards);
            Assert.Equal("Exclusive ProductB Get 10$ Off", rewards.Single().Promotion.Id);
        }

        [Fact]
        public void EvaluateRewards_SkipRewardsMakingPriceNegative()
        {
            //Arrange            
            var evalPolicy = GetPromotionEvaluationPolicy(GetPromotions("Get ProductA Free", "Get ProductA With 25$ Off"));
            var productA = new ProductPromoEntry { ProductId = "ProductA", Price = 100, Quantity = 1 };
            var context = new PromotionEvaluationContext
            {
                PromoEntries = new[] { productA }
            };
            //Act
            var rewards = evalPolicy.EvaluatePromotionAsync(context).GetAwaiter().GetResult().Rewards;

            //Assert
            Assert.Single(rewards);
            Assert.Equal("Get ProductA Free", rewards.Single().Promotion.Id);
            Assert.Equal(0, productA.Price);
        }

        [Fact]
        public void EvaluateRewards_ShippingMethodNotSpecified_Counted()
        {
            //Arrange            
            var evalPolicy = GetPromotionEvaluationPolicy(GetPromotions("Any shipment 50% Off"));
            var productA = new ProductPromoEntry { ProductId = "ProductA", Price = 100, Quantity = 1 };
            var context = new PromotionEvaluationContext
            {
                ShipmentMethodCode = "FedEx",
                ShipmentMethodPrice = 100,
                PromoEntries = new[] { productA }
            };
            //Act
            var rewards = evalPolicy.EvaluatePromotionAsync(context).GetAwaiter().GetResult().Rewards;

            //Assert
            Assert.Equal(1, rewards.Count);
            Assert.Equal(50m, context.ShipmentMethodPrice);
            Assert.Equal(100m, productA.Price);
        }

        [Fact]
        public void EvaluateRewards_BuyProductWithTag_Counted()
        {
            //Arrange 
            var evalPolicy = GetPromotionEvaluationPolicy(new List<BuyProductWithTagPromotion>()
            {
                new BuyProductWithTagPromotion(new [] {"tag1"}, 10)
            });
            var productA = new ProductPromoEntry { ProductId = "ProductA", Price = 100, Quantity = 1, Attributes = new Dictionary<string, string> { { "tag", "tag1" } } };
            var context = new PromotionEvaluationContext
            {
                PromoEntries = new[] { productA }
            };

            //Act
            var rewards = evalPolicy.EvaluatePromotionAsync(context).GetAwaiter().GetResult().Rewards;

            //Assert
            Assert.Equal(1, rewards.Count);
        }

        [Fact]
        public void EvaluateRewards_DynamicPromotions()
        {
            //Arrange 
            var couponSearchMockServiceMock = new Mock<ICouponSearchService>();
            var promotionUsageSearchMock = new Mock<IPromotionUsageSearchService>();
            var promoConditionTree = "{\"AvailableChildren\":null,\"Children\":[{\"All\":false,\"Not\":false,\"AvailableChildren\":null,\"Children\":[{\"AvailableChildren\":null,\"Children\":[],\"Id\":\"ConditionIsRegisteredUser\"}],\"Id\":\"BlockCustomerCondition\"},{\"All\":false,\"Not\":false,\"AvailableChildren\":null,\"Children\":[],\"Id\":\"BlockCatalogCondition\"},{\"All\":false,\"Not\":false,\"AvailableChildren\":null,\"Children\":[{\"NumItem\":10,\"NumItemSecond\":13,\"ProductId\":null,\"ProductName\":null,\"CompareCondition\":\"Between\",\"AvailableChildren\":null,\"Children\":[],\"Id\":\"ConditionAtNumItemsInCart\"},{\"SubTotal\":0.0,\"SubTotalSecond\":100.0,\"ExcludingCategoryIds\":[],\"ExcludingProductIds\":[],\"CompareCondition\":\"AtLeast\",\"AvailableChildren\":null,\"Children\":[],\"Id\":\"ConditionCartSubtotalLeast\"}],\"Id\":\"BlockCartCondition\"},{\"AvailableChildren\":null,\"Children\":[{\"Amount\":15.0,\"AvailableChildren\":null,\"Children\":[],\"Id\":\"RewardCartGetOfAbsSubtotal\"}],\"Id\":\"BlockReward\"}],\"Id\":\"PromotionConditionAndRewardTree\"}";

            AbstractTypeFactory<Promotion>.RegisterType<DynamicPromotion>().WithSetupAction((promotion) =>
            {
                var dynamicPromotion = promotion as DynamicPromotion;
                dynamicPromotion.CouponSearchService = couponSearchMockServiceMock.Object;
                dynamicPromotion.PromotionUsageSearchService = promotionUsageSearchMock.Object;
                dynamicPromotion.DynamicExpression = AbstractTypeFactory<PromotionConditionAndRewardTree>.TryCreateInstance();
                dynamicPromotion.DynamicExpression.Children = dynamicPromotion.DynamicExpression.AvailableChildren.ToList();
            });

            foreach (var conditionTree in AbstractTypeFactory<PromotionConditionAndRewardTreePrototype>.TryCreateInstance().Traverse<IConditionTree>(x => x.AvailableChildren))
            {
                AbstractTypeFactory<IConditionTree>.RegisterType(conditionTree.GetType());
            }

            var evalPolicy = GetPromotionEvaluationPolicy(new List<Promotion> { new DynamicPromotion
            {
                CouponSearchService = couponSearchMockServiceMock.Object,
                PromotionUsageSearchService = promotionUsageSearchMock.Object,
                DynamicExpression = JsonConvert.DeserializeObject<PromotionConditionAndRewardTree>(promoConditionTree, new ConditionJsonConverter(), new RewardJsonConverter())
            } });

            var context = new PromotionEvaluationContext()
            {
                IsRegisteredUser = true,
                IsEveryone = true,
                Currency = "usd",
                PromoEntries = new List<ProductPromoEntry> { new ProductPromoEntry() { ProductId = "1" } },
                CartPromoEntries = new List<ProductPromoEntry> { new ProductPromoEntry { Quantity = 11, Price = 5 } }
            };

            //Act
            var rewards = evalPolicy.EvaluatePromotionAsync(context).GetAwaiter().GetResult().Rewards;

            //Assert
            Assert.Equal(1, rewards.Count);
        }

        [Theory]
        [InlineData(45, 2, 36, 0)]
        [InlineData(40, 1, 32, 5)]
        public async Task EvaluateRewards_PromotionRewardEvaluation_ConditionCalculatesBasedOnChangedPrice(decimal initialPrice, int expectedRewardsApplied, decimal expectedPrice, decimal expectedShipmentPrice)
        {
            //Arrange
            var evalPolicy = GetPromotionEvaluationPolicy(GetPromotions("Register and First time buyer => get 20% off.", "Free shipping on orders totaling $35.00 or more."));
            var productA = new ProductPromoEntry { ProductId = "ProductA", Price = initialPrice, Quantity = 1 };
            var context = new PromotionEvaluationContext
            {
                ShipmentMethodCode = "FedEx",
                ShipmentMethodPrice = 5,
                PromoEntries = new[] { productA }
            };
            //Act
            var rewards = (await evalPolicy.EvaluatePromotionAsync(context)).Rewards;

            //Assert
            Assert.Equal(expectedRewardsApplied, rewards.Count);
            Assert.Equal(expectedShipmentPrice, context.ShipmentMethodPrice);
            Assert.Equal(expectedPrice, productA.Price);
        }

        [Fact]
        public async Task EvaluateRewards_PromotionRewardEvaluation_CalledAgainAfterPossibleDiscountAffectingCartTotal()
        {
            //Arrange
            var promotionRewardEvaluatorMock = GetPromotionRewardEvaluatorMock();
            var evalPolicy = GetPromotionEvaluationPolicy(GetPromotions("Register and First time buyer => get 20% off.", "Free shipping on orders totaling $35.00 or more."), promotionRewardEvaluatorMock);
            var productA = new ProductPromoEntry { ProductId = "ProductA", Price = 45, Quantity = 1 };
            var context = new PromotionEvaluationContext
            {
                ShipmentMethodCode = "FedEx",
                ShipmentMethodPrice = 5,
                PromoEntries = new[] { productA }
            };
            //Act
            var rewards = (await evalPolicy.EvaluatePromotionAsync(context)).Rewards;

            //Assert
            promotionRewardEvaluatorMock.Verify(x => x.GetOrderedValidRewardsAsync(It.IsAny<IEnumerable<Promotion>>(), It.IsAny<IEvaluationContext>()), Times.Exactly(2));
            Assert.Equal(2, rewards.Count);
        }

        [Fact]
        public async Task EvaluateRewards_PromotionRewardEvaluation_CalledOnceWhenNoPossibleDiscountsAffectingCartTotal()
        {
            //Arrange
            var promotionRewardEvaluatorMock = GetPromotionRewardEvaluatorMock();
            var evalPolicy = GetPromotionEvaluationPolicy(GetPromotions("Any shipment 50% Off", "Free shipping on orders totaling $35.00 or more."), promotionRewardEvaluatorMock);
            var productA = new ProductPromoEntry { ProductId = "ProductA", Price = 45, Quantity = 1 };
            var context = new PromotionEvaluationContext
            {
                ShipmentMethodCode = "FedEx",
                ShipmentMethodPrice = 5,
                PromoEntries = new[] { productA }
            };
            //Act
            var rewards = (await evalPolicy.EvaluatePromotionAsync(context)).Rewards;

            //Assert
            promotionRewardEvaluatorMock.Verify(x => x.GetOrderedValidRewardsAsync(It.IsAny<IEnumerable<Promotion>>(), It.IsAny<IEvaluationContext>()), Times.Once);
            Assert.Equal(2, rewards.Count);
        }

        [Fact]
        public async Task EvaluateRewards_PromotionRewardEvaluation_CalledOnceOnTwoDifferentTypeRewardsInOnePromotion()
        {
            //Arrange
            var promotionRewardEvaluatorMock = GetPromotionRewardEvaluatorMock();
            var evalPolicy = GetPromotionEvaluationPolicy(GetPromotions("Register and First time buyer => get 20% off and free shipping."), promotionRewardEvaluatorMock);
            var productA = new ProductPromoEntry { ProductId = "ProductA", Price = 45, Quantity = 1 };
            var context = new PromotionEvaluationContext
            {
                ShipmentMethodCode = "FedEx",
                ShipmentMethodPrice = 5,
                PromoEntries = new[] { productA }
            };
            //Act
            var rewards = (await evalPolicy.EvaluatePromotionAsync(context)).Rewards;

            //Assert
            promotionRewardEvaluatorMock.Verify(x => x.GetOrderedValidRewardsAsync(It.IsAny<IEnumerable<Promotion>>(), It.IsAny<IEvaluationContext>()), Times.Once);
            Assert.Equal(2, rewards.Count);
        }

        [Fact]
        public async Task EvaluateRewards_SpecificPaymentMethod_Applied()
        {
            //Arrange            
            var evalPolicy = GetPromotionEvaluationPolicy(GetPromotions("For MasterCard Payment method => get 10% off for payment method"));
            var productA = new ProductPromoEntry { ProductId = "ProductA", Price = 100, Quantity = 1 };
            var context = new PromotionEvaluationContext
            {
                PaymentMethodCode = "MasterCard",
                PaymentMethodPrice = 100,
                PromoEntries = new[] { productA },
            };

            //Act
            await evalPolicy.EvaluatePromotionAsync(context);

            //Assert
            Assert.Equal(90m, context.PaymentMethodPrice);
            Assert.Equal(100m, productA.Price);
        }

        [Fact]
        public async Task EvaluateRewards_SpecificPaymentMethod_NotApplied()
        {
            //Arrange            
            var evalPolicy = GetPromotionEvaluationPolicy(GetPromotions("For MasterCard Payment method => get 10% off for payment method"));
            var productA = new ProductPromoEntry { ProductId = "ProductA", Price = 100, Quantity = 1 };
            var context = new PromotionEvaluationContext
            {
                PaymentMethodCode = "AuthorizeNet",
                PaymentMethodPrice = 100,
                PromoEntries = new[] { productA },
            };
            //Act
            await evalPolicy.EvaluatePromotionAsync(context);

            //Assert
            Assert.Equal(100m, context.PaymentMethodPrice);
            Assert.Equal(100m, productA.Price);
        }

        [Theory]
        [InlineData("MasterCard")]
        [InlineData("")]
        [InlineData(null)]
        public async Task EvaluateRewards_PaymentMethodNotSpecified_AppliedForAnyProductPaymentMethod(string productPaymentMethod)
        {
            //Arrange            
            var evalPolicy = GetPromotionEvaluationPolicy(GetPromotions("For Any Payment method => get 10% off for payment method"));
            var productA = new ProductPromoEntry { ProductId = "ProductA", Price = 100, Quantity = 1 };
            var context = new PromotionEvaluationContext
            {
                PaymentMethodCode = productPaymentMethod,
                PaymentMethodPrice = 100,
                PromoEntries = new[] { productA },
            };
            //Act
            await evalPolicy.EvaluatePromotionAsync(context);

            //Assert
            Assert.Equal(90m, context.PaymentMethodPrice);
            Assert.Equal(100m, productA.Price);
        }

        [Fact]
        public void EvaluateRewards_NonHandledReward_Throws()
        {
            //Arrange            
            var evalPolicy = GetPromotionEvaluationPolicy(GetPromotions("Reward non handled by the policy"));
            var productA = new ProductPromoEntry { ProductId = "ProductA", Price = 100, Quantity = 1 };
            var context = new PromotionEvaluationContext
            {
                PromoEntries = new[] { productA },
            };

            //Act

            //Assert
            Assert.ThrowsAsync<NotSupportedException>(() => evalPolicy.EvaluatePromotionAsync(context));
        }

        private static IMarketingPromoEvaluator GetPromotionEvaluationPolicy(IEnumerable<Promotion> promotions, Mock<IPromotionRewardEvaluator> promotionRewardEvaluatorMock = null)
        {
            var result = new PromotionSearchResult
            {
                Results = promotions.ToList()
            };
            var promoSearchServiceMock = new Moq.Mock<IPromotionSearchService>();
            promoSearchServiceMock.Setup(x => x.SearchPromotionsAsync(It.IsAny<PromotionSearchCriteria>())).ReturnsAsync(result);

            if (promotionRewardEvaluatorMock == null)
            {
                promotionRewardEvaluatorMock = GetPromotionRewardEvaluatorMock();
            }

            return new CombineStackablePromotionPolicy(promoSearchServiceMock.Object, promotionRewardEvaluatorMock.Object);
        }

        private static Mock<IPromotionRewardEvaluator> GetPromotionRewardEvaluatorMock()
        {
            var result = new Mock<IPromotionRewardEvaluator>();

            result.Setup(x => x.GetOrderedValidRewardsAsync(It.IsAny<IEnumerable<Promotion>>(), It.IsAny<IEvaluationContext>()))
                .Returns<IEnumerable<Promotion>, IEvaluationContext>(
                    async (promotions, context) => await new DefaultPromotionRewardEvaluator().GetOrderedValidRewardsAsync(promotions, context)
                );

            return result;
        }

        private static IEnumerable<Promotion> TestPromotions
        {
            get
            {
                yield return new MockPromotion
                {
                    Id = "FedEx Get 50% Off",
                    Rewards = new[]
                    {
                        new ShipmentReward { ShippingMethod = "FedEx", Amount = 50, AmountType = RewardAmountType.Relative, IsValid = true }
                    },
                    Priority = 1,
                    IsExclusive = false
                };
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
                    Id = "Any shipment 50% Off",
                    Rewards = new[]
                   {
                        new ShipmentReward { ShippingMethod = null, Amount = 50, AmountType = RewardAmountType.Relative, IsValid = true  }
                    },
                    Priority = 2,
                    IsExclusive = false
                };
                yield return new MockPromotion
                {
                    Id = "Exclusive ProductB Get 10$ Off",
                    Rewards = new[]
                   {
                        new CatalogItemAmountReward { ProductId = "ProductB", Amount = 10, AmountType = RewardAmountType.Absolute, IsValid = true }
                    },
                    Priority = 10,
                    IsExclusive = true
                };
                yield return new MockPromotion
                {
                    Id = "Get ProductA Free",
                    Rewards = new[]
                    {
                       new CatalogItemAmountReward { ProductId = "ProductA", Amount = 100, AmountType = RewardAmountType.Relative, IsValid = true },
                    },
                    Priority = 100,
                    IsExclusive = false
                };
                yield return new MockPromotion
                {
                    Id = "Get ProductA With 25$ Off",
                    Rewards = new[]
                    {
                       new CatalogItemAmountReward { ProductId = "ProductA", Amount = 25, AmountType = RewardAmountType.Absolute, IsValid = true },
                    },
                    Priority = 80,
                    IsExclusive = false
                };
                yield return new MockPromotion
                {
                    Id = "ProductA and ProductB Get 2 With 50% Off",
                    Rewards = new[]
                    {
                       new CatalogItemAmountReward { ProductId = "ProductA", Amount = 50, Quantity = 2, AmountType = RewardAmountType.Relative, IsValid = true },
                       new CatalogItemAmountReward { ProductId = "ProductB", Amount = 50, Quantity = 2, AmountType = RewardAmountType.Relative, IsValid = true}
                    },
                    Priority = 15,
                    IsExclusive = false
                };
                yield return new MockPromotion
                {
                    Id = "Buy Order with 55% Off",
                    Rewards = new[]
                    {
                       new CartSubtotalReward {  Amount = 55, IsValid = true }
                    },
                    Priority = 20,
                    IsExclusive = false
                };
                yield return new MockPromotion
                {
                    Id = "Get Gift",
                    Rewards = new[]
                    {
                       new GiftReward {  ProductId = "ProductA", IsValid = true }
                    },
                    Priority = 0,
                    IsExclusive = false
                };
                yield return new MockPromotion
                {
                    Id = "Register and First time buyer => get 20% off.",
                    Rewards = new[]
                    {
                       new CatalogItemAmountReward { ProductId = "ProductA", Amount = 20, AmountType = RewardAmountType.Relative, IsValid = true }
                    },
                    Priority = 10,
                    IsExclusive = false
                };
                yield return new MockPromotion
                {
                    Id = "Free shipping on orders totaling $35.00 or more.",
                    Condition = x => (x as PromotionEvaluationContext)?.PromoEntries.Sum(entry => entry.Price * entry.Quantity) >= 35,
                    Rewards = new[]
                    {
                        new ShipmentReward { ShippingMethod = "FedEx", Amount = 100, AmountType = RewardAmountType.Relative, IsValid = true  }
                    },
                    Priority = 2,
                    IsExclusive = false
                };
                yield return new MockPromotion
                {
                    Id = "Register and First time buyer => get 20% off and free shipping.",
                    Rewards = new PromotionReward[]
                    {
                       new CatalogItemAmountReward { ProductId = "ProductA", Amount = 20, AmountType = RewardAmountType.Relative, IsValid = true },
                       new ShipmentReward { ShippingMethod = null, Amount = 100, AmountType = RewardAmountType.Relative, IsValid = true  }
                    },
                    Priority = 0,
                    IsExclusive = false
                };
                yield return new MockPromotion
                {
                    Id = "For MasterCard Payment method => get 10% off for payment method",
                    Rewards = new PromotionReward[]
                    {
                        new PaymentReward { PaymentMethod = "MasterCard", Amount = 10, AmountType = RewardAmountType.Relative, IsValid = true },
                    },
                    Priority = 0,
                    IsExclusive = false
                };
                yield return new MockPromotion
                {
                    Id = "For Any Payment method => get 10% off for payment method",
                    Rewards = new PromotionReward[]
                    {
                        new PaymentReward { PaymentMethod = null, Amount = 10, AmountType = RewardAmountType.Relative, IsValid = true },
                    },
                    Priority = 0,
                    IsExclusive = false
                };
                yield return new MockPromotion
                {
                    Id = "Reward non handled by the policy",
                    Rewards = new PromotionReward[]
                    {
                        new NonHandledReward { IsValid = true },
                    },
                    Priority = 0,
                    IsExclusive = false
                };
            }
        }

        private static IEnumerable<Promotion> GetPromotions(params string[] ids)
        {
            return TestPromotions.Where(x => ids.Contains(x.Id));
        }
    }

    internal class MockPromotion : Promotion
    {
        public IEnumerable<PromotionReward> Rewards { get; set; }

        public Func<IEvaluationContext, bool> Condition { get; set; }

        public override Task<PromotionReward[]> EvaluatePromotionAsync(IEvaluationContext context)
        {
            foreach (var reward in Rewards)
            {
                reward.Promotion = this;
                reward.IsValid = Condition == null || Condition(context);
            }

            return Task.FromResult(Rewards.ToArray());
        }
    }
}
