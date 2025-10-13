using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VirtoCommerce.MarketingModule.Core.Events;
using VirtoCommerce.MarketingModule.Core.Model.Promotions;
using VirtoCommerce.MarketingModule.Core.Services;
using VirtoCommerce.MarketingModule.Data.Model;
using VirtoCommerce.MarketingModule.Data.Repositories;
using VirtoCommerce.Platform.Core.Caching;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Events;
using VirtoCommerce.Platform.Data.GenericCrud;

namespace VirtoCommerce.MarketingModule.Data.Services;

public class PromotionUsageService(
    Func<IMarketingRepository> repositoryFactory,
    IPlatformMemoryCache platformMemoryCache,
    IEventPublisher eventPublisher)
    : CrudService<PromotionUsage, PromotionUsageEntity, PromotionUsageChangingEvent, PromotionUsageChangedEvent>
        (repositoryFactory, platformMemoryCache, eventPublisher),
        IPromotionUsageService
{
    [Obsolete("Use GetAsync()", DiagnosticId = "VC0011", UrlFormat = "https://docs.virtocommerce.org/products/products-virto3-versions")]
    public virtual async Task<PromotionUsage[]> GetByIdsAsync(string[] ids)
    {
        return (await GetAsync(ids)).ToArray();
    }

    [Obsolete("Use SaveChangesAsync()", DiagnosticId = "VC0011", UrlFormat = "https://docs.virtocommerce.org/products/products-virto3-versions")]
    public virtual Task SaveUsagesAsync(PromotionUsage[] usages)
    {
        return SaveChangesAsync(usages);
    }

    [Obsolete("Use DeleteAsync()", DiagnosticId = "VC0011", UrlFormat = "https://docs.virtocommerce.org/products/products-virto3-versions")]
    public virtual Task DeleteUsagesAsync(string[] ids)
    {
        return DeleteAsync(ids);
    }


    protected override Task<IList<PromotionUsageEntity>> LoadEntities(IRepository repository, IList<string> ids, string responseGroup)
    {
        return ((IMarketingRepository)repository).GetMarketingUsagesByIdsAsync(ids);
    }
}
