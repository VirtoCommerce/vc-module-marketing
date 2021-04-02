using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using VirtoCommerce.CoreModule.Core.Common;
using VirtoCommerce.MarketingModule.Core.Model.Promotions;
using VirtoCommerce.MarketingModule.Core.Services;
using VirtoCommerce.MarketingModule.Data.Caching;
using VirtoCommerce.Platform.Core.Caching;

namespace VirtoCommerce.MarketingModule.Data.Services.EvaluationPolicies
{
    public abstract class BasePromotionPolicy : IMarketingPromoEvaluator
    {
        private readonly IPlatformMemoryCache _platformMemoryCache;

        public BasePromotionPolicy(IPlatformMemoryCache platformMemoryCache)
        {
            _platformMemoryCache = platformMemoryCache;
        }

        public virtual async Task<PromotionResult> EvaluatePromotionAsync(IEvaluationContext context)
        {
            var promoContext = GetPromotionEvaluationContext(context);

            var cacheKey = CacheKey.With(GetType(), nameof(EvaluatePromotionAsync), string.Join("-", promoContext.GetCacheKey()));
            var result = await _platformMemoryCache.GetOrCreateExclusiveAsync(cacheKey, async (cacheEntry) =>
            {
                cacheEntry.SlidingExpiration = TimeSpan.FromMinutes(1);
                cacheEntry.AddExpirationToken(PromotionSearchCacheRegion.CreateChangeToken());

                return await EvaluatePromotionCachelessAsync(promoContext);
            });

            return result;
        }

        protected abstract Task<PromotionResult> EvaluatePromotionCachelessAsync(PromotionEvaluationContext promoContext);

        private static PromotionEvaluationContext GetPromotionEvaluationContext(IEvaluationContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (!(context is PromotionEvaluationContext promoContext))
            {
                throw new ArgumentException($"{nameof(context)} type {context.GetType()} must be derived from PromotionEvaluationContext");
            }

            return promoContext;
        }
    }
}
