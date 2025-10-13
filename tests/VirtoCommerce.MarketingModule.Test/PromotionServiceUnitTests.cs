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

public class PromotionServiceUnitTests
{
    private readonly Mock<IMarketingRepository> _repositoryMock = new();

    [Fact]
    [Obsolete("To be removed", DiagnosticId = "VC0011", UrlFormat = "https://docs.virtocommerce.org/products/products-virto3-versions")]
    public async Task GetByIdsAsync_GetThenSavePromotion_ReturnCachedPromotion()
    {
        // Arrange
        var id = Guid.NewGuid().ToString();
        var newPromotion = new Promotion { Id = id };
        var newPromotionEntity = AbstractTypeFactory<PromotionEntity>.TryCreateInstance().FromModel(newPromotion, new PrimaryKeyResolvingMap());
        var service = GetPromotionService();

        _repositoryMock
            .Setup(x => x.Add(newPromotionEntity))
            .Callback(() =>
            {
                _repositoryMock
                    .Setup(x => x.GetPromotionsByIdsAsync(new[] { id }))
                    .ReturnsAsync([newPromotionEntity]);
            });

        // Act
        var nullPromotion = await service.GetPromotionsByIdsAsync([id]);
        await service.SavePromotionsAsync([newPromotion]);
        var promotion = await service.GetPromotionsByIdsAsync([id]);

        // Assert
        Assert.NotEqual(nullPromotion, promotion);
    }


    private IPromotionService GetPromotionService()
    {
        var memoryCache = new MemoryCache(Options.Create(new MemoryCacheOptions()));
        var platformMemoryCache = new PlatformMemoryCache(memoryCache, Options.Create(new CachingOptions()), Mock.Of<ILogger<PlatformMemoryCache>>());

        _repositoryMock
            .Setup(x => x.UnitOfWork)
            .Returns(Mock.Of<IUnitOfWork>());

        return new PromotionService(
            () => _repositoryMock.Object,
            platformMemoryCache,
            Mock.Of<IEventPublisher>());
    }
}
