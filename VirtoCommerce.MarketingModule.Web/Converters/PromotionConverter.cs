using System.Linq;
using Newtonsoft.Json;
using Omu.ValueInjecter;
using VirtoCommerce.Domain.Common;
using VirtoCommerce.Domain.Marketing.Model;
using VirtoCommerce.Domain.Marketing.Services;
using VirtoCommerce.MarketingModule.Data.Promotions;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Serialization;
using webModel = VirtoCommerce.MarketingModule.Web.Model;

namespace VirtoCommerce.MarketingModule.Web.Converters
{
    public static class PromotionConverter
    {
        public static webModel.Promotion ToWebModel(this Promotion promotion, PromoDynamicExpressionTree etalonEpressionTree = null)
        {
            var result = new webModel.Promotion();
            result.InjectFrom(promotion);

            // Workaround for UI: DynamicPromotion type is hardcoded in HTML template
            var dynamicPromotionType = typeof(DynamicPromotion);
            var promotionType = promotion.GetType();
            result.Type = dynamicPromotionType.IsAssignableFrom(promotionType)
                ? dynamicPromotionType.Name
                : promotionType.Name;

            var dynamicPromotion = promotion as DynamicPromotion;
            if (dynamicPromotion != null && etalonEpressionTree != null)
            {
                result.DynamicExpression = etalonEpressionTree;
                if (!string.IsNullOrEmpty(dynamicPromotion.PredicateVisualTreeSerialized))
                {
                    result.DynamicExpression = JsonConvert.DeserializeObject<PromoDynamicExpressionTree>(dynamicPromotion.PredicateVisualTreeSerialized);

                    // Copy available elements from etalon because they not persisted
                    var sourceBlocks = ((DynamicExpression)etalonEpressionTree).Traverse(x => x.Children);
                    var targetBlocks = ((DynamicExpression)result.DynamicExpression).Traverse(x => x.Children).ToList();

                    foreach (var sourceBlock in sourceBlocks)
                    {
                        foreach (var targetBlock in targetBlocks.Where(x => x.Id == sourceBlock.Id))
                        {
                            targetBlock.AvailableChildren = sourceBlock.AvailableChildren;
                        }
                    }

                    // Copy available elements from etalon
                    result.DynamicExpression.AvailableChildren = etalonEpressionTree.AvailableChildren;
                }
            }

            return result;
        }

        public static Promotion ToCoreModel(this webModel.Promotion promotion, IExpressionSerializer expressionSerializer)
        {
            var result = AbstractTypeFactory<DynamicPromotion>.TryCreateInstance();
            result.InjectFrom(promotion);

            //result.Coupons = promotion.Coupons;

            if (promotion.DynamicExpression?.Children != null)
            {
                var conditionExpression = promotion.DynamicExpression.GetConditionExpression();
                result.PredicateSerialized = expressionSerializer.SerializeExpression(conditionExpression);

                var rewards = promotion.DynamicExpression.GetRewards();
                result.RewardsSerialized = JsonConvert.SerializeObject(rewards, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All });

                // Clear availableElements in expression to decrease size
                promotion.DynamicExpression.AvailableChildren = null;
                var allBlocks = ((DynamicExpression)promotion.DynamicExpression).Traverse(x => x.Children);
                foreach (var block in allBlocks)
                {
                    block.AvailableChildren = null;
                }

                result.PredicateVisualTreeSerialized = JsonConvert.SerializeObject(promotion.DynamicExpression);
            }

            return result;
        }
    }
}
