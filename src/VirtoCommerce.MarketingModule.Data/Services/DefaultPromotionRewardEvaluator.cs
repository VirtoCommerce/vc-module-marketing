using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VirtoCommerce.CoreModule.Core.Common;
using VirtoCommerce.MarketingModule.Core.Model.Promotions;
using VirtoCommerce.MarketingModule.Core.Services;

namespace VirtoCommerce.MarketingModule.Data.Services
{
    public class DefaultPromotionRewardEvaluator : IPromotionRewardEvaluator
    {
        public virtual async Task<IEnumerable<PromotionReward>> GetOrderedValidRewardsAsync(IEnumerable<Promotion> promotions, IEvaluationContext context)
        {
            var promotionEvaluationTasks = promotions.Select(x => x.EvaluatePromotionAsync(context)).ToArray();

            await Task.WhenAll(promotionEvaluationTasks);

            var result = promotionEvaluationTasks
                .SelectMany(x => x.Result)
                .OrderByDescending(x => x.Promotion.IsExclusive)
                .ThenByDescending(x => x.Promotion.Priority)
                .Where(x => x.IsValid)
                .ToList();

            return result;

        }
    }
}
