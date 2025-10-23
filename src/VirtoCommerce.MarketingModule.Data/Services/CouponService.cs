using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VirtoCommerce.MarketingModule.Core.Events;
using VirtoCommerce.MarketingModule.Core.Model.Promotions;
using VirtoCommerce.MarketingModule.Core.Services;
using VirtoCommerce.MarketingModule.Data.Model;
using VirtoCommerce.MarketingModule.Data.Repositories;
using VirtoCommerce.Platform.Caching;
using VirtoCommerce.Platform.Core.Caching;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Events;
using VirtoCommerce.Platform.Data.GenericCrud;

namespace VirtoCommerce.MarketingModule.Data.Services;

public class CouponService(
    Func<IMarketingRepository> repositoryFactory,
    IPlatformMemoryCache platformMemoryCache,
    IEventPublisher eventPublisher)
    : CrudService<Coupon, CouponEntity, CouponChangingEvent, CouponChangedEvent>
        (repositoryFactory, platformMemoryCache, eventPublisher),
        ICouponService
{
    [Obsolete("Use GetAsync()", DiagnosticId = "VC0011", UrlFormat = "https://docs.virtocommerce.org/products/products-virto3-versions")]
    public async Task<Coupon[]> GetByIdsAsync(string[] ids)
    {
        return (await GetAsync(ids)).ToArray();
    }

    [Obsolete("Use SaveChangesAsync()", DiagnosticId = "VC0011", UrlFormat = "https://docs.virtocommerce.org/products/products-virto3-versions")]
    public Task SaveCouponsAsync(Coupon[] coupons)
    {
        return SaveChangesAsync(coupons);
    }

    [Obsolete("Use DeleteAsync()", DiagnosticId = "VC0011", UrlFormat = "https://docs.virtocommerce.org/products/products-virto3-versions")]
    public Task DeleteCouponsAsync(string[] ids)
    {
        return DeleteAsync(ids);
    }


    protected override async Task BeforeSaveChanges(IList<Coupon> models)
    {
        await base.BeforeSaveChanges(models);

        if (models.Any(x => x.Code.IsNullOrEmpty()))
        {
            throw new InvalidOperationException("Coupon cannot have empty code.");
        }

        using var repository = repositoryFactory();
        var nonUniqueCouponErrors = await repository.CheckCouponsForUniquenessAsync(models.Where(x => x.IsTransient()).ToArray());

        if (!nonUniqueCouponErrors.IsNullOrEmpty())
        {
            throw new InvalidOperationException(string.Join(Environment.NewLine, nonUniqueCouponErrors));
        }
    }

    protected override Task<IList<CouponEntity>> LoadEntities(IRepository repository, IList<string> ids, string responseGroup)
    {
        return ((IMarketingRepository)repository).GetCouponsByIdsAsync(ids);
    }

    protected override void ClearCache(IList<Coupon> models)
    {
        base.ClearCache(models);
        ClearPromotionCache(models);
    }

    protected override void ClearSearchCache(IList<Coupon> models)
    {
        base.ClearSearchCache(models);
        ClearPromotionSearchCache();
    }

    private static void ClearPromotionCache(IList<Coupon> models)
    {
        foreach (var promotionId in models.Select(x => x.PromotionId))
        {
            GenericCachingRegion<Promotion>.ExpireTokenForKey(promotionId);
        }
    }

    private static void ClearPromotionSearchCache()
    {
        GenericSearchCachingRegion<Promotion>.ExpireRegion();
    }
}
