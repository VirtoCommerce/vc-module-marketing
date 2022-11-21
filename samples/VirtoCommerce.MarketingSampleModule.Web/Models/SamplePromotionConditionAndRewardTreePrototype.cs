using System.Linq;
using VirtoCommerce.MarketingModule.Core.Model.Promotions;
using VirtoCommerce.MarketingModule.Core.Model.Promotions.Conditions;

namespace VirtoCommerce.MarketingSampleModule.Web.Models
{
    public sealed class SamplePromotionConditionAndRewardTreePrototype : PromotionConditionAndRewardTreePrototype
    {
        public SamplePromotionConditionAndRewardTreePrototype()
        {
            //Extend existing 'If any of these catalog condition' block with a new condition element
            var blockCatalogCondition = AvailableChildren.OfType<BlockCatalogCondition>().FirstOrDefault();
            blockCatalogCondition.AvailableChildren.Add(new SampleCondition());

            // Add a new block with sample condition to the beginning of the tree
            var blockSampleConditions = new BlockSampleCondition().WithAvailConditions(new SampleCondition());
            AvailableChildren.Insert(0, blockSampleConditions);
            Children.Insert(0, blockSampleConditions);
        }
    }
}
