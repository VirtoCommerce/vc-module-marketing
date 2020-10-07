using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using VirtoCommerce.CoreModule.Core.Conditions;
using VirtoCommerce.MarketingModule.Core.Model.Promotions;
using VirtoCommerce.MarketingModule.Core.Model.Promotions.Conditions;
using VirtoCommerce.MarketingModule.Core.Model.Promotions.Search;
using VirtoCommerce.MarketingModule.Core.Promotions;
using VirtoCommerce.MarketingModule.Core.Search;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Testing;
using Xunit;

namespace VirtoCommerce.MarketingModule.Test
{
    [Trait("Category", "CI")]
    public class DynamicPromotionTests
    {
        [Theory]
        [InlineData(9, 10, 0)]
        [InlineData(10, 10, 0)]
        [InlineData(11, 10, 1)]
        public async void FindValidCoupon_UsesNumber(int maxUsesNumber, int totalUses, int expectedCouponsCount)
        {
            //Arrange
            var testCoupon = new Coupon() { Id = "1", Code = "1", MaxUsesNumber = maxUsesNumber, };

            var dynamicPromotion = CreateDynamicPromotion(totalUses, testCoupon);

            //Act
            var validCoupons = await dynamicPromotion.FindValidCouponsAsync(new List<string>() { "any coupon" }, null);

            //Assert
            Assert.Equal(expectedCouponsCount, validCoupons.Count());
        }

        [Theory]
        [InlineData(9, 10, 0, "userId")]
        [InlineData(10, 10, 0, "userId")]
        [InlineData(11, 10, 1, "userId")]
        public async void FindValidCoupon_UsesNumberWithUserId(int maxUsesNumber, int totalUses, int expectedCouponsCount,
            string userId)
        {
            //Arrange
            var testCoupon = new Coupon() { Id = "1", Code = "1", MaxUsesNumber = maxUsesNumber, };

            var dynamicPromotion = CreateDynamicPromotion(totalUses, testCoupon);

            //Act
            var validCouponsWithUserId = await dynamicPromotion.FindValidCouponsAsync(new List<string>() { "any coupon" }, userId);

            //Assert
            Assert.Equal(expectedCouponsCount, validCouponsWithUserId.Count());
        }

        public static IEnumerable<object[]> ExpirationDateData =>
            new List<object[]>
            {
                new object[] {DateTime.UtcNow.AddDays(-1), 0}, new object[] {DateTime.UtcNow.AddDays(1), 1}
            };

        [Theory]
        [MemberData(nameof(ExpirationDateData))]
        public async void FindValidCoupon_ExpirationDate(DateTime expirationDate, int expectedCouponsCount)
        {
            //Arrange
            var testCoupon = new Coupon() { Id = "1", Code = "1", ExpirationDate = expirationDate };

            var dynamicPromotion = CreateDynamicPromotion(0, testCoupon);

            //Act
            var validCoupons = await dynamicPromotion.FindValidCouponsAsync(new List<string>() { "any coupon" }, null);

            //Assert
            Assert.Equal(expectedCouponsCount, validCoupons.Count());
        }

        [Fact]
        public void DynamicPromotion_Clone()
        {

            var blockCustomer = new BlockCustomerCondition()
                    .WithAvailConditions(
                        new ConditionIsRegisteredUser(),
                        new ConditionIsEveryone(),
                        new ConditionIsFirstTimeBuyer(),
                        new UserGroupsContainsCondition() { Group = "11" }
                     );
            var blockCatalog = new BlockCatalogCondition()
                    .WithAvailConditions(
                        new ConditionCategoryIs() { CategoryId = "11", CategoryName = "", ExcludingCategoryIds = new string[] { "1", "2" }, ExcludingProductIds = new string[] { "3", "4" } },
                        new ConditionCodeContains() { Keyword = "keyword" },
                        new ConditionCurrencyIs() { Currency = "USD" },
                        new ConditionEntryIs() { ProductIds = new string[] { "1", "2" }, ProductNames = new string[] { "name1", "name2" } },
                        new ConditionInStockQuantity() { CompareCondition = "CND", Quantity = 111, QuantitySecond = 222 },
                        new ConditionHasNoSalePrice()
                     );
            var blockCart = new BlockCartCondition()
                    .WithAvailConditions(
                        new ConditionAtNumItemsInCart() { CompareCondition = "CND", ExcludingCategoryIds = new string[] { "1", "2" }, ExcludingProductIds = new string[] { "3", "4" }, NumItem = 111, NumItemSecond = 222 },
                        new ConditionAtNumItemsInCategoryAreInCart() { CategoryId = "catid", CategoryName = "catname", CompareCondition = "CND", ExcludingCategoryIds = new string[] { "1", "2" }, ExcludingProductIds = new string[] { "3", "4" }, NumItem = 111, NumItemSecond = 222 },
                        new ConditionAtNumItemsOfEntryAreInCart() { CompareCondition = "CND", NumItem = 111, NumItemSecond = 222, ProductId = "Id", ProductName = "Name" },
                        new ConditionCartSubtotalLeast() { CompareCondition = "CND", ExcludingCategoryIds = new string[] { "1", "2" }, ExcludingProductIds = new string[] { "3", "4" }, SubTotal = 111, SubTotalSecond = 222 }
                     );
            var blockReward = new BlockReward()
                    .WithAvailConditions(
                      new RewardCartGetOfAbsSubtotal() { Amount = 444 },
                      new RewardCartGetOfRelSubtotal() { Amount = 444, MaxLimit = 555 },
                      new RewardItemGetFreeNumItemOfProduct() { NumItem = 55, ProductId = "Id", ProductName = "Name" },
                      new RewardItemGetOfAbs() { Amount = 444, ProductId = "Id", ProductName = "Name" },
                      new RewardItemGetOfAbsForNum() { Amount = 444, ProductId = "Id", ProductName = "Name", NumItem = 23 },
                      new RewardItemGetOfRel() { Amount = 444, ProductId = "Id", ProductName = "Name", MaxLimit = 23 },
                      new RewardItemGetOfRelForNum() { Amount = 444, ProductId = "Id", ProductName = "Name", MaxLimit = 23, NumItem = 32 },
                      new RewardItemGiftNumItem() { CategoryId = "catid", CategoryName = "catname", Description = "description", ProductId = "productId", ProductName = "ptoductName", Name = "Name", ImageUrl = "url:\\", MeasureUnit = "px", Quantity = 33 },
                      new RewardShippingGetOfAbsShippingMethod() { Amount = 444, ShippingMethod = "shipMethod" },
                      new RewardShippingGetOfRelShippingMethod() { Amount = 444, ShippingMethod = "shipMethod", MaxLimit = 22 },
                      new RewardPaymentGetOfAbs() { Amount = 444, PaymentMethod = "payMethod" },
                      new RewardPaymentGetOfRel() { Amount = 444, PaymentMethod = "payMethod", MaxLimit = 22 },
                      new RewardItemForEveryNumInGetOfRel() { Amount = 444, ForNthQuantity = 77, InEveryNthQuantity = 78, MaxLimit = 22, ItemLimit = 23, Product = new ProductContainer() { ProductId = "prodID", ProductName = "prodName" } },
                      new RewardItemForEveryNumOtherItemInGetOfRel() { Amount = 444, ForNthQuantity = 77, InEveryNthQuantity = 78, MaxLimit = 22, ItemLimit = 23, Product = new ProductContainer() { ProductId = "prodID", ProductName = "prodName" }, ConditionalProduct = new ProductContainer() { ProductId = "condProdID", ProductName = "condProdName" } }
                    );


            var dynamicPromotion = new DynamicPromotion
            {
                DynamicExpression = AbstractTypeFactory<PromotionConditionAndRewardTree>.TryCreateInstance()
            };
            dynamicPromotion.DynamicExpression.WithChildrens(
                blockCustomer,
                blockCatalog,
                blockCart,
                blockReward
                );

            dynamicPromotion.AssertCloneIndependency();
        }

        private DynamicPromotionMoq CreateDynamicPromotion(int totalUses, Coupon testCoupon)
        {
            var coupons = new List<Coupon>() { testCoupon };

            var promotionUsageServiceMoq = new Mock<IPromotionUsageSearchService>();
            promotionUsageServiceMoq.Setup(x => x.SearchUsagesAsync(It.IsAny<PromotionUsageSearchCriteria>()))
                .ReturnsAsync(new PromotionUsageSearchResult() { TotalCount = totalUses });

            var couponServiceMoq = new Mock<ICouponSearchService>();
            couponServiceMoq.Setup(x => x.SearchCouponsAsync(It.IsAny<CouponSearchCriteria>()))
                .ReturnsAsync(new CouponSearchResult() { Results = coupons });

            var result = new DynamicPromotionMoq()
            {
                CouponSearchService = couponServiceMoq.Object,
                PromotionUsageSearchService = promotionUsageServiceMoq.Object
            };

            return result;
        }

        private class DynamicPromotionMoq : DynamicPromotion
        {
            public new async Task<IEnumerable<Coupon>> FindValidCouponsAsync(ICollection<string> couponCodes, string userId)
            {
                return await base.FindValidCouponsAsync(couponCodes, userId);
            }
        }
    }
}
