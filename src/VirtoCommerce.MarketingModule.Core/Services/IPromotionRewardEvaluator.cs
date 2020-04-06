using System.Collections.Generic;
using System.Threading.Tasks;
using VirtoCommerce.CoreModule.Core.Common;
using VirtoCommerce.MarketingModule.Core.Model.Promotions;

namespace VirtoCommerce.MarketingModule.Core.Services
{
    public interface IPromotionRewardEvaluator
    {
        Task<IEnumerable<PromotionReward>> GetOrderedValidRewardsAsync(IEnumerable<Promotion> promotions, IEvaluationContext context);
    }
}
