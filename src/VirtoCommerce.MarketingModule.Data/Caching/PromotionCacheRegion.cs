using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Microsoft.Extensions.Primitives;
using VirtoCommerce.Platform.Core.Caching;

namespace VirtoCommerce.MarketingModule.Data.Caching
{
    public class PromotionCacheRegion : CancellableCacheRegion<PromotionCacheRegion>
    {
        private static readonly ConcurrentDictionary<string, CancellationTokenSource> _promotionUsageRegionTokenLookup = new ConcurrentDictionary<string, CancellationTokenSource>();

        public static IChangeToken CreateChangeToken(string[] promotionIds)
        {
            if (promotionIds == null)
            {
                throw new ArgumentNullException(nameof(promotionIds));
            }

            var changeTokens = new List<IChangeToken>() { CreateChangeToken() };

            foreach (var promotionId in promotionIds.Distinct())
            {
                changeTokens.Add(new CancellationChangeToken(_promotionUsageRegionTokenLookup.GetOrAdd(promotionId, new CancellationTokenSource()).Token));
            }

            return new CompositeChangeToken(changeTokens);
        }

        public static void ExpirePromotions(string[] promotionIds)
        {
            foreach (var promotionId in promotionIds)
            {
                if (_promotionUsageRegionTokenLookup.TryRemove(promotionId, out var token))
                {
                    token.Cancel();
                }
            }
        }
    }
}
