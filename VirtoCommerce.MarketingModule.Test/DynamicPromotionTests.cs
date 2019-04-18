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
        public static IEnumerable<object[]> Data =>
            new List<object[]>
            {
                new object[] {9, 10, null, 0},
                new object[] {10, 10, null, 0},
                new object[] {11, 10, null, 1},
                new object[] {11, 1, DateTime.Now.AddDays(-1), 0},
                new object[] {11, 1, DateTime.Now, 1},
                new object[] {11, 1, DateTime.Now.AddDays(1), 1},
            };

        [Theory]
        [MemberData(nameof(Data))]
        public void FindValidCouponTest(int maxUsesNumber, int totalUses, DateTime? expirationDate, int validCouponsExpect)
        {
            //Arrange
            var coupons = new List<Coupon>()
            {
                new Coupon()
                {
                    Id = "1",
                    Code = "1",
                    MaxUsesNumber = maxUsesNumber,
                    ExpirationDate = expirationDate
                }
            };

            var promotionUsageServiceMoq = new Mock<IPromotionUsageService>();
            promotionUsageServiceMoq.Setup(x => x.SearchUsages(It.IsAny<PromotionUsageSearchCriteria>()))
                .Returns(new GenericSearchResult<PromotionUsage>() {TotalCount = totalUses});

            var couponServiceMoq = new Mock<ICouponService>();
            couponServiceMoq.Setup(x => x.SearchCoupons(It.IsAny<CouponSearchCriteria>()))
                .Returns(new GenericSearchResult<Coupon>() {Results = coupons});

            var dynamicPromotion = new DynamicPromotionMoq(new Moq.Mock<IExpressionSerializer>().Object,
                couponServiceMoq.Object, promotionUsageServiceMoq.Object);

            //Act
            var validCoupons = dynamicPromotion.FindValidCoupons(new List<string>() {"any coupon"}, null);
            var validCouponsWithUserId = dynamicPromotion.FindValidCoupons(new List<string>() {"any coupon"}, "userId");

            //Assert
            Assert.Equal(validCouponsExpect, validCoupons.Count());
            Assert.Equal(validCouponsExpect, validCouponsWithUserId.Count());
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