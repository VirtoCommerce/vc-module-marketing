using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VirtoCommerce.MarketingModule.Core.Events;
using VirtoCommerce.MarketingModule.Core.Model.Promotions;
using VirtoCommerce.MarketingModule.Core.Services;
using VirtoCommerce.MarketingModule.Data.Caching;
using VirtoCommerce.MarketingModule.Data.Model;
using VirtoCommerce.MarketingModule.Data.Repositories;
using VirtoCommerce.Platform.Core.Caching;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Events;
using VirtoCommerce.Platform.Data.GenericCrud;

namespace VirtoCommerce.MarketingModule.Data.Services;

public class PromotionService(
    Func<IMarketingRepository> repositoryFactory,
    IPlatformMemoryCache platformMemoryCache,
    IEventPublisher eventPublisher)
    : CrudService<Promotion, PromotionEntity, PromotionChangingEvent, PromotionChangedEvent>
        (repositoryFactory, platformMemoryCache, eventPublisher),
        IPromotionService
{
    [Obsolete("Use GetAsync()", DiagnosticId = "VC0011", UrlFormat = "https://docs.virtocommerce.org/products/products-virto3-versions")]
    public virtual async Task<Promotion[]> GetPromotionsByIdsAsync(string[] ids)
    {
        return (await GetAsync(ids)).ToArray();
    }

    [Obsolete("Use SaveChangesAsync()", DiagnosticId = "VC0011", UrlFormat = "https://docs.virtocommerce.org/products/products-virto3-versions")]
    public virtual Task SavePromotionsAsync(Promotion[] promotions)
    {
        return SaveChangesAsync(promotions);
    }

    [Obsolete("Use DeleteAsync()", DiagnosticId = "VC0011", UrlFormat = "https://docs.virtocommerce.org/products/products-virto3-versions")]
    public virtual Task DeletePromotionsAsync(string[] ids)
    {
        return DeleteAsync(ids);
    }


    protected override Task<IList<PromotionEntity>> LoadEntities(IRepository repository, IList<string> ids, string responseGroup)
    {
        return ((IMarketingRepository)repository).GetPromotionsByIdsAsync(ids);
    }

    [Obsolete("Use ClearCache(IList<Promotion> models)", DiagnosticId = "VC0011", UrlFormat = "https://docs.virtocommerce.org/products/products-virto3-versions")]
    protected virtual void ClearCache(string[] promotionIds)
    {
        PromotionSearchCacheRegion.ExpireRegion();
        PromotionCacheRegion.ExpirePromotions(promotionIds);
    }
}
