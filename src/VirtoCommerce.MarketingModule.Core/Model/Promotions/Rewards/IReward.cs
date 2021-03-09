using System.Collections.Generic;

namespace VirtoCommerce.MarketingModule.Core.Model.Promotions
{
    public interface IReward
    {
        IEnumerable<PromotionReward> GetRewards();
    }
}
