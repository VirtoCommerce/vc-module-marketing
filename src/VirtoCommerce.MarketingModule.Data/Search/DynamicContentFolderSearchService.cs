using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Options;
using VirtoCommerce.MarketingModule.Core.Model;
using VirtoCommerce.MarketingModule.Core.Model.DynamicContent.Search;
using VirtoCommerce.MarketingModule.Core.Search;
using VirtoCommerce.MarketingModule.Core.Services;
using VirtoCommerce.MarketingModule.Data.Model;
using VirtoCommerce.MarketingModule.Data.Repositories;
using VirtoCommerce.Platform.Core.Caching;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.GenericCrud;
using VirtoCommerce.Platform.Data.GenericCrud;

namespace VirtoCommerce.MarketingModule.Data.Search;

public class DynamicContentFolderSearchService(
    Func<IMarketingRepository> repositoryFactory,
    IPlatformMemoryCache platformMemoryCache,
    IDynamicContentFolderService crudService,
    IOptions<CrudOptions> crudOptions)
    : SearchService<DynamicContentFolderSearchCriteria, DynamicContentFolderSearchResult, DynamicContentFolder, DynamicContentFolderEntity>
        (repositoryFactory, platformMemoryCache, crudService, crudOptions),
        IDynamicContentFolderSearchService
{
    protected override IQueryable<DynamicContentFolderEntity> BuildQuery(IRepository repository, DynamicContentFolderSearchCriteria criteria)
    {
        var query = ((IMarketingRepository)repository).Folders
            .Where(x => x.ParentFolderId == criteria.FolderId);

        if (!criteria.Keyword.IsNullOrEmpty())
        {
            query = query.Where(x => x.Name.Contains(criteria.Keyword));
        }

        return query;
    }

    protected override IList<SortInfo> BuildSortExpression(DynamicContentFolderSearchCriteria criteria)
    {
        var sortInfos = criteria.SortInfos;

        if (sortInfos.IsNullOrEmpty())
        {
            sortInfos =
            [
                new SortInfo
                {
                    SortColumn = nameof(DynamicContentFolder.Name),
                    SortDirection = SortDirection.Ascending,
                },
            ];
        }

        return sortInfos;
    }
}
