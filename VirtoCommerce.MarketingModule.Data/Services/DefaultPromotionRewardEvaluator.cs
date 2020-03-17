using System.Collections.Generic;
using System.Linq;
using VirtoCommerce.Domain.Common;
using VirtoCommerce.Domain.Marketing.Model;

namespace VirtoCommerce.MarketingModule.Data.Services
{
    public class DefaultPromotionRewardEvaluator : IPromotionRewardEvaluator
    {
        public virtual IEnumerable<PromotionReward> GetOrderedValidRewards(IEnumerable<Promotion> promotions, IEvaluationContext context)
        {
            var result = promotions.SelectMany(x => x.EvaluatePromotion(context))
                        .OrderByDescending(x => x.Promotion.IsExclusive)
                        .ThenByDescending(x => x.Promotion.Priority)
                        .Where(x => x.IsValid)
                        .ToList();

            return result;
        }
    }
}
