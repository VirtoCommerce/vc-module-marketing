using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VirtoCommerce.MarketingModule.Core.Events;
using VirtoCommerce.MarketingModule.Core.Model;
using VirtoCommerce.MarketingModule.Core.Services;
using VirtoCommerce.MarketingModule.Data.Model;
using VirtoCommerce.MarketingModule.Data.Repositories;
using VirtoCommerce.Platform.Core.Caching;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Events;
using VirtoCommerce.Platform.Data.GenericCrud;

namespace VirtoCommerce.MarketingModule.Data.Services;

public class DynamicContentFolderService(
    Func<IMarketingRepository> repositoryFactory,
    IPlatformMemoryCache platformMemoryCache,
    IEventPublisher eventPublisher)
    : CrudService<DynamicContentFolder, DynamicContentFolderEntity, DynamicContentFolderChangingEvent, DynamicContentFolderChangedEvent>
        (repositoryFactory, platformMemoryCache, eventPublisher),
        IDynamicContentFolderService
{
    public override async Task DeleteAsync(IList<string> ids, bool softDelete = false)
    {
        var allIds = await IncludeChildFolderIds(ids);
        await base.DeleteAsync(allIds, softDelete);
    }

    protected override Task<IList<DynamicContentFolderEntity>> LoadEntities(IRepository repository, IList<string> ids, string responseGroup)
    {
        return ((IMarketingRepository)repository).GetContentFoldersByIdsAsync(ids);
    }

    private async Task<List<string>> IncludeChildFolderIds(IList<string> ids)
    {
        var allIds = ids.ToList();

        using var repository = repositoryFactory();
        var currentIds = ids;

        while (currentIds.Count > 0)
        {
            var parentIds = currentIds;

            var childIds = await repository.Folders
                .Where(x => parentIds.Contains(x.ParentFolderId))
                .Select(x => x.Id)
                .ToListAsync();

            allIds.InsertRange(0, childIds);

            currentIds = childIds;
        }

        return allIds;
    }
}
