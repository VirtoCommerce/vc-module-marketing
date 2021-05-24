using VirtoCommerce.MarketingModule.Core.Model.Promotions;

namespace VirtoCommerce.MarketingSampleModule.Web.Models
{
    public sealed class SamplePromotionConditionAndRewardTreePrototype : PromotionConditionAndRewardTreePrototype
    {
        public SamplePromotionConditionAndRewardTreePrototype()
        {
            // Add sample condition to the beginning of the tree
            var blockSampleConditions = new BlockSampleCondition().WithAvailConditions(new SampleCondition());
            AvailableChildren.Insert(0, blockSampleConditions);
            Children.Insert(0, blockSampleConditions);
        }
    }
}
