using System;
using System.Threading.Tasks;
using VirtoCommerce.MarketingModule.Core.Model.Promotions;
using VirtoCommerce.MarketingModule.Core.Model.Promotions.Search;
using VirtoCommerce.Platform.Core.GenericCrud;

namespace VirtoCommerce.MarketingModule.Core.Search;

public interface IPromotionUsageSearchService : ISearchService<PromotionUsageSearchCriteria, PromotionUsageSearchResult, PromotionUsage>
{
    [Obsolete("Use SearchAsync()", DiagnosticId = "VC0011", UrlFormat = "https://docs.virtocommerce.org/products/products-virto3-versions")]
    Task<PromotionUsageSearchResult> SearchUsagesAsync(PromotionUsageSearchCriteria criteria);
}
