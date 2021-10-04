using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using VirtoCommerce.MarketingModule.Core.Model.Promotions;
using VirtoCommerce.MarketingModule.Data.Model;
using VirtoCommerce.MarketingModule.Data.Repositories;
using VirtoCommerce.MarketingModule.Data.Services;
using VirtoCommerce.Platform.Caching;
using VirtoCommerce.Platform.Core.Caching;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Domain;
using VirtoCommerce.Platform.Core.Events;
using Xunit;

namespace VirtoCommerce.MarketingModule.Test
{
    public class CouponServiceTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IMarketingRepository> _repositoryMock;
        private readonly Mock<IEventPublisher> _eventPublisherMock;

        public CouponServiceTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _repositoryMock = new Mock<IMarketingRepository>();
            _eventPublisherMock = new Mock<IEventPublisher>();
        }

        [Fact]
        public async Task GetByIdsAsync_GetThenSaveCoupon_ReturnCachedCoupon()
        {
            //Arrange
            var id = Guid.NewGuid().ToString();
            var newCoupon = new Coupon { Id = id, Code = "Test" };
            var newCouponEntity = AbstractTypeFactory<CouponEntity>.TryCreateInstance().FromModel(newCoupon, new PrimaryKeyResolvingMap());
            var service = GetCouponServiceImplWithPlatformMemoryCache();
            _repositoryMock.Setup(x => x.Add(newCouponEntity))
                .Callback(() =>
                {
                    _repositoryMock.Setup(o => o.GetCouponsByIdsAsync(new[] { id }))
                        .ReturnsAsync(new[] { newCouponEntity });
                });

            //Act
            var nullCoupon = await service.GetByIdsAsync(new[] { id });
            await service.SaveCouponsAsync(new[] { newCoupon });
            var coupon = await service.GetByIdsAsync(new[] { id });

            //Assert
            Assert.NotEqual(nullCoupon, coupon);
        }


        private CouponService GetCouponServiceImplWithPlatformMemoryCache()
        {
            var memoryCache = new MemoryCache(Options.Create(new MemoryCacheOptions()));
            var platformMemoryCache = new PlatformMemoryCache(memoryCache, Options.Create(new CachingOptions()), new Mock<ILogger<PlatformMemoryCache>>().Object);
            _repositoryMock.Setup(ss => ss.UnitOfWork).Returns(_unitOfWorkMock.Object);

            return GetCouponService(platformMemoryCache, _repositoryMock.Object);
        }

        public CouponService GetCouponService(IPlatformMemoryCache platformMemoryCache, IMarketingRepository repository)
        {
            return new CouponService(
                () => repository,
                _eventPublisherMock.Object,
                platformMemoryCache);
        }
    }
}
