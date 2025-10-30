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

public class CouponSearchService(
    Func<IMarketingRepository> repositoryFactory,
    IPlatformMemoryCache platformMemoryCache,
    ICouponService crudService,
    IOptions<CrudOptions> crudOptions)
    : SearchService<CouponSearchCriteria, CouponSearchResult, Coupon, CouponEntity>
        (repositoryFactory, platformMemoryCache, crudService, crudOptions),
        ICouponSearchService
{
    [Obsolete("Use SearchAsync()", DiagnosticId = "VC0011", UrlFormat = "https://docs.virtocommerce.org/products/products-virto3-versions")]
    public Task<CouponSearchResult> SearchCouponsAsync(CouponSearchCriteria criteria)
    {
        return SearchAsync(criteria);
    }


    [Obsolete("Use BuildQuery(IRepository repository, CouponSearchCriteria criteria)", DiagnosticId = "VC0011", UrlFormat = "https://docs.virtocommerce.org/products/products-virto3-versions")]
    protected virtual IQueryable<CouponEntity> BuildQuery(CouponSearchCriteria criteria, IMarketingRepository repository)
    {
        return BuildQuery(repository, criteria);
    }

    protected override IQueryable<CouponEntity> BuildQuery(IRepository repository, CouponSearchCriteria criteria)
    {
        var query = ((IMarketingRepository)repository).Coupons;

        if (!criteria.PromotionId.IsNullOrEmpty())
        {
            query = query.Where(x => x.PromotionId == criteria.PromotionId);
        }

        if (!criteria.Code.IsNullOrEmpty())
        {
            query = query.Where(x => x.Code == criteria.Code);
        }

        if (!criteria.Codes.IsNullOrEmpty())
        {
            query = query.Where(x => criteria.Codes.Contains(x.Code));
        }

        return query;
    }

    [Obsolete("Use BuildSortExpression()", DiagnosticId = "VC0011", UrlFormat = "https://docs.virtocommerce.org/products/products-virto3-versions")]
    protected virtual IList<SortInfo> BuildSearchExpression(CouponSearchCriteria criteria)
    {
        return BuildSortExpression(criteria);
    }

    protected override IList<SortInfo> BuildSortExpression(CouponSearchCriteria criteria)
    {
        var sortInfos = criteria.SortInfos;

        // TODO: Sort by TotalUsesCount 
        if (sortInfos.IsNullOrEmpty() || sortInfos.Any(x => x.SortColumn.EqualsIgnoreCase(nameof(Coupon.TotalUsesCount))))
        {
            sortInfos =
            [
                new SortInfo
                {
                    SortColumn = nameof(Coupon.Code),
                    SortDirection = SortDirection.Descending,
                },
            ];
        }

        if (sortInfos.Count < 2)
        {
            sortInfos.Add(new SortInfo
            {
                SortColumn = nameof(Coupon.Id),
                SortDirection = SortDirection.Ascending,
            });
        }

        return sortInfos;
    }
}
