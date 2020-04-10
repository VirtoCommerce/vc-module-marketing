using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Microsoft.Extensions.Primitives;
using VirtoCommerce.MarketingModule.Core.Model.Promotions;
using VirtoCommerce.Platform.Core.Caching;

namespace VirtoCommerce.MarketingModule.Data.Caching
{
    public class PromotionUsageCacheRegion : CancellableCacheRegion<PromotionUsageCacheRegion>
    {
        private static readonly ConcurrentDictionary<string, CancellationTokenSource> _promotionUsageRegionTokenLookup = new ConcurrentDictionary<string, CancellationTokenSource>();

        public static IChangeToken CreateChangeToken(string[] usageIds)
        {
            if (usageIds == null)
            {
                throw new ArgumentNullException(nameof(usageIds));
            }

            var changeTokens = new List<IChangeToken>() { CreateChangeToken() };

            foreach (var usageId in usageIds.Distinct())
            {
                changeTokens.Add(new CancellationChangeToken(_promotionUsageRegionTokenLookup.GetOrAdd(usageId, new CancellationTokenSource()).Token));
            }

            return new CompositeChangeToken(changeTokens);
        }

        public static void ExpireUsages(PromotionUsage[] usages)
        {
            var usageIds = usages.Select(x => x.Id).ToArray();

            foreach (var usageId in usageIds)
            {
                if (_promotionUsageRegionTokenLookup.TryRemove(usageId, out var token))
                {
                    token.Cancel();
                }
            }
        }
    }
}
