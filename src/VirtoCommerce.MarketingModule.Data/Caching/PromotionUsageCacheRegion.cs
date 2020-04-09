using System.Collections.Concurrent;
using System.Threading;
using Microsoft.Extensions.Primitives;
using VirtoCommerce.Platform.Core.Caching;

namespace VirtoCommerce.MarketingModule.Data.Caching
{
    public class PromotionUsageCacheRegion : CancellableCacheRegion<PromotionUsageCacheRegion>
    {
        private static readonly ConcurrentDictionary<string, CancellationTokenSource> _promotionUsageRegionTokenLookup = new ConcurrentDictionary<string, CancellationTokenSource>();

        public static IChangeToken CreateChangeToken(string promotionId)
        {
            var cancellationTokenSource = _promotionUsageRegionTokenLookup.GetOrAdd(promotionId, new CancellationTokenSource());
            return new CompositeChangeToken(new[] { CreateChangeToken(), new CancellationChangeToken(cancellationTokenSource.Token) });
        }

        public static void ExpireUsages(params string[] promotionIds)
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
