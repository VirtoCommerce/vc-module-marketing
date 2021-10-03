using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Moq;
using VirtoCommerce.MarketingModule.Core.Model.Promotions;
using VirtoCommerce.MarketingModule.Data.Repositories;
using VirtoCommerce.Platform.Core.Security;
using Xunit;
using VirtoCommerce.MarketingModule.Web.Controllers.Api;
using VirtoCommerce.MarketingModule.Core.Model.Promotions.Search;
using VirtoCommerce.MarketingModule.Core.Search;
using System.Security.Claims;
using VirtoCommerce.MarketingModule.Core.Services;
using VirtoCommerce.Platform.Core.PushNotifications;
using VirtoCommerce.Platform.Core.Assets;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using VirtoCommerce.MarketingModule.Data.Model;

namespace VirtoCommerce.MarketingModule.Test
{
    public class MarketingModulePromotionControllerTests
    {
        private readonly Mock<IPromotionService> _mockPromotion;
        private readonly Mock<ICouponService> _mockCoupon;
        private readonly Mock<IMarketingPromoEvaluator> _mockPromoEvaluator;
        private readonly Mock<IPromotionSearchService> _mockPromotionSearch;
        private readonly Mock<IUserNameResolver> _mockUserNameResolver;
        private readonly Mock<IPushNotificationManager> _mockNotifier;
        private readonly Mock<IBlobStorageProvider> _mockBlobStorageProvider;
        private readonly Mock<Func<IMarketingRepository>> _mockRepositoryFactory;
        private readonly Mock<ICouponSearchService> _mockCouponSearch;
        private readonly Mock<IAuthorizationService> _mockAuthorization;

        // Controller
        private readonly MarketingModulePromotionController _controller;

        public MarketingModulePromotionControllerTests()
        {
            _mockPromotion = new Mock<IPromotionService>();
            _mockCoupon = new Mock<ICouponService>();
            _mockPromoEvaluator = new Mock<IMarketingPromoEvaluator>();
            _mockPromotionSearch = new Mock<IPromotionSearchService>();
            _mockUserNameResolver = new Mock<IUserNameResolver>();
            _mockNotifier = new Mock<IPushNotificationManager>();
            _mockBlobStorageProvider = new Mock<IBlobStorageProvider>();
            _mockRepositoryFactory = new Mock<Func<IMarketingRepository>>();
            _mockCouponSearch = new Mock<ICouponSearchService>();
            _mockAuthorization = new Mock<IAuthorizationService>();

            _controller = new MarketingModulePromotionController(
                promotionService: _mockPromotion.Object,
                couponService: _mockCoupon.Object,
                promoEvaluator: _mockPromoEvaluator.Object,
                promoSearchService: _mockPromotionSearch.Object,
                userNameResolver: _mockUserNameResolver.Object,
                notifier: _mockNotifier.Object,
                blobStorageProvider: _mockBlobStorageProvider.Object,
                repositoryFactory: _mockRepositoryFactory.Object,
                couponSearchService: _mockCouponSearch.Object,
                authorizationService: _mockAuthorization.Object,
                csvCouponImporter: null);
        }

        [Fact]
        public async Task PromotionsSearch_AuthorizationSuccess_ReturnPromotionSearchResult()
        {
            //Arrange
            var promotionSearchResult = new PromotionSearchResult
            {
                Results = TestPromotions.ToList()
            };
            _mockAuthorization.Setup(x => x.AuthorizeAsync(It.IsAny<ClaimsPrincipal>(), It.IsAny<object>(), It.IsAny<IEnumerable<IAuthorizationRequirement>>())).ReturnsAsync(AuthorizationResult.Success());
            _mockPromotionSearch.Setup(x => x.SearchPromotionsAsync(It.IsAny<PromotionSearchCriteria>())).ReturnsAsync(promotionSearchResult);

            //Act
            var actual = await _controller.PromotionsSearch(new PromotionSearchCriteria());
            var result = actual.ExtractFromOkResult();

            //Assert
            result.Should().NotBeNull();
        }

        [Fact]
        public async Task PromotionsSearch_AuthorizationFailed_ReturnUnauthorized()
        {
            //Arrange
            _mockAuthorization.Setup(x => x.AuthorizeAsync(It.IsAny<ClaimsPrincipal>(), It.IsAny<object>(), It.IsAny<IEnumerable<IAuthorizationRequirement>>())).ReturnsAsync(AuthorizationResult.Failed());

            //Act
            var actual = await _controller.PromotionsSearch(new PromotionSearchCriteria());

            //Assert
            actual.Value.Should().BeNull();
        }

        [Fact]
        public async Task GetPromotionById_AuthorizationSuccess_ReturnPromotion()
        {
            //Arrange
            _mockAuthorization.Setup(x => x.AuthorizeAsync(It.IsAny<ClaimsPrincipal>(), It.IsAny<object>(), It.IsAny<IEnumerable<IAuthorizationRequirement>>())).ReturnsAsync(AuthorizationResult.Success());
            _mockPromotion.Setup(x => x.GetPromotionsByIdsAsync(It.IsAny<string[]>())).ReturnsAsync(TestPromotions.ToArray());

            //Act
            var actual = await _controller.GetPromotionById("");
            var result = actual.ExtractFromOkResult();

            //Assert
            result.Should().NotBeNull();
        }

        [Fact]
        public async Task GetPromotionById_AuthorizationFailed_ReturnUnauthorized()
        {
            //Arrange
            _mockAuthorization.Setup(x => x.AuthorizeAsync(It.IsAny<ClaimsPrincipal>(), It.IsAny<object>(), It.IsAny<IEnumerable<IAuthorizationRequirement>>())).ReturnsAsync(AuthorizationResult.Failed());
            _mockPromotion.Setup(x => x.GetPromotionsByIdsAsync(It.IsAny<string[]>())).ReturnsAsync(TestPromotions.ToArray());

            //Act
            var actual = await _controller.GetPromotionById("");

            //Assert
            actual.Value.Should().BeNull();
        }

        [Fact]
        public async Task SearchCoupons_CouponFound_ReturnCouponSearchResult()
        {
            //Arrange
            var id = Guid.NewGuid().ToString();
            var coupons = new List<Coupon>() { new Coupon() { Id = id, Code = "test" } };
            CouponEntity[] testCouponEntity = { new CouponEntity() { Id = id, Code = "test" } };

            _mockCouponSearch.Setup(x => x.SearchCouponsAsync(It.IsAny<CouponSearchCriteria>())).ReturnsAsync(new CouponSearchResult() { Results = coupons });
            _mockRepositoryFactory.Setup(x => x().GetCouponsByIdsAsync(It.IsAny<string[]>())).ReturnsAsync(testCouponEntity);

            //Act
            var actual = await _controller.SearchCoupons(new CouponSearchCriteria() { Code = "test" });
            var result = actual.ExtractFromOkResult();

            //Assert
            result.Should().NotBeNull();
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
            }
        }
    }
}
