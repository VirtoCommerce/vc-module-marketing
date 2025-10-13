using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Primitives;
using VirtoCommerce.CoreModule.Core.Common;
using VirtoCommerce.CoreModule.Core.Currency;
using VirtoCommerce.MarketingModule.Core.Model.Promotions;
using VirtoCommerce.MarketingModule.Core.Services;
using VirtoCommerce.Platform.Caching;
using VirtoCommerce.Platform.Core.Caching;

namespace VirtoCommerce.MarketingModule.Data.Services.EvaluationPolicies;

public abstract class PromotionPolicyBase(
    ICurrencyService currencyService,
    IPlatformMemoryCache platformMemoryCache)
    : IMarketingPromoEvaluator
{
    public virtual async Task<PromotionResult> EvaluatePromotionAsync(IEvaluationContext context)
    {
        var promoContext = GetPromotionEvaluationContext(context);

        var cacheKey = CacheKey.With(GetType(), nameof(EvaluatePromotionAsync), promoContext.GetCacheKey());
        var result = await platformMemoryCache.GetOrCreateExclusiveAsync(cacheKey, async cacheOptions =>
        {
            cacheOptions.SlidingExpiration = TimeSpan.FromMinutes(1);
            cacheOptions.AddExpirationToken(CreateCacheToken(promoContext));

            promoContext.CurrencyObject = (await currencyService.GetAllCurrenciesAsync()).First(x => x.Code == promoContext.Currency);

            return await EvaluatePromotionWithoutCache(promoContext);
        });

        return result;
    }

    protected abstract Task<PromotionResult> EvaluatePromotionWithoutCache(PromotionEvaluationContext promoContext);

    protected virtual IChangeToken CreateCacheToken(PromotionEvaluationContext promoContext)
    {
        return GenericSearchCachingRegion<Promotion>.CreateChangeToken();
    }

    private static PromotionEvaluationContext GetPromotionEvaluationContext(IEvaluationContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        if (context is not PromotionEvaluationContext promoContext)
        {
            throw new ArgumentException($"{nameof(context)} type {context.GetType()} must be derived from PromotionEvaluationContext");
        }

        return promoContext;
    }
}
