using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using VirtoCommerce.CoreModule.Core.Common;
using VirtoCommerce.CoreModule.Core.Currency;
using VirtoCommerce.MarketingModule.Core.Model.Promotions;
using VirtoCommerce.MarketingModule.Core.Services;
using VirtoCommerce.MarketingModule.Data.Caching;
using VirtoCommerce.Platform.Core.Caching;

namespace VirtoCommerce.MarketingModule.Data.Services.EvaluationPolicies
{
    public abstract class PromotionPolicyBase : IMarketingPromoEvaluator
    {
        private readonly IPlatformMemoryCache _platformMemoryCache;
        private readonly ICurrencyService _currencyService;

        protected PromotionPolicyBase(
            IPlatformMemoryCache platformMemoryCache,
            ICurrencyService currencyService)
        {
            _platformMemoryCache = platformMemoryCache;
            _currencyService = currencyService;
        }

        public virtual async Task<PromotionResult> EvaluatePromotionAsync(IEvaluationContext context)
        {
            var promoContext = GetPromotionEvaluationContext(context);

            var cacheKey = CacheKey.With(GetType(), nameof(EvaluatePromotionAsync), string.Join("-", promoContext.GetCacheKey()));
            var result = await _platformMemoryCache.GetOrCreateExclusiveAsync(cacheKey, async (cacheEntry) =>
            {
                cacheEntry.SlidingExpiration = TimeSpan.FromMinutes(1);
                cacheEntry.AddExpirationToken(PromotionSearchCacheRegion.CreateChangeToken());

                promoContext.CurrencyObject = (await _currencyService.GetAllCurrenciesAsync()).First(x => x.Code == promoContext.Currency);

                return await EvaluatePromotionWithoutCache(promoContext);
            });

            return result;
        }

        protected abstract Task<PromotionResult> EvaluatePromotionWithoutCache(PromotionEvaluationContext promoContext);

        private static PromotionEvaluationContext GetPromotionEvaluationContext(IEvaluationContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (context is not PromotionEvaluationContext promoContext)
            {
                throw new ArgumentException($"{nameof(context)} type {context.GetType()} must be derived from PromotionEvaluationContext");
            }

            return promoContext;
        }
    }
}
