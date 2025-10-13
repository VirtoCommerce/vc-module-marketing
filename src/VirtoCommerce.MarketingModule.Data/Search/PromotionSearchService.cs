using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using VirtoCommerce.MarketingModule.Core.Model.Promotions;
using VirtoCommerce.MarketingModule.Core.Model.Promotions.Search;
using VirtoCommerce.MarketingModule.Core.Search;
using VirtoCommerce.MarketingModule.Core.Services;
using VirtoCommerce.MarketingModule.Data.Model;
using VirtoCommerce.MarketingModule.Data.Repositories;
using VirtoCommerce.Platform.Core.Caching;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.GenericCrud;
using VirtoCommerce.Platform.Data.GenericCrud;

namespace VirtoCommerce.MarketingModule.Data.Search;

public class PromotionSearchService(
    Func<IMarketingRepository> repositoryFactory,
    IPlatformMemoryCache platformMemoryCache,
    IPromotionService crudService,
    IOptions<CrudOptions> crudOptions)
    : SearchService<PromotionSearchCriteria, PromotionSearchResult, Promotion, PromotionEntity>
        (repositoryFactory, platformMemoryCache, crudService, crudOptions),
        IPromotionSearchService
{
    [Obsolete("Use SearchAsync()", DiagnosticId = "VC0011", UrlFormat = "https://docs.virtocommerce.org/products/products-virto3-versions")]
    public virtual Task<PromotionSearchResult> SearchPromotionsAsync(PromotionSearchCriteria criteria)
    {
        return SearchAsync(criteria);
    }


    protected override IQueryable<PromotionEntity> BuildQuery(IRepository repository, PromotionSearchCriteria criteria)
    {
        // Temporarily calling the obsolete method that could potentially be overridden in derived classes.
#pragma warning disable VC0011 // Type or member is obsolete
        return BuildQuery((IMarketingRepository)repository, criteria);
#pragma warning restore VC0011 // Type or member is obsolete
    }

    [Obsolete("Use BuildQuery(IRepository repository, PromotionSearchCriteria criteria)", DiagnosticId = "VC0011", UrlFormat = "https://docs.virtocommerce.org/products/products-virto3-versions")]
    protected virtual IQueryable<PromotionEntity> BuildQuery(IMarketingRepository repository, PromotionSearchCriteria criteria)
    {
        var query = repository.Promotions;

        if (!criteria.Store.IsNullOrEmpty())
        {
            query = query.Where(x => !x.Stores.Any() || x.Stores.Any(s => s.StoreId == criteria.Store));
        }

        if (!criteria.StoreIds.IsNullOrEmpty())
        {
            query = query.Where(x => !x.Stores.Any() || x.Stores.Any(s => criteria.StoreIds.Contains(s.StoreId)));
        }

        if (criteria.OnlyActive)
        {
            var now = DateTime.UtcNow;
            query = query.Where(x => x.IsActive && now >= x.StartDate && (x.EndDate == null || x.EndDate >= now));
        }

        if (!criteria.Keyword.IsNullOrEmpty())
        {
            query = query.Where(x => x.Name.Contains(criteria.Keyword) || x.Description.Contains(criteria.Keyword));
        }

        return query;
    }

    protected override IList<SortInfo> BuildSortExpression(PromotionSearchCriteria criteria)
    {
        var sortInfos = criteria.SortInfos;

        if (sortInfos.IsNullOrEmpty())
        {
            sortInfos =
            [
                new SortInfo
                {
                    SortColumn = nameof(Promotion.Priority),
                    SortDirection = SortDirection.Descending,
                },
            ];
        }

        return sortInfos;
    }
}
