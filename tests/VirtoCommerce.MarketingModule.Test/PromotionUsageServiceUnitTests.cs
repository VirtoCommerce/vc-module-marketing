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
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Domain;
using VirtoCommerce.Platform.Core.Events;
using Xunit;

namespace VirtoCommerce.MarketingModule.Test;

[Obsolete("To be removed", DiagnosticId = "VC0011", UrlFormat = "https://docs.virtocommerce.org/products/products-virto3-versions")]
public class PromotionUsageUsageServiceUnitTests
{
    private readonly Mock<IMarketingRepository> _repositoryMock = new();

    [Fact]
    public async Task GetByIdsAsync_GetThenSavePromotionUsage_ReturnCachedPromotionUsage()
    {
        //Arrange
        var id = Guid.NewGuid().ToString();
        var newPromotionUsage = new PromotionUsage { Id = id };
        var newPromotionUsageEntity = AbstractTypeFactory<PromotionUsageEntity>.TryCreateInstance().FromModel(newPromotionUsage, new PrimaryKeyResolvingMap());
        var service = GetPromotionUsageService();

        _repositoryMock
            .Setup(x => x.Add(newPromotionUsageEntity))
            .Callback(() =>
            {
                _repositoryMock
                    .Setup(x => x.GetMarketingUsagesByIdsAsync(new[] { id }))
                    .ReturnsAsync([newPromotionUsageEntity]);
            });

        //Act
        var nullPromotionUsage = await service.GetByIdsAsync([id]);
        await service.SaveUsagesAsync([newPromotionUsage]);
        var promotionUsage = await service.GetByIdsAsync([id]);

        //Assert
        Assert.NotEqual(nullPromotionUsage, promotionUsage);
    }


    private PromotionUsageService GetPromotionUsageService()
    {
        var memoryCache = new MemoryCache(Options.Create(new MemoryCacheOptions()));
        var platformMemoryCache = new PlatformMemoryCache(memoryCache, Options.Create(new CachingOptions()), Mock.Of<ILogger<PlatformMemoryCache>>());

        _repositoryMock
            .Setup(x => x.UnitOfWork)
            .Returns(Mock.Of<IUnitOfWork>());

        return new PromotionUsageService(
            () => _repositoryMock.Object,
            platformMemoryCache,
            Mock.Of<IEventPublisher>());
    }
}
