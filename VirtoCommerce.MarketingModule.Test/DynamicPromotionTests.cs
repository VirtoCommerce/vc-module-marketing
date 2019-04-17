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
        [Fact]
        public void FindValidCouponTest()
        {
            //Arrange
            var coupons = new List<Coupon>()
            {
                new Coupon()
                {
                    Id = "1",
                    Code = "1",
                    MaxUsesNumber = 9
                },
                new Coupon()
                {
                    Id = "2",
                    Code = "2",
                    MaxUsesNumber = 10
                },
                new Coupon()
                {
                    Id = "3",
                    Code = "3",
                    MaxUsesNumber = 11
                },
                new Coupon()
                {
                    Id = "4",
                    Code = "4",
                    ExpirationDate = DateTime.Now.AddDays(-1)
                },
                new Coupon()
                {
                    Id = "5",
                    Code = "5",
                    ExpirationDate = DateTime.Now
                },
                new Coupon()
                {
                    Id = "6",
                    Code = "6",
                    ExpirationDate = DateTime.Now.AddDays(1)
                }
            };

            var promotionUsageServiceMoq = new Mock<IPromotionUsageService>();
            promotionUsageServiceMoq.Setup(x => x.SearchUsages(It.IsAny<PromotionUsageSearchCriteria>()))
                .Returns(new GenericSearchResult<PromotionUsage>() { TotalCount = 10 });

            var couponServiceMoq = new Mock<ICouponService>();
            couponServiceMoq.Setup(x => x.SearchCoupons(It.IsAny<CouponSearchCriteria>()))
                .Returns(new GenericSearchResult<Coupon>() { Results = coupons });

            var dynamicPromotion = new DynamicPromotionMoq(new Moq.Mock<IExpressionSerializer>().Object,
                couponServiceMoq.Object, promotionUsageServiceMoq.Object);

            //Act
            var validCoupons = dynamicPromotion.FindValidCoupons(new List<string>() { "any coupon" }, null);
            var validCouponsWithUserId = dynamicPromotion.FindValidCoupons(new List<string>() { "any coupon" }, "userId");

            //Assert
            Assert.Equal(3, validCoupons.Count());
            Assert.Equal(3, validCouponsWithUserId.Count());
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
