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

public class DynamicContentPublicationSearchService(
    Func<IMarketingRepository> repositoryFactory,
    IPlatformMemoryCache platformMemoryCache,
    IDynamicContentPublicationService crudService,
    IOptions<CrudOptions> crudOptions)
    : SearchService<DynamicContentPublicationSearchCriteria, DynamicContentPublicationSearchResult, DynamicContentPublication, DynamicContentPublishingGroupEntity>
        (repositoryFactory, platformMemoryCache, crudService, crudOptions),
        IDynamicContentPublicationSearchService
{
    protected override IQueryable<DynamicContentPublishingGroupEntity> BuildQuery(IRepository repository, DynamicContentPublicationSearchCriteria criteria)
    {
        var query = ((IMarketingRepository)repository).PublishingGroups;

        if (!criteria.Store.IsNullOrEmpty())
        {
            query = query.Where(x => x.StoreId == criteria.Store);
        }

        if (criteria.OnlyActive)
        {
            query = query.Where(x => x.IsActive == true);
        }

        if (criteria.ToDate != null)
        {
            query = query.Where(x => x.StartDate == null || (criteria.ToDate >= x.StartDate && criteria.ToDate <= x.EndDate));
        }

        if (!criteria.PlaceName.IsNullOrEmpty())
        {
            query = query.Where(x => x.ContentPlaces.Any(y => y.ContentPlace.Name == criteria.PlaceName));
        }

        if (!criteria.Keyword.IsNullOrEmpty())
        {
            query = query.Where(q => q.Name.Contains(criteria.Keyword));
        }

        return query;
    }

    protected override IList<SortInfo> BuildSortExpression(DynamicContentPublicationSearchCriteria criteria)
    {
        var sortInfos = criteria.SortInfos;

        if (sortInfos.IsNullOrEmpty())
        {
            sortInfos =
            [
                new SortInfo
                {
                    SortColumn = nameof(DynamicContentPublication.Priority),
                    SortDirection = SortDirection.Ascending,
                },
            ];
        }

        return sortInfos;
    }
}
