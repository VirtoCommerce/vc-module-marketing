using System.Collections.Generic;
using VirtoCommerce.Domain.Common;
using VirtoCommerce.Domain.Marketing.Model;

namespace VirtoCommerce.MarketingModule.Data.Services
{
    public interface IPromotionRewardEvaluator
    {
        IEnumerable<PromotionReward> GetOrderedValidRewards(IEnumerable<Promotion> promotions, IEvaluationContext context);
    }
}
