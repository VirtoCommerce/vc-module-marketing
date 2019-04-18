using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using VirtoCommerce.Domain.Commerce.Model.Search;
using VirtoCommerce.Domain.Marketing.Model;
using VirtoCommerce.Domain.Marketing.Model.Promotions.Search;
using VirtoCommerce.Domain.Marketing.Services;
using VirtoCommerce.MarketingModule.Data.Promotions;
using VirtoCommerce.Platform.Core.Serialization;
using Xunit;

namespace VirtoCommerce.MarketingModule.Test
{
    public class DynamicPromotionTests
    {
        [Theory]
        [InlineData(9, 10, 0)]
        [InlineData(10, 10, 0)]
        [InlineData(11, 10, 1)]
        public void FindValidCoupon_UsesNumber(int maxUsesNumber, int totalUses, int expectedCouponsCount)
        {
            //Arrange
            var testCoupon = new Coupon()
            {
                Id = "1",
                Code = "1",
                MaxUsesNumber = maxUsesNumber,
            };

            var dynamicPromotion = CreateDynamicPromotion(totalUses, testCoupon);

            //Act
            var validCoupons = dynamicPromotion.FindValidCoupons(new List<string>() { "any coupon" }, null);

            //Assert
            Assert.Equal(expectedCouponsCount, validCoupons.Count());
        }

        [Theory]
        [InlineData(9, 10, 0, "userId")]
        [InlineData(10, 10, 0, "userId")]
        [InlineData(11, 10, 1, "userId")]
        public void FindValidCoupon_UsesNumberWithUserId(int maxUsesNumber, int totalUses, int expectedCouponsCount, string userId)
        {
            //Arrange
            var testCoupon = new Coupon()
            {
                Id = "1",
                Code = "1",
                MaxUsesNumber = maxUsesNumber,
            };

            var dynamicPromotion = CreateDynamicPromotion(totalUses, testCoupon);

            //Act
            var validCouponsWithUserId = dynamicPromotion.FindValidCoupons(new List<string>() { "any coupon" }, userId);

            //Assert
            Assert.Equal(expectedCouponsCount, validCouponsWithUserId.Count());
        }

        public static IEnumerable<object[]> ExpirationDateData =>
            new List<object[]>
            {
                new object[] {DateTime.Now.AddDays(-1), 0},
                new object[] {DateTime.Now, 1},
                new object[] {DateTime.Now.AddDays(1), 1},
            };

        [Theory]
        [MemberData(nameof(ExpirationDateData))]
        public void FindValidCoupon_ExpirationDate(DateTime expirationDate, int expectedCouponsCount)
        {
            //Arrange
            var testCoupon = new Coupon()
            {
                Id = "1",
                Code = "1",
                ExpirationDate = expirationDate
            };

            var dynamicPromotion = CreateDynamicPromotion(0, testCoupon);

            //Act
            var validCoupons = dynamicPromotion.FindValidCoupons(new List<string>() { "any coupon" }, null);

            //Assert
            Assert.Equal(expectedCouponsCount, validCoupons.Count());

        }


        private DynamicPromotionMoq CreateDynamicPromotion(int totalUses, Coupon testCoupon)
        {
            var coupons = new List<Coupon>()
            {
                testCoupon
            };

            var promotionUsageServiceMoq = new Mock<IPromotionUsageService>();
            promotionUsageServiceMoq.Setup(x => x.SearchUsages(It.IsAny<PromotionUsageSearchCriteria>()))
                .Returns(new GenericSearchResult<PromotionUsage>() { TotalCount = totalUses });

            var couponServiceMoq = new Mock<ICouponService>();
            couponServiceMoq.Setup(x => x.SearchCoupons(It.IsAny<CouponSearchCriteria>()))
                .Returns(new GenericSearchResult<Coupon>() { Results = coupons });

            return new DynamicPromotionMoq(new Moq.Mock<IExpressionSerializer>().Object,
                couponServiceMoq.Object, promotionUsageServiceMoq.Object);
        }

        private class DynamicPromotionMoq : DynamicPromotion
        {
            public DynamicPromotionMoq(IExpressionSerializer expressionSerializer, ICouponService couponService,
                IPromotionUsageService usageService) : base(expressionSerializer, couponService, usageService)
            {
            }

            public IEnumerable<Coupon> FindValidCoupons(ICollection<string> couponCodes, string userId)
            {
                return base.FindValidCoupons(couponCodes, userId);
            }
        }
    }
}
