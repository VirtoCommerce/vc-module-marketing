using System.Collections.Generic;
using System.Linq;
using VirtoCommerce.CoreModule.Core.Conditions;

namespace VirtoCommerce.MarketingModule.Core.Model.Promotions
{
    public class BlockReward : ConditionTree, IReward
    {
        #region IRewardsExpression Members

        public IEnumerable<PromotionReward> GetRewards()
        {
            var result = Enumerable.Empty<PromotionReward>();

            if (Children != null)
            {
                result = Children.OfType<IReward>().SelectMany(x => x.GetRewards());
            }
            return result;
        }

        #endregion       
    }
}
