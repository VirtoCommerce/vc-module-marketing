using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using VirtoCommerce.MarketingModule.Core.Model.Promotions;
using VirtoCommerce.MarketingModule.Core.Services;
using VirtoCommerce.MarketingModule.Data.Model;
using VirtoCommerce.MarketingModule.Data.Repositories;
using VirtoCommerce.MarketingModule.Data.Services;
using VirtoCommerce.Platform.Caching;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Domain;
using VirtoCommerce.Platform.Core.Events;
using Xunit;

namespace VirtoCommerce.MarketingModule.Test;

public class CouponServiceTests
{
    private readonly Mock<IMarketingRepository> _repositoryMock = new();

    [Fact]
    [Obsolete("To be removed", DiagnosticId = "VC0011", UrlFormat = "https://docs.virtocommerce.org/products/products-virto3-versions")]
    public async Task GetByIdsAsync_GetThenSaveCoupon_ReturnCachedCoupon()
    {
        // Arrange
        var id = Guid.NewGuid().ToString();
        var newCoupon = new Coupon { Id = id, Code = "Test" };
        var newCouponEntity = AbstractTypeFactory<CouponEntity>.TryCreateInstance().FromModel(newCoupon, new PrimaryKeyResolvingMap());
        var service = GetCouponService();

        _repositoryMock
            .Setup(x => x.Add(newCouponEntity))
            .Callback(() =>
            {
                _repositoryMock
                    .Setup(x => x.GetCouponsByIdsAsync(new[] { id }))
                    .ReturnsAsync([newCouponEntity]);
            });

        // Act
        var nullCoupon = await service.GetByIdsAsync([id]);
        await service.SaveCouponsAsync([newCoupon]);
        var coupon = await service.GetByIdsAsync([id]);

        // Assert
        Assert.NotEqual(nullCoupon, coupon);
    }


    private ICouponService GetCouponService()
    {
        var memoryCache = new MemoryCache(Options.Create(new MemoryCacheOptions()));
        var platformMemoryCache = new PlatformMemoryCache(memoryCache, Options.Create(new CachingOptions()), Mock.Of<ILogger<PlatformMemoryCache>>());

        _repositoryMock
            .Setup(x => x.UnitOfWork)
            .Returns(Mock.Of<IUnitOfWork>());

        return new CouponService(
            () => _repositoryMock.Object,
            platformMemoryCache,
            Mock.Of<IEventPublisher>());
    }
}
