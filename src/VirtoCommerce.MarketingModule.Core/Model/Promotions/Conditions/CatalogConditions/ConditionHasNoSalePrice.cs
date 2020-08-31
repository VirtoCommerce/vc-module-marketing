using VirtoCommerce.CoreModule.Core.Common;
using VirtoCommerce.CoreModule.Core.Conditions;

namespace VirtoCommerce.MarketingModule.Core.Model.Promotions.Conditions
{
    public class ConditionHasNoSalePrice : ConditionTree
    {
        /// <summary>
        /// ((PromotionEvaluationContext)x).HasNoSalePrice()
        /// </summary>
        public override bool IsSatisfiedBy(IEvaluationContext context)
        {
            var result = false;
            if (context is PromotionEvaluationContext promotionEvaluationContext)
            {
                if (promotionEvaluationContext.PromoEntry != null)
                {
                    result = promotionEvaluationContext.PromoEntry.Price == promotionEvaluationContext.PromoEntry.ListPrice;
                }
            }

            return result;
        }
    }
}
