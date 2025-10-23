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

public class PromotionUsageSearchService(
    Func<IMarketingRepository> repositoryFactory,
    IPlatformMemoryCache platformMemoryCache,
    IPromotionUsageService crudService,
    IOptions<CrudOptions> crudOptions)
    : SearchService<PromotionUsageSearchCriteria, PromotionUsageSearchResult, PromotionUsage, PromotionUsageEntity>
        (repositoryFactory, platformMemoryCache, crudService, crudOptions),
        IPromotionUsageSearchService
{
    [Obsolete("Use SearchAsync()", DiagnosticId = "VC0011", UrlFormat = "https://docs.virtocommerce.org/products/products-virto3-versions")]
    public virtual Task<PromotionUsageSearchResult> SearchUsagesAsync(PromotionUsageSearchCriteria criteria)
    {
        return SearchAsync(criteria);
    }


    [Obsolete("Use BuildQuery(IRepository repository, PromotionUsageSearchCriteria criteria)", DiagnosticId = "VC0011", UrlFormat = "https://docs.virtocommerce.org/products/products-virto3-versions")]
    protected virtual IQueryable<PromotionUsageEntity> BuildQuery(IMarketingRepository repository, PromotionUsageSearchCriteria criteria)
    {
        return BuildQuery((IRepository)repository, criteria);
    }

    protected override IQueryable<PromotionUsageEntity> BuildQuery(IRepository repository, PromotionUsageSearchCriteria criteria)
    {
        var query = ((IMarketingRepository)repository).PromotionUsages;

        if (!criteria.PromotionId.IsNullOrEmpty())
        {
            query = query.Where(x => x.PromotionId == criteria.PromotionId);
        }

        if (!criteria.CouponCode.IsNullOrEmpty())
        {
            query = query.Where(x => x.CouponCode == criteria.CouponCode);
        }

        if (!criteria.ObjectId.IsNullOrEmpty())
        {
            query = query.Where(x => x.ObjectId == criteria.ObjectId);
        }

        if (!criteria.ObjectType.IsNullOrEmpty())
        {
            query = query.Where(x => x.ObjectType == criteria.ObjectType);
        }

        if (!criteria.UserId.IsNullOrWhiteSpace())
        {
            query = query.Where(x => x.UserId == criteria.UserId);
        }

        if (!criteria.UserName.IsNullOrWhiteSpace())
        {
            query = query.Where(x => x.UserName == criteria.UserName);
        }

        return query;
    }

    protected override IList<SortInfo> BuildSortExpression(PromotionUsageSearchCriteria criteria)
    {
        var sortInfos = criteria.SortInfos;

        if (sortInfos.IsNullOrEmpty())
        {
            sortInfos =
            [
                new SortInfo
                {
                    SortColumn = nameof(PromotionUsage.ModifiedDate),
                    SortDirection = SortDirection.Descending,
                },
            ];
        }

        return sortInfos;
    }
}
