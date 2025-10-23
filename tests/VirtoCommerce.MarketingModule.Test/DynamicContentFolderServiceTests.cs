using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using VirtoCommerce.MarketingModule.Data.Model;
using VirtoCommerce.MarketingModule.Data.Services;
using VirtoCommerce.Platform.Caching;
using VirtoCommerce.Platform.Core.Events;
using Xunit;

namespace VirtoCommerce.MarketingModule.Test;

public class DynamicContentFolderServiceTests : DynamicContentServiceTestsBase
{
    [Fact]
    public async Task DeleteAsync_FolderHasChildren_AllRemoved()
    {
        //Arrange
        var mainId = Guid.NewGuid().ToString();
        var firstChildId = Guid.NewGuid().ToString();
        var singleId = Guid.NewGuid().ToString();

        var mainFolder = new DynamicContentFolderEntity { Id = mainId };
        var childFolder1 = new DynamicContentFolderEntity { Id = firstChildId, ParentFolderId = mainId };
        var childFolder2 = new DynamicContentFolderEntity { Id = Guid.NewGuid().ToString(), ParentFolderId = firstChildId };
        var childFolder3 = new DynamicContentFolderEntity { Id = Guid.NewGuid().ToString(), ParentFolderId = firstChildId };
        var childFolder4 = new DynamicContentFolderEntity { Id = Guid.NewGuid().ToString(), ParentFolderId = firstChildId };
        var singleFolder = new DynamicContentFolderEntity { Id = singleId };

        var folders = new List<DynamicContentFolderEntity> { mainFolder, childFolder1, childFolder2, childFolder3, childFolder4, singleFolder };

        var service = GetDynamicContentFolderService(folders);

        //Act
        await service.DeleteAsync([mainId]);

        //Assert
        folders.Should().HaveCount(1);
        folders.Should().Contain(x => x.Id == singleId);
    }

    private static DynamicContentFolderService GetDynamicContentFolderService(List<DynamicContentFolderEntity> folders)
    {
        var memoryCache = new MemoryCache(Options.Create(new MemoryCacheOptions()));
        var platformMemoryCache = new PlatformMemoryCache(memoryCache, Options.Create(new CachingOptions()), Mock.Of<ILogger<PlatformMemoryCache>>());
        var repositoryMock = GetRepositoryMock(folders);
        var eventPublisher = Mock.Of<IEventPublisher>();

        return new DynamicContentFolderService(
            () => repositoryMock.Object,
            platformMemoryCache,
            eventPublisher);
    }
}
