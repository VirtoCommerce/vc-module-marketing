using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Authorization;
using Moq;
using VirtoCommerce.AssetsModule.Core.Assets;
using VirtoCommerce.MarketingModule.Core.Model.Promotions;
using VirtoCommerce.MarketingModule.Core.Model.Promotions.Search;
using VirtoCommerce.MarketingModule.Core.Search;
using VirtoCommerce.MarketingModule.Core.Services;
using VirtoCommerce.MarketingModule.Data.Model;
using VirtoCommerce.MarketingModule.Data.Repositories;
using VirtoCommerce.MarketingModule.Web.Controllers.Api;
using VirtoCommerce.Platform.Core.PushNotifications;
using VirtoCommerce.Platform.Core.Security;
using Xunit;

namespace VirtoCommerce.MarketingModule.Test;

public class MarketingModulePromotionControllerTests
{
    private readonly Mock<IPromotionService> _mockPromotion;
    private readonly Mock<IPromotionSearchService> _mockPromotionSearch;
    private readonly Mock<Func<IMarketingRepository>> _mockRepositoryFactory;
    private readonly Mock<ICouponSearchService> _mockCouponSearch;
    private readonly Mock<IAuthorizationService> _mockAuthorization;

    // Controller
    private readonly MarketingModulePromotionController _controller;

    public MarketingModulePromotionControllerTests()
    {
        _mockPromotion = new Mock<IPromotionService>();
        _mockPromotionSearch = new Mock<IPromotionSearchService>();
        _mockRepositoryFactory = new Mock<Func<IMarketingRepository>>();
        _mockCouponSearch = new Mock<ICouponSearchService>();
        _mockAuthorization = new Mock<IAuthorizationService>();

        _controller = new MarketingModulePromotionController(
            promotionService: _mockPromotion.Object,
            couponService: new Mock<ICouponService>().Object,
            promoEvaluator: new Mock<IMarketingPromoEvaluator>().Object,
            promoSearchService: _mockPromotionSearch.Object,
            userNameResolver: new Mock<IUserNameResolver>().Object,
            notifier: new Mock<IPushNotificationManager>().Object,
            blobStorageProvider: new Mock<IBlobStorageProvider>().Object,
            repositoryFactory: _mockRepositoryFactory.Object,
            couponSearchService: _mockCouponSearch.Object,
            authorizationService: _mockAuthorization.Object,
            csvCouponImporter: null);
    }

    [Fact]
    public async Task PromotionsSearch_AuthorizationSuccess_ReturnPromotionSearchResult()
    {
        // Arrange
        var promotionSearchResult = new PromotionSearchResult
        {
            Results = GetTestPromotions(),
        };

        _mockAuthorization
            .Setup(x => x.AuthorizeAsync(It.IsAny<ClaimsPrincipal>(), It.IsAny<object>(), It.IsAny<IEnumerable<IAuthorizationRequirement>>()))
            .ReturnsAsync(AuthorizationResult.Success());

        _mockPromotionSearch
            .Setup(x => x.SearchAsync(It.IsAny<PromotionSearchCriteria>(), It.IsAny<bool>()))
            .ReturnsAsync(promotionSearchResult);

        // Act
        var actual = await _controller.PromotionsSearch(new PromotionSearchCriteria());
        var result = actual.ExtractFromOkResult();

        // Assert
        result.Should().NotBeNull();
    }

    [Fact]
    public async Task PromotionsSearch_AuthorizationFailed_ReturnUnauthorized()
    {
        // Arrange
        _mockAuthorization
            .Setup(x => x.AuthorizeAsync(It.IsAny<ClaimsPrincipal>(), It.IsAny<object>(), It.IsAny<IEnumerable<IAuthorizationRequirement>>()))
            .ReturnsAsync(AuthorizationResult.Failed());

        // Act
        var actual = await _controller.PromotionsSearch(new PromotionSearchCriteria());

        // Assert
        actual.Value.Should().BeNull();
    }

    [Fact]
    public async Task GetPromotionById_AuthorizationSuccess_ReturnPromotion()
    {
        // Arrange
        _mockAuthorization
            .Setup(x => x.AuthorizeAsync(It.IsAny<ClaimsPrincipal>(), It.IsAny<object>(), It.IsAny<IEnumerable<IAuthorizationRequirement>>()))
            .ReturnsAsync(AuthorizationResult.Success());

        _mockPromotion
            .Setup(x => x.GetAsync(It.IsAny<IList<string>>(), It.IsAny<string>(), It.IsAny<bool>()))
            .ReturnsAsync(GetTestPromotions());

        // Act
        var actual = await _controller.GetPromotionById("1");
        var result = actual.ExtractFromOkResult();

        // Assert
        result.Should().NotBeNull();
    }

    [Fact]
    public async Task GetPromotionById_AuthorizationFailed_ReturnUnauthorized()
    {
        // Arrange
        _mockAuthorization
            .Setup(x => x.AuthorizeAsync(It.IsAny<ClaimsPrincipal>(), It.IsAny<object>(), It.IsAny<IEnumerable<IAuthorizationRequirement>>()))
            .ReturnsAsync(AuthorizationResult.Failed());

        _mockPromotion
            .Setup(x => x.GetAsync(It.IsAny<IList<string>>(), It.IsAny<string>(), It.IsAny<bool>()))
            .ReturnsAsync(GetTestPromotions());

        // Act
        var actual = await _controller.GetPromotionById("1");

        // Assert
        actual.Value.Should().BeNull();
    }

    [Fact]
    public async Task SearchCoupons_CouponFound_ReturnCouponSearchResult()
    {
        // Arrange
        var id = Guid.NewGuid().ToString();
        var coupons = new List<Coupon> { new() { Id = id, Code = "test" } };
        CouponEntity[] testCouponEntity = [new() { Id = id, Code = "test" }];

        _mockCouponSearch
            .Setup(x => x.SearchAsync(It.IsAny<CouponSearchCriteria>(), It.IsAny<bool>()))
            .ReturnsAsync(new CouponSearchResult { Results = coupons });

        _mockRepositoryFactory
            .Setup(x => x().GetCouponsByIdsAsync(It.IsAny<string[]>()))
            .ReturnsAsync(testCouponEntity);

        // Act
        var actual = await _controller.SearchCoupons(new CouponSearchCriteria { Code = "test" });
        var result = actual.ExtractFromOkResult();

        // Assert
        result.Should().NotBeNull();
    }

    private static IList<Promotion> GetTestPromotions()
    {
        return
        [
            new MockPromotion
            {
                Id = "FedEx Get 50% Off",
                Rewards =
                [
                    new ShipmentReward { ShippingMethod = "FedEx", Amount = 50, AmountType = RewardAmountType.Relative, IsValid = true },
                ],
                Priority = 1,
                IsExclusive = false,
            },
            new MockPromotion
            {
                Id = "FedEx Get 30% Off",
                Rewards =
                [
                    new ShipmentReward { ShippingMethod = "FedEx", Amount = 30, AmountType = RewardAmountType.Relative, IsValid = true },
                ],
                Priority = 2,
                IsExclusive = false,
            },
            new MockPromotion
            {
                Id = "Any shipment 50% Off",
                Rewards =
                [
                    new ShipmentReward { ShippingMethod = null, Amount = 50, AmountType = RewardAmountType.Relative, IsValid = true },
                ],
                Priority = 2,
                IsExclusive = false,
            },
        ];
    }
}
