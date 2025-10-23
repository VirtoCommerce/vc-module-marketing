using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VirtoCommerce.CoreModule.Core.Common;
using VirtoCommerce.MarketingModule.Core.Model.Promotions;

namespace VirtoCommerce.MarketingModule.Test.CustomPromotion;

public class BuyProductWithTagPromotion(string[] tags, decimal discountAmount) : Promotion
{
    public override Task<IList<PromotionReward>> EvaluatePromotionAsync(IEvaluationContext context)
    {
        var rewards = new List<PromotionReward>();

        if (context is PromotionEvaluationContext promoContext)
        {
            foreach (var entry in promoContext.PromoEntries)
            {
                var tag = entry.Attributes?["tag"];

                var reward = new CatalogItemAmountReward
                {
                    AmountType = RewardAmountType.Relative,
                    Amount = discountAmount,
                    IsValid = !string.IsNullOrEmpty(tag) && tags.Contains(tag),
                    ProductId = entry.ProductId,
                    Promotion = this,
                };

                rewards.Add(reward);
            }
        }

        return Task.FromResult<IList<PromotionReward>>(rewards);
    }
}
