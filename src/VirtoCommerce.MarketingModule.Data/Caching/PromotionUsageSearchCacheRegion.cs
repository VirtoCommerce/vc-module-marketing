using System;
using VirtoCommerce.Platform.Core.Caching;

namespace VirtoCommerce.MarketingModule.Data.Caching;

[Obsolete("Use GenericSearchCachingRegion<PromotionUsage>", DiagnosticId = "VC0011", UrlFormat = "https://docs.virtocommerce.org/products/products-virto3-versions")]
public class PromotionUsageSearchCacheRegion : CancellableCacheRegion<PromotionUsageCacheRegion>;
