using System.Collections.Generic;
using System.IO;
using BenchmarkDotNet.Attributes;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Newtonsoft.Json;
using VirtoCommerce.CoreModule.Core.Common;
using VirtoCommerce.CoreModule.Core.Conditions;
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

namespace VirtoCommerce.MarketingModule.Benchmark.PromoPolicies
{
    public class PolicyBenchmark
    {
        private readonly PromotionEvaluationContext pCtx;
        private readonly Mock<IPromotionSearchService> promoSearchServiceMock;
        private readonly PlatformMemoryCache disabledPlatformMemoryCache;

        private readonly IMarketingPromoEvaluator _bestRewardPolicy;
        private readonly IMarketingPromoEvaluator _stackablePolicy;

        public PolicyBenchmark()
        {
            // Register discriminators for promotion rewards to allow custom jsonconverters work correctly
            PolymorphJsonConverter.RegisterTypeForDiscriminator(typeof(PromotionReward), nameof(PromotionReward.Id));

            // Register the resulting trees expressions into the AbstractFactory<IConditionTree> to allow custom jsonconverters work correctly
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
            disabledPlatformMemoryCache = new PlatformMemoryCache(memoryCache, Options.Create(new CachingOptions() { CacheEnabled = false }), new Mock<ILogger<PlatformMemoryCache>>().Object);


            // Mock evaluation context from pctx_mock.json file
            pCtx = MockPromotionEvaluationContext();

            // Mock promotions from promotions_mock.json file
            promoSearchServiceMock = new Moq.Mock<IPromotionSearchService>();
            promoSearchServiceMock.Setup(x => x.SearchPromotionsAsync(It.IsAny<PromotionSearchCriteria>())).ReturnsAsync(MockPromotionSearchResult());


            _bestRewardPolicy = GetBestRewardPromotionPolicy();
            _stackablePolicy = GetCombineStackablePromotionPolicy();
        }


        private IMarketingPromoEvaluator GetBestRewardPromotionPolicy()
        {

            return new BestRewardPromotionPolicy(promoSearchServiceMock.Object, disabledPlatformMemoryCache);
        }

        private IMarketingPromoEvaluator GetCombineStackablePromotionPolicy()
        {
            return new CombineStackablePromotionPolicy(promoSearchServiceMock.Object, GetPromotionRewardEvaluatorMock().Object, disabledPlatformMemoryCache);
        }


        private Mock<IPromotionRewardEvaluator> GetPromotionRewardEvaluatorMock()
        {
            var result = new Mock<IPromotionRewardEvaluator>();

            _ = result.Setup(x => x.GetOrderedValidRewardsAsync(It.IsAny<IEnumerable<Promotion>>(), It.IsAny<IEvaluationContext>()))
                .Returns<IEnumerable<Promotion>, IEvaluationContext>(
                    async (promotions, context) => await new DefaultPromotionRewardEvaluator().GetOrderedValidRewardsAsync(promotions, context)
                );

            return result;
        }

        private PromotionEvaluationContext MockPromotionEvaluationContext()
        {
            return JsonConvert.DeserializeObject<PromotionEvaluationContext>(File.OpenText("pctx_mock.json").ReadToEnd());
        }

        private PromotionSearchResult MockPromotionSearchResult()
        {
            var result = new PromotionSearchResult();
            result.Results = JsonConvert.DeserializeObject<DynamicPromotion[]>(File.OpenText("promotions_mock.json").ReadToEnd(), new ConditionJsonConverter(), new PolymorphJsonConverter());
            result.TotalCount = result.Results.Count;

            return result;
        }

        /// <summary>
        /// Evaluate best reward promotion policy
        /// </summary>
        [Benchmark]
        public void EvaluateBestReward()
        {
            _bestRewardPolicy.EvaluatePromotionAsync(pCtx);
        }

        /// <summary>
        /// Evaluate stackable promotion policy
        /// </summary>
        [Benchmark]
        public void EvaluateStackable()
        {
            _stackablePolicy.EvaluatePromotionAsync(pCtx);
        }

    }
}
