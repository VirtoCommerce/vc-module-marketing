using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Primitives;
using VirtoCommerce.Platform.Core.Caching;

namespace VirtoCommerce.MarketingModule.Data.Caching
{
    public class PromotionCacheRegion : CancellableCacheRegion<PromotionCacheRegion>
    {
        public static IChangeToken CreateChangeToken(string[] promotionIds)
        {
            if (promotionIds == null)
            {
                throw new ArgumentNullException(nameof(promotionIds));
            }

            var changeTokens = new List<IChangeToken>() { CreateChangeToken() };

            foreach (var promotionId in promotionIds.Distinct())
            {
                changeTokens.Add(CreateChangeTokenForKey(promotionId));
            }

            return new CompositeChangeToken(changeTokens);
        }

        public static void ExpirePromotions(string[] promotionIds)
        {
            foreach (var promotionId in promotionIds)
            {
                ExpireTokenForKey(promotionId);
            }
        }
    }
}
