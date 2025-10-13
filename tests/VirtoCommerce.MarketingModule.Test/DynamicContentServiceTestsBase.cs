using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MockQueryable.Moq;
using Moq;
using VirtoCommerce.MarketingModule.Data.Model;
using VirtoCommerce.MarketingModule.Data.Repositories;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Domain;

namespace VirtoCommerce.MarketingModule.Test;

public abstract class DynamicContentServiceTestsBase
{
    protected static Mock<IMarketingRepository> GetRepositoryMock(List<DynamicContentFolderEntity> folders)
    {
        var foldersDbSetMock = folders.BuildMockDbSet();

        var repositoryMock = new Mock<IMarketingRepository>();

        repositoryMock
            .Setup(x => x.UnitOfWork)
            .Returns(Mock.Of<IUnitOfWork>());

        repositoryMock
            .Setup(x => x.Folders)
            .Returns(() => foldersDbSetMock.Object);

        repositoryMock
            .Setup(x => x.GetContentFoldersByIdsAsync(It.IsAny<IList<string>>()))
            .Returns((IList<string> ids) =>
            {
                return Task.FromResult<IList<DynamicContentFolderEntity>>(
                    folders
                        .Where(x => ids.Contains(x.Id))
                        .ToList()
                );
            });

        repositoryMock
            .Setup(x => x.Remove(It.IsAny<DynamicContentFolderEntity>()))
            .Callback<DynamicContentFolderEntity>(entity =>
            {
                Remove(folders, [entity.Id]);
            });

        repositoryMock
            .Setup(x => x.RemoveFoldersAsync(It.IsAny<DynamicContentFolderEntity[]>()))
            .Callback<DynamicContentFolderEntity[]>(entities =>
            {

                Remove(folders, entities.Select(x => x.Id));
            });

        return repositoryMock;
    }

    private static void Remove<TEntity>(List<TEntity> entities, IEnumerable<string> ids)
        where TEntity : IEntity
    {
        foreach (var entity in entities.ToList().Where(x => ids.ContainsIgnoreCase(x.Id)))
        {
            entities.Remove(entity);
        }
    }
}
