using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using VirtoCommerce.MarketingModule.Data.Model;
using VirtoCommerce.MarketingModule.Data.Repositories;
using VirtoCommerce.MarketingModule.Data.Services;
using VirtoCommerce.Platform.Caching;
using VirtoCommerce.Platform.Core.Caching;
using VirtoCommerce.Platform.Core.Domain;
using VirtoCommerce.Platform.Core.Events;
using Xunit;

namespace VirtoCommerce.MarketingModule.Test
{
    public class DynamicContentServiceUnitTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IMarketingRepository> _repositoryMock;
        private readonly Mock<IEventPublisher> _eventPublisherMock;

        public DynamicContentServiceUnitTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _repositoryMock = new Mock<IMarketingRepository>();
            _eventPublisherMock = new Mock<IEventPublisher>();
        }

        [Fact]
        public async Task DeleteFoldersAsync_FolderHasChildren_AllRemoved()
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

            var items = new[] { mainFolder, childFolder1, childFolder2, childFolder3, childFolder4, singleFolder }.AsQueryable();

            _repositoryMock.Setup(o => o.Folders).Returns(() => items);
            _repositoryMock.Setup(o => o.RemoveFoldersAsync(It.IsAny<string[]>())).Callback<string[]>(f =>
            {
                var allFolders = _repositoryMock.Object.Folders.Where(x => !f.Contains(x.Id)).AsQueryable();
                _repositoryMock.Setup(o => o.Folders).Returns(() => allFolders);
            });

            var service = GetDynamicContentServiceWithPlatformMemoryCache();

            //Act
            await service.DeleteFoldersAsync(new[] { mainId });

            //Assert
            _repositoryMock.Object.Folders.Should().HaveCount(1);
            _repositoryMock.Object.Folders.Should().Contain(x => x.Id == singleId);
        }


        private DynamicContentService GetDynamicContentServiceWithPlatformMemoryCache()
        {
            var memoryCache = new MemoryCache(Options.Create(new MemoryCacheOptions()));
            var platformMemoryCache = new PlatformMemoryCache(memoryCache, Options.Create(new CachingOptions()), new Mock<ILogger<PlatformMemoryCache>>().Object);
            _repositoryMock.Setup(ss => ss.UnitOfWork).Returns(_unitOfWorkMock.Object);

            return GetPromotionService(platformMemoryCache, _repositoryMock.Object);
        }

        public DynamicContentService GetPromotionService(IPlatformMemoryCache platformMemoryCache, IMarketingRepository repository)
        {
            return new DynamicContentService(() => repository, _eventPublisherMock.Object, platformMemoryCache);
        }
    }
}
