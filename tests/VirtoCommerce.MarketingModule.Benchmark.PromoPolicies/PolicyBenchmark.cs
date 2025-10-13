using System.Collections.Generic;
using System.IO;
using System.Linq;
using BenchmarkDotNet.Attributes;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Newtonsoft.Json;
using VirtoCommerce.CoreModule.Core.Common;
using VirtoCommerce.CoreModule.Core.Conditions;
using VirtoCommerce.CoreModule.Core.Currency;
using VirtoCommerce.MarketingModule.Core.Model.DynamicContent;
using VirtoCommerce.MarketingModule.Core.Model.Promotions;
using VirtoCommerce.MarketingModule.Core.Model.Promotions.Search;
using VirtoCommerce.MarketingModule.Core.Promotions;
using VirtoCommerce.MarketingModule.Core.Search;
using VirtoCommerce.MarketingModule.Core.Services;
using VirtoCommerce.MarketingModule.Data.Services;
using VirtoCommerce.Platform.Caching;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.JsonConverters;

namespace VirtoCommerce.MarketingModule.Benchmark.PromoPolicies;

public class PolicyBenchmark
{
    private readonly PromotionEvaluationContext _context;
    private readonly Mock<IPromotionSearchService> _promoSearchServiceMock;
    private readonly PlatformMemoryCache _disabledPlatformMemoryCache;

    private readonly IMarketingPromoEvaluator _bestRewardPolicy;
    private readonly IMarketingPromoEvaluator _stackablePolicy;

    public PolicyBenchmark()
    {
        // Register discriminators for promotion rewards to allow custom json converters work correctly
        PolymorphJsonConverter.RegisterTypeForDiscriminator(typeof(PromotionReward), nameof(PromotionReward.Id));

        // Register the resulting trees expressions into the AbstractFactory<IConditionTree> to allow custom json converters work correctly
        foreach (var conditionTree in AbstractTypeFactory<PromotionConditionAndRewardTreePrototype>.TryCreateInstance().Traverse<IConditionTree>(x => x.AvailableChildren))
        {
            AbstractTypeFactory<IConditionTree>.RegisterType(conditionTree.GetType());
        }

        foreach (var conditionTree in AbstractTypeFactory<DynamicContentConditionTreePrototype>.TryCreateInstance().Traverse<IConditionTree>(x => x.AvailableChildren))
        {
            AbstractTypeFactory<IConditionTree>.RegisterType(conditionTree.GetType());
        }

        // Create disabled memory cache to test full calculations every policy call
        var memoryCache = new MemoryCache(Options.Create(new MemoryCacheOptions()));
        _disabledPlatformMemoryCache = new PlatformMemoryCache(memoryCache, Options.Create(new CachingOptions() { CacheEnabled = false }), new Mock<ILogger<PlatformMemoryCache>>().Object);


        // Mock evaluation context from a json file
        _context = MockPromotionEvaluationContext();

        // Mock promotions from a json file
        // If you want to have jsons like this from live system to benchmark different promotions combination, just call
        // in marketing module debug:
        // JsonConvert.SerializeObject(dynamicPromotion.DynamicExpression, new ConditionJsonConverter(doNotSerializeAvailCondition: true))
        // in immediate or watch
        _promoSearchServiceMock = new Mock<IPromotionSearchService>();
        _promoSearchServiceMock
            .Setup(x => x.SearchAsync(It.IsAny<PromotionSearchCriteria>(), It.IsAny<bool>()))
            .ReturnsAsync(MockPromotionSearchResult());


        _bestRewardPolicy = GetBestRewardPromotionPolicy();
        _stackablePolicy = GetCombineStackablePromotionPolicy();
    }


    private IMarketingPromoEvaluator GetBestRewardPromotionPolicy()
    {

        return new BestRewardPromotionPolicy(new Mock<ICurrencyService>().Object, _disabledPlatformMemoryCache, _promoSearchServiceMock.Object);
    }

    private IMarketingPromoEvaluator GetCombineStackablePromotionPolicy()
    {
        return new CombineStackablePromotionPolicy(
            new Mock<ICurrencyService>().Object,
            _disabledPlatformMemoryCache,
            _promoSearchServiceMock.Object,
            GetPromotionRewardEvaluatorMock().Object);
    }

    private static Mock<IPromotionRewardEvaluator> GetPromotionRewardEvaluatorMock()
    {
        var result = new Mock<IPromotionRewardEvaluator>();

        result
            .Setup(x => x.GetOrderedValidRewardsAsync(It.IsAny<IEnumerable<Promotion>>(), It.IsAny<IEvaluationContext>()))
            .Returns<IEnumerable<Promotion>, IEvaluationContext>(
                async (promotions, context) => await new DefaultPromotionRewardEvaluator().GetOrderedValidRewardsAsync(promotions, context)
            );

        return result;
    }

    private static PromotionEvaluationContext MockPromotionEvaluationContext()
    {
        return JsonConvert.DeserializeObject<PromotionEvaluationContext>(File.ReadAllText("promotion_evaluation_context_mock.json"));
    }

    private static PromotionSearchResult MockPromotionSearchResult()
    {
        var dynamicPromotions = JsonConvert.DeserializeObject<DynamicPromotion[]>(File.ReadAllText("promotions_mock.json"), new ConditionJsonConverter(), new PolymorphJsonConverter());

        var result = new PromotionSearchResult
        {
            Results = dynamicPromotions.Cast<Promotion>().ToList(),
            TotalCount = dynamicPromotions.Length,
        };

        return result;
    }

    /// <summary>
    /// Evaluate best reward promotion policy
    /// </summary>
    [Benchmark]
    public void EvaluateBestReward()
    {
        _bestRewardPolicy.EvaluatePromotionAsync(_context);
    }

    /// <summary>
    /// Evaluate stackable promotion policy
    /// </summary>
    [Benchmark]
    public void EvaluateStackable()
    {
        _stackablePolicy.EvaluatePromotionAsync(_context);
    }
}
