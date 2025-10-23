using System;
using System.Threading.Tasks;
using VirtoCommerce.MarketingModule.Core.Model.Promotions;
using VirtoCommerce.Platform.Core.GenericCrud;

namespace VirtoCommerce.MarketingModule.Core.Services;

public interface IPromotionUsageService : ICrudService<PromotionUsage>
{
    [Obsolete("Use GetAsync()", DiagnosticId = "VC0011", UrlFormat = "https://docs.virtocommerce.org/products/products-virto3-versions")]
    Task<PromotionUsage[]> GetByIdsAsync(string[] ids);

    [Obsolete("Use SaveChangesAsync()", DiagnosticId = "VC0011", UrlFormat = "https://docs.virtocommerce.org/products/products-virto3-versions")]
    Task SaveUsagesAsync(PromotionUsage[] usages);

    [Obsolete("Use DeleteAsync()", DiagnosticId = "VC0011", UrlFormat = "https://docs.virtocommerce.org/products/products-virto3-versions")]
    Task DeleteUsagesAsync(string[] ids);
}
