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

public class DynamicContentPlaceSearchService(
    Func<IMarketingRepository> repositoryFactory,
    IPlatformMemoryCache platformMemoryCache,
    IDynamicContentPlaceService crudService,
    IOptions<CrudOptions> crudOptions)
    : SearchService<DynamicContentPlaceSearchCriteria, DynamicContentPlaceSearchResult, DynamicContentPlace, DynamicContentPlaceEntity>
        (repositoryFactory, platformMemoryCache, crudService, crudOptions),
        IDynamicContentPlaceSearchService
{
    protected override IQueryable<DynamicContentPlaceEntity> BuildQuery(IRepository repository, DynamicContentPlaceSearchCriteria criteria)
    {
        var query = ((IMarketingRepository)repository).Places;

        if (!criteria.FolderId.IsNullOrEmpty())
        {
            query = query.Where(x => x.FolderId == criteria.FolderId);
        }

        if (!criteria.Keyword.IsNullOrEmpty())
        {
            query = query.Where(x => x.Name.Contains(criteria.Keyword));
        }

        return query;
    }

    protected override IList<SortInfo> BuildSortExpression(DynamicContentPlaceSearchCriteria criteria)
    {
        var sortInfos = criteria.SortInfos;

        if (sortInfos.IsNullOrEmpty())
        {
            sortInfos =
            [
                new SortInfo
                {
                    SortColumn = nameof(DynamicContentPlace.Name),
                    SortDirection = SortDirection.Ascending,
                },
            ];
        }

        return sortInfos;
    }
}
