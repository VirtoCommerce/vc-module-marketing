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
    public class PromotionUsageUsageServiceUnitTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IMarketingRepository> _repositoryMock;
        private readonly Mock<IEventPublisher> _eventPublisherMock;

        public PromotionUsageUsageServiceUnitTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _repositoryMock = new Mock<IMarketingRepository>();
            _eventPublisherMock = new Mock<IEventPublisher>();
        }

        [Fact]
        public async Task GetByIdsAsync_GetThenSavePromotionUsage_ReturnCachedPromotionUsage()
        {
            //Arrange
            var id = Guid.NewGuid().ToString();
            var newPromotionUsage = new PromotionUsage { Id = id };
            var newPromotionUsageEntity = AbstractTypeFactory<PromotionUsageEntity>.TryCreateInstance().FromModel(newPromotionUsage, new PrimaryKeyResolvingMap());
            var service = GetPromotionUsageServiceImplWithPlatformMemoryCache();
            _repositoryMock.Setup(x => x.Add(newPromotionUsageEntity))
                .Callback(() =>
                {
                    _repositoryMock.Setup(o => o.GetMarketingUsagesByIdsAsync(new[] { id }))
                        .ReturnsAsync(new[] { newPromotionUsageEntity });
                });

            //Act
            var nullPromotionUsage = await service.GetByIdsAsync(new[] { id });
            await service.SaveUsagesAsync(new[] { newPromotionUsage });
            var promotionUsage = await service.GetByIdsAsync(new[] { id });

            //Assert
            Assert.NotEqual(nullPromotionUsage, promotionUsage);
        }


        private PromotionUsageService GetPromotionUsageServiceImplWithPlatformMemoryCache()
        {
            var memoryCache = new MemoryCache(Options.Create(new MemoryCacheOptions()));
            var platformMemoryCache = new PlatformMemoryCache(memoryCache, Options.Create(new CachingOptions()), new Mock<ILogger<PlatformMemoryCache>>().Object);
            _repositoryMock.Setup(ss => ss.UnitOfWork).Returns(_unitOfWorkMock.Object);

            return GetPromotionUsageService(platformMemoryCache, _repositoryMock.Object);
        }

        public PromotionUsageService GetPromotionUsageService(IPlatformMemoryCache platformMemoryCache, IMarketingRepository repository)
        {
            return new PromotionUsageService(
                () => repository,
                _eventPublisherMock.Object,
                platformMemoryCache);
        }
    }
}
