using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Primitives;
using VirtoCommerce.MarketingModule.Core.Model.Promotions;
using VirtoCommerce.Platform.Core.Caching;

namespace VirtoCommerce.MarketingModule.Data.Caching;

[Obsolete("Use GenericCachingRegion<PromotionUsage>", DiagnosticId = "VC0011", UrlFormat = "https://docs.virtocommerce.org/products/products-virto3-versions")]
public class PromotionUsageCacheRegion : CancellableCacheRegion<PromotionUsageCacheRegion>
{
    public static IChangeToken CreateChangeToken(string[] usageIds)
    {
        if (usageIds == null)
        {
            throw new ArgumentNullException(nameof(usageIds));
        }

        var changeTokens = new List<IChangeToken>() { CreateChangeToken() };

        foreach (var usageId in usageIds.Distinct())
        {
            changeTokens.Add(CreateChangeTokenForKey(usageId));
        }

        return new CompositeChangeToken(changeTokens);
    }

    public static void ExpireUsages(PromotionUsage[] usages)
    {
        var usageIds = usages.Select(x => x.Id).ToArray();

        foreach (var usageId in usageIds)
        {
            ExpireTokenForKey(usageId);
        }
    }
}
