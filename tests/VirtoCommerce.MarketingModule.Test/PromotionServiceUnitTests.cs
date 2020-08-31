using System;
using System.Collections.Generic;
using System.Text;
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
    public class PromotionServiceUnitTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IMarketingRepository> _repositoryMock;
        private readonly Mock<IEventPublisher> _eventPublisherMock;

        public PromotionServiceUnitTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _repositoryMock = new Mock<IMarketingRepository>();
            _eventPublisherMock = new Mock<IEventPublisher>();
        }

        [Fact]
        public async Task GetByIdsAsync_GetThenSavePromotion_ReturnCachedPromotion()
        {
            //Arrange
            var id = Guid.NewGuid().ToString();
            var newPromotion = new Promotion { Id = id };
            var newPromotionEntity = AbstractTypeFactory<PromotionEntity>.TryCreateInstance().FromModel(newPromotion, new PrimaryKeyResolvingMap());
            var service = GetPromotionServiceImplWithPlatformMemoryCache();
            _repositoryMock.Setup(x => x.Add(newPromotionEntity))
                .Callback(() =>
                {
                    _repositoryMock.Setup(o => o.GetPromotionsByIdsAsync(new[] { id }))
                        .ReturnsAsync(new[] { newPromotionEntity });
                });

            //Act
            var nullPromotion = await service.GetPromotionsByIdsAsync(new[] { id });
            await service.SavePromotionsAsync(new[] { newPromotion });
            var promotion = await service.GetPromotionsByIdsAsync(new[] { id });

            //Assert
            Assert.NotEqual(nullPromotion, promotion);
        }


        private PromotionService GetPromotionServiceImplWithPlatformMemoryCache()
        {
            var memoryCache = new MemoryCache(Options.Create(new MemoryCacheOptions()));
            var platformMemoryCache = new PlatformMemoryCache(memoryCache, Options.Create(new CachingOptions()), new Mock<ILogger<PlatformMemoryCache>>().Object);
            _repositoryMock.Setup(ss => ss.UnitOfWork).Returns(_unitOfWorkMock.Object);

            return GetPromotionService(platformMemoryCache, _repositoryMock.Object);
        }

        public PromotionService GetPromotionService(IPlatformMemoryCache platformMemoryCache, IMarketingRepository repository)
        {
            return new PromotionService(
                () => repository,
                platformMemoryCache,
                _eventPublisherMock.Object);
        }
    }
}
